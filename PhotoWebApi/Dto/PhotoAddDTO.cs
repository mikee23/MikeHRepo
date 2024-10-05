namespace PhotoWebApi.Dto
{
    public class PhotoAddDTO
    {
        public string ImageURL { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? Title { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
    }
}
