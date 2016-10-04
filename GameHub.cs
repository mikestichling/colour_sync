using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using ColourSync.Domain;
using System.Linq;

public class GameHub : Hub
{
    private static GameState game = new GameState();
    public async Task JoinTable(string tableName, string username)
    {
        await Groups.Add(Context.ConnectionId, tableName);
        game.JoinTable(tableName, username, Guid.Parse(Context.ConnectionId));
        Clients.Group(tableName).updateUsers(game.Tables.Single(t => t.Name == tableName).Players);
    }

    public async Task LeaveTable(string tableName)
    {
        await Groups.Remove(Context.ConnectionId, tableName);
        game.LeaveTable(tableName, Guid.Parse(Context.ConnectionId));
        Clients.Group(tableName).updateUsers(game.Tables.Single(t => t.Name == tableName).Players);
    }
}
