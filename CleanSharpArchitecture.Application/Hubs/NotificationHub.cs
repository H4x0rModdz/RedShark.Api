using Microsoft.AspNetCore.SignalR;

namespace CleanSharpArchitecture.Application.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
    }
}
