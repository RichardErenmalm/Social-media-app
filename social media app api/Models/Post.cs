namespace social_media_app_api
{
    public class Post
    {
        public int PostId { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
        public DateTime PublicationTime { get; set; } = DateTime.Now;
        public int PublisherId { get; set; }
    }
}
