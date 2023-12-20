using Hexagon.Model.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public class PaletteClass : Base
    {
        /// <summary>
        /// Informa el tipo de dato para el que se usará la paleta. En caso de MemberNumber >0 se descarta Datalimits.
        /// Esto se podría intentar calcular pero  la diferencia entre secuencial y divergente no es unívoca
        /// </summary>
        /// <param name="EnumPaletteClass">Clase de paleta</param>
        /// <param name="MemberNumber">Cantidad de Clases</param>
        /// <param name="DataRange">Rango de las clases</param>
        public EnumPaletteClass EnumPaletteClass { get; set; }
        public int MemberNumber { get; set; }
        public Dictionary<int, Color> RGBS { get; set; } = new Dictionary<int, Color>();
        public string Palette { get; set; }
        
        public override string Name { get { return Palette + "_" + this.EnumPaletteClass.ToString() + "_" + MemberNumber.ToString(); }  set { }  } 
        

    }
}