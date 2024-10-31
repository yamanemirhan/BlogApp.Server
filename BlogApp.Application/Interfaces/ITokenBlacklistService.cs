namespace BlogApp.Application.Interfaces
{
    public interface ITokenBlacklistService
    {
        void AddToBlacklist(string token);
        bool IsBlacklisted(string token);
    }
}
