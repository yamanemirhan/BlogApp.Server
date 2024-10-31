using BlogApp.Application.Commands.CommentCommands;
using BlogApp.Application.DTOs.Requests;

namespace BlogApp.Application.Services
{
    public interface ICommentService
    {
        Task<int> AddCommentAsync(CreateCommentRequestDto createCommandDto, int userId);
        Task<Comment> UpdateCommentAsync(int commentId, string content);

        Task<bool> DeleteCommentByIdAsync(int commentId);
    }
}

