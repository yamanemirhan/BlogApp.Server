using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.DTOs.Requests;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using BlogApp.Domain.DTOs.Requests;
using AutoMapper;

public class PostService(IPostRepository postRepository, 
    ITagRepository tagRepository, IPostTagRepository postTagRepository, 
    ICategoryRepository categoryRepository, IUserRepository userRepository,
    IMapper mapper) : IPostService, IResourceOwnershipService
{
    public async Task<bool> IsOwnerAsync(int postId, int userId)
    {
        var post = await GetPostByIdAsync(postId); 
        return post != null && post.Author.AuthorId == userId;
    }

    public async Task<Post> CreatePostAsync(CreatePostRequestDto createPostDto, int userId)
    {
        // Validate that the number of tags is between 1 and 3
        if (createPostDto.TagIds.Count < 1 || createPostDto.TagIds.Count > 3)
        {
            throw new ValidationException("A post must have between 1 and 3 tags.");
        }

        // Create the post object
        var post = new Post
        {
            Title = createPostDto.Title,
            Description = createPostDto.Description,
            Content = createPostDto.Content,
            AuthorId = userId,
            CategoryId = createPostDto.CategoryId,
            CoverImageUrl = string.IsNullOrWhiteSpace(createPostDto.CoverImageUrl)
                ? "https://res.cloudinary.com/dcj1bb8vk/image/upload/v1728852590/blogapp/default-post_celfxg.png"
                : createPostDto.CoverImageUrl,
            PostTags = createPostDto.TagIds.Select(tagId => new PostTag { TagId = tagId }).ToList(),
            PostImages = createPostDto.ContentImages?.Select(imageUrl => new PostImage
            {
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? new List<PostImage>()
        };

        // Create the post with all related entities
        var createdPost = await postRepository.CreateAsync(post);

        return createdPost;
    }

    private List<CommentDto> MapComments(List<Comment> comments)
    {
        return comments.Select(comment => new CommentDto
        {
            CommentId = comment.CommentId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            IsUpdated = comment.IsUpdated,
            Author = new AuthorDto
            {
                AuthorId = comment.User.UserId,
                Username = comment.User.Username,
                Email = comment.User.Email,
                IsAdmin = comment.User.IsAdmin,
                ProfileImageUrl = comment.User.ProfileImageUrl,
                CreatedAt = comment.User.CreatedAt
            },
            Children = comment.Replies?
                .Select(reply => new CommentDto
                {
                    CommentId = reply.CommentId,
                    Content = reply.Content,
                    CreatedAt = reply.CreatedAt,
                    IsUpdated = reply.IsUpdated,
                    Author = new AuthorDto
                    {
                        AuthorId = reply.User.UserId,
                        Username = reply.User.Username,
                        Email = reply.User.Email,
                        IsAdmin = reply.User.IsAdmin,
                        ProfileImageUrl = reply.User.ProfileImageUrl,
                        CreatedAt = reply.User.CreatedAt
                    },
                    Children = MapComments(reply.Replies?.ToList() ?? new List<Comment>())
                }).ToList() ?? new List<CommentDto>()
        }).ToList();
    }
    public async Task<PostResponseDto> GetPostByIdAsync(int postId)
    {
        var post = await postRepository.GetPostByIdAsync(postId);
        if (post == null)
            return null;

        return new PostResponseDto
        {
            PostId = post.PostId,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            PublishedDate = post.PublishedDate,
            LastUpdated = post.LastUpdated,
            CoverImageUrl = post.CoverImageUrl,
            PostImages = post.PostImages,
            Author = MapToAuthorDto(post.Author),
            Category = new CategoryDto
            {
                CategoryId = post.Category.CategoryId,
                Name = post.Category.Name
            },
            Tags = post.PostTags.Select(pt => new TagDto
            {
                TagId = pt.Tag.TagId,
                Name = pt.Tag.Name
            }).ToList(),
            Comments = MapCommentsHierarchy(post.Comments)
        };
    }

    private static AuthorDto MapToAuthorDto(User user)
    {
        return new AuthorDto
        {
            AuthorId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            ProfileImageUrl = user.ProfileImageUrl,
            CreatedAt = user.CreatedAt
        };
    }

    private static List<CommentDto> MapCommentsHierarchy(ICollection<Comment> comments)
    {
        if (comments == null)
            return new List<CommentDto>();

        return comments
            .Where(c => c.ParentCommentId == null)
            .Select(c => MapSingleComment(c))
            .ToList();
    }

    private static CommentDto MapSingleComment(Comment comment)
    {
        return new CommentDto
        {
            CommentId = comment.CommentId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            IsUpdated = comment.IsUpdated,
            Author = MapToAuthorDto(comment.User),
            Children = comment.Replies?
                .Select(MapSingleComment)
                .ToList() ?? new List<CommentDto>()
        };
    }

    private static CategoryDto MapToCategoryDto(Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name
        };
    }

    private static List<TagDto> MapToTagDtos(ICollection<PostTag> postTags)
    {
        return postTags.Select(pt => new TagDto
        {
            TagId = pt.Tag.TagId,
            Name = pt.Tag.Name
        }).ToList();
    }
    public async Task<IEnumerable<PostResponseDto>> GetPostsByUserIdAsync(int userId)
    {
        var posts = await postRepository.GetPostsByUserIdAsync(userId);
        if (!posts.Any())
            return Enumerable.Empty<PostResponseDto>();

        return posts.Select(post => new PostResponseDto
        {
            PostId = post.PostId,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            PublishedDate = post.PublishedDate,
            LastUpdated = post.LastUpdated,
            CoverImageUrl = post.CoverImageUrl,
            PostImages = post.PostImages,
            Author = MapToAuthorDto(post.Author),
            Category = MapToCategoryDto(post.Category),
            Tags = MapToTagDtos(post.PostTags),
            Comments = MapCommentsHierarchy(post.Comments)
        }).ToList();
    }

    public async Task<IEnumerable<PostResponseDto>> GetPostsByUsernameAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);

        if (user == null)
            return Enumerable.Empty<PostResponseDto>();

        var posts = await postRepository.GetPostsByUsernameAsync(user.Username);
        if (!posts.Any())
            return Enumerable.Empty<PostResponseDto>();

        return posts.Select(post => new PostResponseDto
        {
            PostId = post.PostId,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            PublishedDate = post.PublishedDate,
            LastUpdated = post.LastUpdated,
            CoverImageUrl = post.CoverImageUrl,
            PostImages = post.PostImages,
            Author = MapToAuthorDto(post.Author),
            Category = MapToCategoryDto(post.Category),
            Tags = MapToTagDtos(post.PostTags),
            Comments = MapCommentsHierarchy(post.Comments)
        }).ToList();
    }

    public async Task<PostResponseDto> UpdatePostAsync(UpdatePostCommand command)
    {
        var post = await postRepository.GetPostByIdAsync(command.PostId);

        if (post == null)
        {
            throw new ValidationException("Post not found.");
        }

        if (command.TagIds.Count < 1 || command.TagIds.Count > 3)
        {
            throw new ValidationException("A post must have between 1 and 3 tags.");
        }

        var _tags = await tagRepository.GetTagsByIdsAsync(command.TagIds);

        // Check if any TagId from command.TagIds is missing in _tags
        var missingTagIds = command.TagIds.Except(_tags.Select(t => t.TagId)).ToList();
        if (missingTagIds.Count > 0)
        {
            throw new ValidationException("An invalid tag found with IDs: " + string.Join(", ", missingTagIds));
        }

        // Fetch the Category from the repository
        var category = await categoryRepository.GetByIdAsync(command.CategoryId);
        if (category == null)
        {
            throw new ValidationException("Category not found.");
        }

        post.Title = command.Title;
        post.Description = command.Description;
        post.Content = command.Content;
        post.CategoryId = command.CategoryId;
        post.Category = category; 
        post.CoverImageUrl = string.IsNullOrWhiteSpace(command.CoverImageUrl)
            ? post.CoverImageUrl
            : command.CoverImageUrl;

        await postTagRepository.RemovePostTags(post.PostTags.ToList());

        post.PostTags = command.TagIds
            .Select(tagId => new PostTag
            {
                PostId = post.PostId,
                TagId = tagId,
                Post = post,
                Tag = _tags.FirstOrDefault(t => t.TagId == tagId)
            })
            .ToList();

        post.LastUpdated = DateTime.Now;

        var updatedPost = await postRepository.UpdateAsync(post);

        var tags = updatedPost.PostTags.Select(pt => pt.Tag).ToList();

        return new PostResponseDto
        {
            PostId = post.PostId,
            Title = updatedPost.Title,
            Description = updatedPost.Description,
            Content = updatedPost.Content,
            PublishedDate = updatedPost.PublishedDate,
            LastUpdated = updatedPost.LastUpdated,
            CoverImageUrl = updatedPost.CoverImageUrl,
            Author = new AuthorDto
            {
                AuthorId = updatedPost.Author.UserId,
                Username = updatedPost.Author.Username,
                Email = updatedPost.Author.Email,
                ProfileImageUrl = updatedPost.Author.ProfileImageUrl,
                CreatedAt = updatedPost.Author.CreatedAt
            },
            Category = new CategoryDto
            {
                CategoryId = updatedPost.Category.CategoryId,
                Name = updatedPost.Category.Name
            },
            Tags = tags.Select(tag => new TagDto
            {
                TagId = tag.TagId,
                Name = tag.Name
            }).ToList()
        };
    }


    public async Task<bool> DeletePostByIdAsync(int postId)
    {
        var isSuccess = await postRepository.DeleteByIdAsync(postId);
        if(isSuccess) {
            await postRepository.SaveChangesAsync();
        }
        return isSuccess;
    }

    public async Task<PaginatedResponseDto<PostResponseDto>> GetAllPostsAsync(GetAllPostsQueryDto queryDto)
    {
        // todo: mapper
        //var query = mapper.Map<GetAllPostsQueryDto>(queryDto);

        var query = queryDto;

        // paginated result
        var result = await postRepository.GetAllPostsAsync(query);

        // map posts to DTOs
        var postDtos = result.Items.Select(post => new PostResponseDto
        {
            PostId = post.PostId,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            PublishedDate = post.PublishedDate,
            LastUpdated = post.LastUpdated,
            CoverImageUrl = post.CoverImageUrl,
            PostImages = post.PostImages,
            Author = new AuthorDto
            {
                AuthorId = post.Author.UserId,
                Username = post.Author.Username,
                Email = post.Author.Email,
                ProfileImageUrl = post.Author.ProfileImageUrl,
                CreatedAt = post.Author.CreatedAt
            },
            Category = new CategoryDto
            {
                CategoryId = post.Category.CategoryId,
                Name = post.Category.Name
            },
            Tags = post.PostTags.Select(pt => new TagDto
            {
                TagId = pt.Tag.TagId,
                Name = pt.Tag.Name
            }).ToList(),
            Comments = MapCommentsHierarchy(post.Comments)
        }).ToList();

        // return paginated response
        return new PaginatedResponseDto<PostResponseDto>
        {
            Items = postDtos,
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };
    }
}
