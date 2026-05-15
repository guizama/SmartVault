using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration.Models
{
    public class DocumentRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public long Length { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
