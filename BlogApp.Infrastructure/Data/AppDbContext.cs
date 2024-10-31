using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCommentRelationships(modelBuilder);
            ConfigurePostTagRelationships(modelBuilder);
            ConfigurePostCategoryRelationship(modelBuilder);
            ConfigurePostImageRelationship(modelBuilder);

            // Seed data for Categories
            modelBuilder.Entity<Category>().HasData(
                 new Category { CategoryId = 1, Name = "Technology" },
                 new Category { CategoryId = 2, Name = "Lifestyle" },
                 new Category { CategoryId = 3, Name = "Education" },
                 new Category { CategoryId = 4, Name = "Health & Fitness" },
                 new Category { CategoryId = 5, Name = "Finance" },
                 new Category { CategoryId = 6, Name = "Travel" },
                 new Category { CategoryId = 7, Name = "Food" },
                 new Category { CategoryId = 8, Name = "Science" }
            );


            // Seed data for Tags
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagId = 1, Name = "CSharp" },
                new Tag { TagId = 2, Name = "DotNet" },
                new Tag { TagId = 3, Name = "Programming" },
                new Tag { TagId = 4, Name = "Development" },
                new Tag { TagId = 5, Name = "Tutorial" },
                new Tag { TagId = 6, Name = "Technology" }
            );
        }

        private void ConfigureCommentRelationships(ModelBuilder modelBuilder)
        {
            // Configure comment relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete comments when post is deleted

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete replies when parent comment is deleted
        }

        private void ConfigurePostTagRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                 .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete post tags when post is deleted

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete post tags when tag is deleted
        }

        private void ConfigurePostCategoryRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category) 
                .WithMany(c => c.Posts)  
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigurePostImageRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                
                // TODO:
            });
        }
    }
}
