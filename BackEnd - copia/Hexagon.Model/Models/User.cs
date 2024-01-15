﻿using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class User : Equatable<User>
    {

        public string Password { get; set; }
        public UserSelectedOpctions UserSelectedOpctions { get; set; }
        public virtual ProyectData Projects { get; set; }
    }
}