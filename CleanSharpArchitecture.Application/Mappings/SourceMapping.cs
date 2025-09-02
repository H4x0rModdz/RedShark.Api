using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Badges;
using CleanSharpArchitecture.Application.DTOs.Badges.UserBadges;
using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;
using CleanSharpArchitecture.Application.DTOs.Chats;
using CleanSharpArchitecture.Application.DTOs.Chats.UserChats;
using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Application.DTOs.Followers;
using CleanSharpArchitecture.Application.DTOs.Likes;
using CleanSharpArchitecture.Application.DTOs.Likes.Request;
using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;
using CleanSharpArchitecture.Application.DTOs.Posts.PostImages;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;

namespace CleanSharpArchitecture.Application.Mappings
{
    public class SourceMapping : Profile
    {
        public SourceMapping()
        {
            #region Chat
            CreateMap<Chat, ChatDto>()
                .ForMember(dest => dest.Participants,
                    opt => opt.MapFrom(src => src.Participants))
                .ForMember(dest => dest.Messages,
                    opt => opt.MapFrom(src => src.Messages));

            CreateMap<ChatMessage, ChatMessageDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.Name));

            CreateMap<UserChat, UserChatDto>()
                .ForMember(dest => dest.ChatName,
                           opt => opt.MapFrom(src => src.Chat.Name))
                .ForMember(dest => dest.UserName,
                           opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Name,
                           opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ProfileImageUrl,
                           opt => opt.MapFrom(src => src.User.ProfileImageUrl));
            #endregion Chat

            #region Post
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(i => new PostImageDto { Id = i.Id, ImageUrl = i.ImageUrl })))
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserImage,
                    opt => opt.MapFrom(src => src.User.ProfileImageUrl));

            CreateMap<PostImage, PostImageDto>();

            CreateMap<Post, GetPostDto>()
                .ForMember(dest => dest.ImageUrls,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)));
            #endregion Post

            #region Badge

            CreateMap<Badge, BadgeDto>();

            CreateMap<UserBadge, UserBadgeDto>()
                .ForMember(dest => dest.BadgeName,
                    opt => opt.MapFrom(src => src.Badge.Name))
                .ForMember(dest => dest.BadgeDescription,
                    opt => opt.MapFrom(src => src.Badge.Description))
                .ForMember(dest => dest.BadgeIconUrl,
                    opt => opt.MapFrom(src => src.Badge.IconUrl));

            #endregion Badge

            #region User

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Posts,
                    opt => opt.MapFrom(src => src.Posts))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Biography,
                    opt => opt.MapFrom(src => src.Biography))
                .ForMember(dest => dest.ProfileImageUrl,
                    opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.CoverImageUrl,
                    opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website));

            CreateMap<UserPhoto, UserPhotoDto>();

            #endregion User


            #region Comment

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserImage,
                    opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => 0));
            
            #endregion Comment

            #region Like
            
            CreateMap<Like, LikeDto>();
            CreateMap<CreateLikeDto, Like>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Post, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore())
                .ForMember(dest => dest.CommentId, opt => opt.Ignore());

            #endregion Like


            #region Follower

            CreateMap<Follower, FollowerDto>()
                .ForMember(d => d.RelationshipId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProfileUserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.FollowerId, o => o.MapFrom(s => s.FollowerId))

                .ForMember(d => d.FollowerDisplayName, o => o.MapFrom(s => s.FollowerUser.Name))
                .ForMember(d => d.FollowerUsername, o => o.MapFrom(s => s.FollowerUser.UserName))
                .ForMember(d => d.FollowerProfileImage, o => o.MapFrom(s => s.FollowerUser.ProfileImageUrl))

                .ForMember(d => d.DoIFollowThisPerson, o => o.Ignore())
                .ForMember(d => d.DoesThisPersonFollowMe, o => o.Ignore())

                .ForMember(d => d.FollowerDisplayName, o => o.PreCondition(s => s.FollowerUser != null))
                .ForMember(d => d.FollowerUsername, o => o.PreCondition(s => s.FollowerUser != null))
                .ForMember(d => d.FollowerProfileImage, o => o.PreCondition(s => s.FollowerUser != null));


            #endregion Follower

            #region Following

            CreateMap<Follower, FollowingDto>()
                .ForMember(d => d.RelationshipId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProfileUserId, o => o.MapFrom(s => s.FollowerId))
                .ForMember(d => d.FollowingId, o => o.MapFrom(s => s.UserId))

                .ForMember(d => d.FollowingDisplayName, o => o.MapFrom(s => s.User.Name))
                .ForMember(d => d.FollowingUsername, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.FollowingProfileImage, o => o.MapFrom(s => s.User.ProfileImageUrl))

                .ForMember(d => d.DoIFollowThisPerson, o => o.Ignore())
                .ForMember(d => d.DoesThisPersonFollowMe, o => o.Ignore())

                .ForMember(d => d.FollowingDisplayName, o => o.PreCondition(s => s.User != null))
                .ForMember(d => d.FollowingUsername, o => o.PreCondition(s => s.User != null))
                .ForMember(d => d.FollowingProfileImage, o => o.PreCondition(s => s.User != null));

            #endregion

            #region Notification

            CreateMap<Notification, NotificationDto>();
            
            #endregion Notification
        }
    }
}
