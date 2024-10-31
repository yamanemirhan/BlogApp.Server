using BlogApp.Application.DTOs.Requests;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Infrastructure.Services
{
    public class CommentService(ICommentRepository commentRepository) : ICommentService, IResourceOwnershipService
    {
        public async Task<bool> IsOwnerAsync(int commentId, int userId)
        {
            var comment = await GetCommentByIdAsync(commentId); // method to get the comment
            return comment != null && comment.User.UserId == userId;
        }
        public async Task<int> AddCommentAsync(CreateCommentRequestDto createCommentDto, int userId)
        {
            var comment = new Comment
            {
                PostId = createCommentDto.PostId,
                UserId = userId,
                Content = createCommentDto.Content,
                ParentCommentId = createCommentDto.ParentCommentId,
                CreatedAt = DateTime.UtcNow
            };

            await commentRepository.AddAsync(comment);
            await commentRepository.SaveChangesAsync();

            return comment.CommentId;
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null) 
                return null;

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(int commentId, string content)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                throw new ValidationException("Comment not found.");
            }

            comment.Content = content;
            comment.IsUpdated = true;

            return await commentRepository.UpdateCommentAsync(comment);
        }

        public async Task<bool> DeleteCommentByIdAsync(int commentId)
        {
            var isSuccess = await commentRepository.DeleteByIdAsync(commentId);
            if (isSuccess)
            {
                await commentRepository.SaveChangesAsync();
            }
            return isSuccess;
        }
    }
}

