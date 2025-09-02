using CleanSharpArchitecture.Domain.Entities;
using System.Linq.Expressions;

namespace CleanSharpArchitecture.Domain.Dictionaries
{
    public static class IncludeMap
    {
        public static readonly Dictionary<string, Expression<Func<User, object>>> UserIncludeDictionary =
            new Dictionary<string, Expression<Func<User, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                { "posts", u => u.Posts },
                { "comments", u => u.Comments },
                { "likes", u => u.Likes },
                { "badges", u => u.Badges },
                { "followers", u => u.Followers },
                { "following", u => u.Following },
                { "notifications", u => u.Notifications },
                { "chats", u => u.Chats }
            };
    }
}
