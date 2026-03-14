using Microsoft.AspNetCore.SignalR;

namespace BombermanServer
{
    public class GameHub : Hub
    {
        
        private static readonly Dictionary<string, string> _players = new();

        public async Task JoinGame(string playerName)
        {
            string id = Context.ConnectionId;

            
            await Clients.Caller.SendAsync("OnAssignedId", id);

            
            foreach (var kvp in _players)
            {
                await Clients.Caller.SendAsync("OnPlayerJoined", kvp.Key, kvp.Value);
            }

            
            _players[id] = playerName;

            
            await Clients.Others.SendAsync("OnPlayerJoined", id, playerName);
        }

        public async Task StartGame(string gridData)
        {
            await Clients.Others.SendAsync("OnGameStart", gridData);
        }

        public async Task PlayerMove(int x, int y)
        {
            string id = Context.ConnectionId;
            await Clients.Others.SendAsync("OnPlayerMoved", id, x, y);
        }

        public async Task PlaceBomb(int x, int y)
        {
            string id = Context.ConnectionId;
            await Clients.Others.SendAsync("OnBombPlaced", id, x, y);
        }

        public async Task BombExploded(string bombId, int x, int y, string destroyedCells)
        {
            await Clients.Others.SendAsync("OnBombExploded", bombId, x, y, destroyedCells);
        }

        public async Task PlayerDied(string playerId)
        {
            await Clients.All.SendAsync("OnPlayerDied", playerId);
        }

        public async Task GameOver(string winnerId)
        {
            await Clients.All.SendAsync("OnGameOver", winnerId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           
            _players.Remove(Context.ConnectionId);
            await Clients.Others.SendAsync("OnPlayerLeft", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}