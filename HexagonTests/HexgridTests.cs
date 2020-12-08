using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGNapoleonics.HexUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace PGNapoleonics.HexUtilities.Tests
{
    [TestClass()]
    public class HexgridTests
    {
        [TestMethod()]
        public void HexgridTest()
        {
            try
            {
                Size GridSize = new Size(10, 10);
                Hexgrid hexgrid = new Hexgrid(false, GridSize, 1);
                Point p = HexCoords.HexOrigin(GridSize, 2, 1);

                

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}