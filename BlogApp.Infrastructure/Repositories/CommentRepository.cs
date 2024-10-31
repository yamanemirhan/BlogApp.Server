using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Repositories
{
    public class CommentRepository(AppDbContext dbContext) : GenericRepository<Comment>(dbContext), ICommentRepository
    {
        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await dbContext.Comments
                .Where(c => c.CommentId == commentId)
                .Include(c => c.User)         
                .Include(c => c.Post)       
                .Include(c => c.Replies)
                .Select(c => new Comment
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Post = c.Post,
                    ParentComment = c.ParentComment,
                    Replies = c.Replies,
                    User = new User
                    {
                        UserId = c.User.UserId,
                        Username = c.User.Username,
                        Email = c.User.Email,
                        ProfileImageUrl = c.User.ProfileImageUrl,
                        IsAdmin = c.User.IsAdmin,
                        CreatedAt = c.User.CreatedAt
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            // Attach the entity to the context in an unchanged state
            var entry = dbContext.Comments.Attach(comment);

            entry.Property(e => e.Content).IsModified = true;
            entry.Property(e => e.IsUpdated).IsModified = true;

            await dbContext.SaveChangesAsync();

            return comment;
        }


    }
}
