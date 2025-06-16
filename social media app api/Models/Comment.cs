namespace social_media_app_api
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int PublisherId { get; set; }
        public int PostId { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime PublicationTime { get; set; } = DateTime.Now;
    }
}
