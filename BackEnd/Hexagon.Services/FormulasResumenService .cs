using Hexagon.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Hexagon.Model;
using Hexagon.Services.CalcStrategy;

namespace Hexagon.Services
{
    public class FormulasResumenService : IFormulasResumen
    {
        private IConfiguration _Configuration;
        public FormulasResumenService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public string[] ClasesDisponibles
        {
            get {
                string PathFunctions = _Configuration.GetSection("PathFunctions").Value;
                return  Directory.GetFiles(PathFunctions);
                
            }
        }


        public List<Function> FormulasDisponibles(string FullClassName)
        {
            Assembly Assembly = Assembly.LoadFrom(FullClassName);
            List<string> Metodos = new List<string>();
            // Obtain a reference to a method known to exist in assembly.
            string PathFunctions = _Configuration.GetSection("PathFunctions").Value;

            return DoCalc.GetFunctions (PathFunctions,FullClassName);
            // Obtain a reference to the parameters collection of the MethodInfo instance.
        }
    }
}
