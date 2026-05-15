using System;

namespace SmartVault.DataGeneration.Models
{
    public class AccountRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
    }

}
