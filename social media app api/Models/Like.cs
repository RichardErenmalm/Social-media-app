namespace social_media_app_api
{
    public class Like
    {
        public int LikeId { get; set; }
        public DateTime PublicationTime { get; set; } = DateTime.Now;

        public int LikedById { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }
    }
}