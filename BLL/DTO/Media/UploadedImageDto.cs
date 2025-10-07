namespace BLL.DTO.Media
{
    public class UploadedImageDto
    {
        public string Url { get; set; } = "";
        public string PublicId { get; set; } = "";
        public int SortOrder { get; set; } = 0;
    }
}
