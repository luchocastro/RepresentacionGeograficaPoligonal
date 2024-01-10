using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Hexagon.Model.Repository
{
    public class Equatable<T> : Base,  IEquatable<T> where T : Base
    {
        public bool Equals([AllowNull] T other)
        {
            if (this.ID == other.ID)
                return true;
            return false;
        }
    }
}
