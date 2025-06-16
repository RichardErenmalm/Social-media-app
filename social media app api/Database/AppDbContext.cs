
using Microsoft.EntityFrameworkCore;

namespace social_media_app_api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Comment <-> Post
            modelBuilder.Entity<Comment>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment <-> User
            modelBuilder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Like <-> User
            modelBuilder.Entity<Like>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.LikedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Like <-> Post
            modelBuilder.Entity<Like>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Like <-> Comment
            modelBuilder.Entity<Like>()
                .HasOne<Comment>()
                .WithMany()
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
