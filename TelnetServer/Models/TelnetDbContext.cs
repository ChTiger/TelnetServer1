using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelnetServer.Models
{
    class TelnetDbContext : DbContext
    {
        public TelnetDbContext()
            : base("name=dblink")
        {
        }
        public DbSet<Ip> Ip { get; set; }

    }
}

