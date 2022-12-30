using Hexagon.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Hexagon.Model;
using Hexagon.Services.CalcStrategy;
using Hexagon.Shared.DTOs;
using System.Linq;

namespace Hexagon.Services
{
    public class FormulasResumenService : IFormulasResumen
    {
        private IConfiguration _Configuration;
        public FormulasResumenService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public string[] ClasesDisponibles (string path)
        {

                string PathFunctions = path;
                return  Directory.GetFiles(PathFunctions);
                
            
        }


        public List<FunctionDTO> FormulasDisponibles(string PathFunction)
        {
            var clases = this.ClasesDisponibles(PathFunction );
            List<FunctionDTO> functions = new List<FunctionDTO>();
            foreach (var clase in clases)
            {

                string FullClassName = Path.Combine(PathFunction, clase);
                Assembly Assembly = Assembly.LoadFrom(FullClassName);
                List<string> Metodos = new List<string>();
                // Obtain a reference to a method known to exist in assembly.

                functions.AddRange(DoCalc.GetFunctions(FullClassName).Select(x => new FunctionDTO(x.Path, x.FullClassName, x.FunctionName, x.Types)).ToList())
                        ;
            }
            return functions;
            // Obtain a reference to the parameters collection of the MethodInfo instance.
        }
    }
}
