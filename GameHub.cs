using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using ColourSync.Domain;
using System.Linq;
using ColourSync.Domain.Enums;
using System.Threading;

public class GameHub : Hub
{
    private object theLock = new object();
    private static GameState game = new GameState();
    public async Task JoinTable(string tableName, string username)
    {
        await Groups.Add(Context.ConnectionId, tableName);
        game.JoinTable(tableName, username, Guid.Parse(Context.ConnectionId));
        Clients.Group(tableName).updateUsers(game.Tables.Single(t => t.Name == tableName).Players);
    }

    public void StartGame(string table)
    {
        
        game.Tables.Single(t => t.Name == table).StartGame();
        int count = 0;
        while(count < 3)
        {
            count++;
            Clients.Group(table).updateTimer(count);
            Thread.Sleep(1000);
        }

        Clients.Group(table).startGame();
        Clients.Group(table).updateServerMessage(game.Tables.Single(t => t.Name == table).PlayersNeedingToMove);
    }

    public void NextRound(string table)
    {
        
        game.Tables.Single(t => t.Name == table).NextRound();
        int count = 0;
        while(count < 3)
        {
            count++;
            Clients.Group(table).updateTimer(count);
            Thread.Sleep(1000);
        }

        Clients.Group(table).startGame();
        Clients.Group(table).updateServerMessage(game.Tables.Single(t => t.Name == table).PlayersNeedingToMove);
    }

    public async Task PickColour(string table, string colour)
    {
        var move = (Moves)Enum.Parse(typeof(Moves), colour.First().ToString().ToUpper() + String.Join("", colour.Skip(1)));

        lock(theLock)
        {
            game.MakeMoveForPlayer(table, Guid.Parse(Context.ConnectionId), move, DateTime.Now);
        }

        while(game.Tables.Single(t => t.Name == table).PlayersNeedingToMove > 0)
        {
            Clients.Group(table).updateServerMessage(game.Tables.Single(t => t.Name == table).PlayersNeedingToMove);
            Thread.Sleep(100);
        }

        Clients.Group(table).gameComplete(game.Tables.Single(t => t.Name == table).JokerLoosers, 
                        game.Tables.Single(t => t.Name == table).Loosers, 
                        game.Tables.Single(t => t.Name == table).SlowestPlayer);
    }

    public async Task LeaveTable(string tableName)
    {
        await Groups.Remove(Context.ConnectionId, tableName);
        game.LeaveTable(tableName, Guid.Parse(Context.ConnectionId));
        Clients.Group(tableName).updateUsers(game.Tables.Single(t => t.Name == tableName).Players);
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        string name = Context.ConnectionId;
        var table = game.Tables.Single(t => t.Players.Any(p => p.Id == Guid.Parse(Context.ConnectionId))).Name;
        game.LeaveTable(table, Guid.Parse(Context.ConnectionId));
        Clients.Group(table).updateUsers(game.Tables.Single(t => t.Name == table).Players);

        return base.OnDisconnected(stopCalled);
    }
}
