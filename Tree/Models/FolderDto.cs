namespace Tree.Models
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FolderDto>? SubFolders { get; set; } = new List<FolderDto>();
        public List<FileDto>? Files { get; set; } = new List<FileDto>();
        public string Path { get; set; }
        
    }
}
