using Microsoft.AspNetCore.SignalR;

namespace PLC.WebApp.Hubs
{
    public class ConnectionHub : Hub
    {


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PlcHub");
            await Clients.Caller.SendAsync("HubConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "PlcHub");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
