namespace PocViseu.Api.Middleware
{
    public class FileModel
    {
        public string fileName { get; set; }
        public IFormFile formFile { get; set; }
        public Int64? ProcessId { get; set; }
        public int? Subitem { get; set; }
    }
}
