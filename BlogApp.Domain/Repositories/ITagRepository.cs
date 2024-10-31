namespace BlogApp.Domain.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds);
    }
}
