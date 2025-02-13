﻿using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;

namespace CleanSharpArchitecture.Application.DTOs.Users.Requests
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public string ProfileImageUrl { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }

        public ICollection<PostDto>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<UserBadge>? Badges { get; set; }
        public ICollection<Follower>? Followers { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<UserChat>? Chats { get; set; }
    }
}
