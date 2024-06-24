using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraftFinal4.Data
{
    public class MVCKhumaloCraftFinal4Context : DbContext
    {
        public MVCKhumaloCraftFinal4Context (DbContextOptions<MVCKhumaloCraftFinal4Context> options)
            : base(options)
        {
        }

        public DbSet<MVCKhumaloCraftFinal4.Models.User> User { get; set; } = default!;
        public DbSet<MVCKhumaloCraftFinal4.Models.Product> Product { get; set; } = default!;
        public DbSet<MVCKhumaloCraftFinal4.Models.Order> Order { get; set; } = default!;
        public DbSet<MVCKhumaloCraftFinal4.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<MVCKhumaloCraftFinal4.Models.Admin> Admin { get; set; } = default!;
    }
}
