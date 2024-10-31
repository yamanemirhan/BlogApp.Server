using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostImage> CreateAsync(PostImage image)
        {
            await _context.PostImages.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<PostImage>> GetByPostIdAsync(int postId)
        {
            return await _context.PostImages
                .Where(pi => pi.PostId == postId)
                .ToListAsync();
        }
    }
}