using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    public interface IFormulasResumen
    {
        List<Shared.DTOs.FunctionDTO> FormulasDisponibles(string ClassName );
        string[] ClasesDisponibles(string Path);

    }
}
