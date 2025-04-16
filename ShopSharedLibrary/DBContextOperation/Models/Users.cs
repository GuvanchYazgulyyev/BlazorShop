using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSharedLibrary.DBContextOperation.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EMailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
