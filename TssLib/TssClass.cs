using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TssLib
{
    public enum TipoDescuentoTss
    {
        SFS,
        AFP
    }
    public class TssConfig
    {
        public decimal SalarioMinimo { get; set; }
        public decimal CantidadSalariosSFS { get; set; }
        public decimal CantidadSalariosAFP { get; set; }
        public decimal PorcentajeSFS { get; set; }
        public decimal PorcentajeAFP { get; set; }
        public decimal Tope(TipoDescuentoTss tipo)
        {
            switch(tipo)
            {
                case TipoDescuentoTss.SFS:
                    return CantidadSalariosSFS * SalarioMinimo;
                case TipoDescuentoTss.AFP:
                    return CantidadSalariosAFP * SalarioMinimo;
                default:
                    return 0;
            }
        }

        public decimal Porcentaje(TipoDescuentoTss tipo)
        {
            switch (tipo)
            {
                case TipoDescuentoTss.SFS:
                    return PorcentajeSFS;
                case TipoDescuentoTss.AFP:
                    return PorcentajeAFP;
                default:
                    return 0;
            }
        }

        public static TssConfig Default()
        {
            return new TssConfig
            {
                CantidadSalariosAFP = 20,
                CantidadSalariosSFS = 10,
                SalarioMinimo = 9855.00m,
                PorcentajeAFP = 2.87m,
                PorcentajeSFS = 3.04m
            };
        }
    }

    public class DecuentoTssItem
    {
        public TipoDescuentoTss Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }

    public class TssUtils
    {
        static readonly Dictionary<TipoDescuentoTss, string> descrp = new Dictionary<TipoDescuentoTss, string>
        {
            {TipoDescuentoTss.AFP,"ADMINITRADORA DE FONDOS DE PENSIONES"},
            { TipoDescuentoTss.SFS,"SEGURO FAMILIAR DE SALUD"}
        };

        private static decimal Calcular(decimal sueldo, TipoDescuentoTss tipo, TssConfig configOpts = null)
        {

            TssConfig _config = configOpts;
            if (_config == null)
                _config = TssConfig.Default();
            decimal valor = sueldo;
            if (sueldo > _config.Tope(tipo))
                valor = _config.Tope(tipo);

            return valor * (_config.Porcentaje(tipo));
        }


        public static List<DecuentoTssItem> DescuentosTss(decimal sueldo, TssConfig config)
        {
            List<DecuentoTssItem> items = new List<DecuentoTssItem>();
            items.Add(new DecuentoTssItem
            {
                Id = TipoDescuentoTss.AFP,
                Descripcion = descrp[TipoDescuentoTss.AFP],
                Monto = Calcular(sueldo, TipoDescuentoTss.AFP, config)
            });
            items.Add(new DecuentoTssItem
            {
                Id = TipoDescuentoTss.SFS,
                Descripcion = descrp[TipoDescuentoTss.SFS],
                Monto = Calcular(sueldo, TipoDescuentoTss.SFS, config)
            });
            return items;
        }

        public static List<DecuentoTssItem> DescuentosTss(decimal sueldo)
        {
            return DescuentosTss(sueldo, null);
        }

        public static List<DecuentoTssItem> DescuentosTssFromJsonFile(decimal sueldo, string jsonFile)
        {
            TssConfig config = new Aportes().JsonToConfig(jsonFile);
            return DescuentosTss(sueldo, config);
        }

    }
}
