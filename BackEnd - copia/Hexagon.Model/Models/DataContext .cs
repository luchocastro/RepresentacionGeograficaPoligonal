using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{

    public class DataContext : DbContext
    {
        DbContextOptions<DataContext> _options;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
            _options = options;
        }
            public DbSet<User> User { get ; set; }

    
    }
}
