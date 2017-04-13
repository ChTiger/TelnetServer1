using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelnetServer.Models
{
    public class Ip
    {
        public Ip()
        {
            Creatime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public string IP { get; set; }
        public string SessionId { get; set; }
        public string Action { get; set; }
        public DateTime Creatime { get; set; }
        public string remark { get; set; }


    }


}
