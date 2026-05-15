using System;
using System.Linq;

namespace SmartVault.DataGeneration.Services
{
    public class DocumentGeneratorService
    {
        public string Generate()
        {
            return string.Join(Environment.NewLine, Enumerable.Repeat("This is my test document", 100));
        }
    }
}
