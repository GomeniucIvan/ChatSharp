using Microsoft.AspNetCore.SignalR;

namespace ChatSharp
{
    public class ChatSharpHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
           
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           
        }
    }
}
