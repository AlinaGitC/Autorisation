using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorisation.Models
{
    public class Userr
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int ID_Role { get; set; }
        [ForeignKey("ID_Role")]
        public Role Role { get; set; }
    }
}
