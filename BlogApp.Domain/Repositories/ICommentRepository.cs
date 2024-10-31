namespace BlogApp.Domain.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<Comment> UpdateCommentAsync(Comment comment);
    }
}
