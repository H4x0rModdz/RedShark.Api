using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Badges;
using CleanSharpArchitecture.Application.DTOs.Badges.UserBadges;
using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;
using CleanSharpArchitecture.Application.DTOs.Chats;
using CleanSharpArchitecture.Application.DTOs.Chats.UserChats;
using CleanSharpArchitecture.Application.DTOs.Comments;
using CleanSharpArchitecture.Application.DTOs.Followers;
using CleanSharpArchitecture.Application.DTOs.Likes;
using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Users.Requests;
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
                           opt => opt.MapFrom(src => src.User.Name));
            #endregion Chat

            #region Post
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(i => new PostImageDto { Id = i.Id, ImageUrl = i.ImageUrl })))
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count));

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

            #endregion User

            #region Comment

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.Name));
            
            #endregion Comment

            #region Like
            
            CreateMap<Like, LikeDto>();
            
            #endregion Like


            #region Follower
            
            CreateMap<Follower, FollowerDto>()
                .ForMember(dest => dest.UserName,
                           opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.FollowerName,
                           opt => opt.MapFrom(src => src.FollowerUser.Name));
            
            #endregion Follower

            #region Notification

            CreateMap<Notification, NotificationDto>();
            
            #endregion Notification
        }
    }
}
