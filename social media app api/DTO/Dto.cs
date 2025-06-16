using System.Text.Json.Serialization;

namespace social_media_app_api.DTO
{
    public class RegisterDto
    {
        public string Name { get; set; } // public name
        public string Username { get; set; } // public username
        public string Gmail { get; set; } // public email   
        public string Password { get; set; } // public password
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class PostDto
    {
        public int PostId { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
        public DateTime PublicationTime { get; set; }
        public int PublisherId { get; set; }
    }

    public class CreatePostDto
    {
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
        public int UserId { get; set; }
    }

    public class CreateCommentDto
    {
        public string? Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateCommentDto
    {
        public string? Text { get; set; }
    }

    public class LikeDto
    {
        public int LikeId { get; set; }
        public DateTime PublicationTime { get; set; }
        public int LikedById { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
    }

    public class CreateLikeDto
    {
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int LikedById { get; set; }
    }

    public class QuoteDto
    {
        [JsonPropertyName("q")]
        public string Content { get; set; } = string.Empty;
        [JsonPropertyName("a")]
        public string Author { get; set; } = string.Empty;
    }
}
