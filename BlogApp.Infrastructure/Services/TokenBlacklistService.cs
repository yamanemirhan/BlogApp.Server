using BlogApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace BlogApp.Infrastructure.Services
{
    public class TokenBlacklistService(IConfiguration configuration) : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklistedTokens = new();
        private readonly IConfiguration _configuration = configuration;

        public void AddToBlacklist(string token)
        {
            // Blacklist the token with an expiration time (you could set this to match token expiration)
            _blacklistedTokens[token] = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])); // Customize duration if needed
        }

        public bool IsBlacklisted(string token)
        {
            // Remove expired tokens
            foreach (var item in _blacklistedTokens)
            {
                if (item.Value < DateTime.UtcNow)
                    _blacklistedTokens.TryRemove(item.Key, out _);
            }

            return _blacklistedTokens.ContainsKey(token);
        }
    }
}
