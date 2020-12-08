namespace Hexagon.Model
{
    public struct PaletteClass
    {
        /// <summary>
        /// Informa el tipo de dato para el que se usará la paleta. En caso de MemberNumber >0 se descarta Datalimits.
        /// Esto se podría intentar calcular pero  la diferencia entre secuencial y divergente no es unívoca
        /// </summary>
        /// <param name="EnumPaletteClass">Clase de paleta</param>
        /// <param name="MemberNumber">Cantidad de Clases</param>
        /// <param name="DataRange">Rango de las clases</param>
        public PaletteClass(EnumPaletteClass EnumPaletteClass, int MemberNumber = 0, float[] DataRange=null)
        {
            this.EnumPaletteClass = EnumPaletteClass;
            this.MemberNumber = MemberNumber;
            this.DataRange = DataRange;
        }
        public EnumPaletteClass EnumPaletteClass { get; }
        public int MemberNumber { get; }
        public float[] DataRange { get; }
    }
}