using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Followers;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Interfaces;

namespace CleanSharpArchitecture.Application.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FollowerService(
            IFollowerRepository followerRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _followerRepository = followerRepository ?? throw new ArgumentNullException(nameof(followerRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<FollowerDto>> GetFollowersAsync(long userId, int pageNumber = 1, int pageSize = 10)
        {
            // User existence already validated by ValidateUser attribute
            var followers = await _followerRepository.GetFollowersAsync(userId, pageNumber, pageSize);
            var followerDtos = new List<FollowerDto>();

            foreach (var follower in followers)
            {
                var doIFollowThisPerson = await _followerRepository.IsFollowingAsync(userId, follower.FollowerId);
                var doesThisPersonFollowMe = true;

                var followerDto = new FollowerDto
                {
                    RelationshipId = follower.Id,
                    ProfileUserId = follower.UserId,
                    FollowerId = follower.FollowerId,
                    FollowerDisplayName = follower.FollowerUser.Name,
                    FollowerUsername = follower.FollowerUser.UserName,
                    FollowerProfileImage = follower.FollowerUser.ProfileImageUrl,
                    DoIFollowThisPerson = doIFollowThisPerson,
                    DoesThisPersonFollowMe = doesThisPersonFollowMe
                };

                followerDtos.Add(followerDto);
            }

            return followerDtos;
        }

        public async Task<IEnumerable<FollowingDto>> GetFollowingAsync(long userId, int pageNumber = 1, int pageSize = 10)
        {
            var following = await _followerRepository.GetFollowingAsync(userId, pageNumber, pageSize);
            var followingDtos = new List<FollowingDto>();

            foreach (var follow in following)
            {
                var doIFollowThisPerson = true;
                var doesThisPersonFollowMe = await _followerRepository.IsFollowingAsync(follow.UserId, userId);

                var followingDto = new FollowingDto
                {
                    RelationshipId = follow.Id,
                    ProfileUserId = follow.FollowerId,
                    FollowingId = follow.UserId,
                    FollowingDisplayName = follow.User.Name,
                    FollowingUsername = follow.User.UserName,
                    FollowingProfileImage = follow.User.ProfileImageUrl,
                    DoIFollowThisPerson = doIFollowThisPerson,
                    DoesThisPersonFollowMe = doesThisPersonFollowMe
                };

                followingDtos.Add(followingDto);
            }

            return followingDtos;
        }

        public async Task<int> GetFollowersCountAsync(long userId)
        {
            return await _followerRepository.GetFollowersCountAsync(userId);
        }

        public async Task<int> GetFollowingCountAsync(long userId)
        {
            return await _followerRepository.GetFollowingCountAsync(userId);
        }

        public async Task<bool> FollowUserAsync(long followerId, long userId)
        {
            var isAlreadyFollowing = await _followerRepository.IsFollowingAsync(followerId, userId);
            if (isAlreadyFollowing)
                return false;
                
            // Prevent self-follow
            if (followerId == userId)
                return false;

            var userExists = await _userRepository.SelectById(userId);
            if (userExists == null)
                return false;

            await _followerRepository.FollowUserAsync(followerId, userId);
            return true;
        }

        public async Task<bool> UnfollowUserAsync(long followerId, long userId)
        {
            var isFollowing = await _followerRepository.IsFollowingAsync(followerId, userId);
            if (!isFollowing)
                return false;
                
            return await _followerRepository.UnfollowUserAsync(followerId, userId);
        }
    }
}