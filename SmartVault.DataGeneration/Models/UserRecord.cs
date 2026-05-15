using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration.Models
{
    public class UserRecord
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
    }

}
