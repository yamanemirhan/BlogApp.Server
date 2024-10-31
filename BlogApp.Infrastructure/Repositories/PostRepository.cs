using BlogApp.Domain.DTOs.Requests;
using BlogApp.Domain.DTOs.Responses;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Repositories
{
    public class PostRepository(AppDbContext dbContext) : GenericRepository<Post>(dbContext), IPostRepository
    {
        public async Task<Post> CreateAsync(Post post)
        {
            await _dbSet.AddAsync(post);
            await SaveChangesAsync();
            return post;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await dbContext.Posts
                .Where(p => p.PostId == postId)
                .Include(p => p.Category)
                .Include(p => p.PostImages)
                .Include(p => p.Comments) // Include all comments
                    .ThenInclude(c => c.User)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                // Organize comments hierarchy after fetching data
                var commentsList = post.Comments.ToList();
                post.Comments = OrganizeCommentHierarchy(commentsList);
            }

            return post;
        }

        private static List<Comment> OrganizeCommentHierarchy(List<Comment> allComments)
        {
            var rootComments = allComments.Where(c => c.ParentCommentId == null).ToList();
            var commentsDictionary = allComments.ToDictionary(c => c.CommentId);

            foreach (var comment in allComments)
            {
                if (comment.ParentCommentId.HasValue && commentsDictionary.ContainsKey(comment.ParentCommentId.Value))
                {
                    var parentComment = commentsDictionary[comment.ParentCommentId.Value];
                    if (parentComment.Replies == null)
                        parentComment.Replies = new List<Comment>();
                    parentComment.Replies.Add(comment);
                }
            }

            return rootComments;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await dbContext.Posts
                .Where(p => p.AuthorId == userId)
                .Include(p => p.Category)
                .Include(p => p.PostImages)
                .Include(p => p.Comments) // Include all comments
                    .ThenInclude(c => c.User)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            // Organize comments hierarchy for each post
            foreach (var post in posts)
            {
                var commentsList = post.Comments.ToList();
                post.Comments = OrganizeCommentHierarchy(commentsList);
            }

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsByUsernameAsync(string username)
        {
            var posts = await dbContext.Posts
                .Where(p => p.Author.Username == username)
                .Include(p => p.Category)
                .Include(p => p.PostImages)
                .Include(p => p.Comments) // Include all comments
                    .ThenInclude(c => c.User)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            // Organize comments hierarchy for each post
            foreach (var post in posts)
            {
                var commentsList = post.Comments.ToList();
                post.Comments = OrganizeCommentHierarchy(commentsList);
            }

            return posts;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            // Maintain the author as unchanged to avoid issues with the update
            dbContext.Entry(post.Author).State = EntityState.Unchanged;

            dbContext.Posts.Update(post);
            await SaveChangesAsync();
            return post;
        }

        public async Task<PaginatedResult<Post>> GetAllPostsAsync(GetAllPostsQueryDto query)
        {
            var baseQuery = dbContext.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostImages)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .AsQueryable();

            baseQuery = ApplyFilters(baseQuery, query);

            var totalCount = await baseQuery.CountAsync();

            baseQuery = ApplySorting(baseQuery, query);

            // pagination
            var items = await baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PaginatedResult<Post>(items, totalCount, query.PageNumber, query.PageSize);
        }

        private IQueryable<Post> ApplyFilters(IQueryable<Post> query, GetAllPostsQueryDto queryParams)
        {
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                query = query.Where(p => p.Title.Contains(queryParams.Search) ||
                                       p.Description.Contains(queryParams.Search) ||
                                       p.Content.Contains(queryParams.Search));
            }

            if (queryParams.FromDate.HasValue)
            {
                query = query.Where(p => p.PublishedDate >= queryParams.FromDate.Value);
            }

            if (queryParams.ToDate.HasValue)
            {
                query = query.Where(p => p.PublishedDate <= queryParams.ToDate.Value);
            }

            if (queryParams.CategoryNames?.Any() == true)
            {
                query = query.Where(p => queryParams.CategoryNames.Contains(p.Category.Name));
            }

            if (queryParams.TagNames?.Any() == true)
            {
                query = query.Where(p => p.PostTags.Any(pt => queryParams.TagNames.Contains(pt.Tag.Name)));
            }

            return query;
        }

        private IQueryable<Post> ApplySorting(IQueryable<Post> query, GetAllPostsQueryDto queryParams)
        {
            var sortProperty = typeof(Post).GetProperty(queryParams.SortBy);
            if (sortProperty == null)
            {
                return query.OrderByDescending(p => p.PublishedDate);
            }

            var parameter = Expression.Parameter(typeof(Post), "p");
            var property = Expression.Property(parameter, sortProperty);
            var lambda = Expression.Lambda<Func<Post, object>>(Expression.Convert(property, typeof(object)), parameter);

            return queryParams.IsDescending
                ? query.OrderByDescending(lambda)
                : query.OrderBy(lambda);
        }


        //private Expression<Func<Post, object>> SortByField(string sortBy)
        //{
        //    return sortBy switch
        //    {
        //        //"Title" => p => p.Title,
        //        "PublishedDate" => p => p.PublishedDate,
        //        //"LastUpdated" => p => p.LastUpdated,
        //        _ => p => p.PublishedDate
        //    };
        //}
    }
}
