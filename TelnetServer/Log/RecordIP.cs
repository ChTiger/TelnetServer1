using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelnetServer.Models;

namespace TelnetServer.Log
{
    public static class RecordIP
    {

        public static void recordip(Ip ip)
        {
            using (var TelnetDbContext = new TelnetDbContext())
            {
                bool res = TelnetDbContext.Database.CreateIfNotExists();
                TelnetDbContext.Ip.Add(ip);
                TelnetDbContext.SaveChanges();
            }
        }
    }
}
