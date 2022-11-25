using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    public interface IFormulasResumen
    {
        List<Function> FormulasDisponibles(string ClassName);
        string[] ClasesDisponibles { get; }

    }
}
