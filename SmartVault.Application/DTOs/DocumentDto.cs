namespace SmartVault.Application.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;
    }
}