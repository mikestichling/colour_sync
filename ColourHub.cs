using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class ColourHub : Hub
{
    private bool running = false;
    private Random random = new Random();
    
    private string[] colours = new string[3] { "#00FF00", "#0000FF", "#FF0000" };

    public Task JoinGroup(string groupName)
    {
        return Groups.Add(Context.ConnectionId, groupName);
    }

    public Task LeaveGroup(string groupName)
    {
        return Groups.Remove(Context.ConnectionId, groupName);
    }

    public void Start(string groupName)
    {
        running = true;
        
        while(running)
        {
            var index = random.Next(0, 3);
            var colour = colours[index];
            Clients.Group(groupName).displayColour(colour);
            System.Threading.Thread.Sleep(100);
        }
    }

    public void Stop()
    {
        running = false;
    }
}
