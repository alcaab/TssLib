using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TssLib
{

    public class Aportes
    {
        public Aportes()
        {

        }
        public TssConfig JsonToConfig(string configFile)
        {
            var json = File.ReadAllText(configFile);
            Aportes aportes = JsonConvert.DeserializeObject<Aportes>(json);
            return new TssConfig
            {
                CantidadSalariosAFP = aportes.AFP.Tope,
                PorcentajeAFP = aportes.AFP.Empleado,
                CantidadSalariosSFS = aportes.SFS.Tope,
                PorcentajeSFS = aportes.SFS.Empleado,
                SalarioMinimo = aportes.SalarioMinimoPromedio
            };
        }
        public decimal SalarioMinimoPromedio { get; set; }
        public SFS SFS { get; set; }
        public AFP AFP { get; set; }
        public SRL SRL { get; set; }

    }

    public class SFS 
    {
        public decimal Tope { get; set; }
        public decimal Empleador { get; set; }
        public decimal Empleado { get; set; }
    }

    public class AFP 
    {
        public decimal Tope { get; set; }
        public decimal Empleador { get; set; }
        public decimal Empleado { get; set; }
    }

    public class SRL
    {
        public decimal Tope { get; set; }
        public decimal Empleador { get; set; }
        public decimal Empleado { get; set; }
    }
}
