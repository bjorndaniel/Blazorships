using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazorships.Server.Hubs
{
    public class ChatHub : Hub
    {
        private IMemoryCache _cache;
        public ChatHub(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue("GameCacheKey", out List<Game> games))
            {
                _cache.Set("GameCacheKey", new List<Game>());
            }
        }
        public async Task SendMessage(string gameId, string playerId, string message)
        {
            //TODO: only send to a game
            var games = _cache.Get<List<Game>>("GameCacheKey");
            var game = games.First(_ => _.Id == gameId);
            if (game != null)
            {
                if (!string.IsNullOrEmpty(game.Player1.ConnectionId))
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("ReceiveMessage", game.GetPlayer(playerId).Name, message);
                }
                if (!string.IsNullOrEmpty(game.Player2.ConnectionId))
                {
                    await Clients.Client(game.Player2.ConnectionId).SendAsync("ReceiveMessage", game.GetPlayer(playerId).Name, message);
                }

            }
            //await Clients.All.SendAsync("ReceiveMessage", playerId, message);
        }

        public async Task InitGame(string user)//TODO: Get unique connection
        {
            Console.WriteLine(user);
            var game = GetSinglePlayerGame();
            if (game == null)
            {
                game = new Game(false);
                game.AddPlayer(user);
                var games = _cache.Get<List<Game>>("GameCacheKey");
                games.Add(game);
                game.Player1.ConnectionId = Context.ConnectionId;
                _cache.Set("GameCacheKey", games);
            }
            else
            {
                game.AddPlayer(user, false);
                game.Player2.ConnectionId = Context.ConnectionId;
                game.Start();
            }
            await SendUpdateGame(game);
            //await Clients.All
        }


        public async Task Fire(string gameId, string playerId, int row, int column)
        {
            var games = _cache.Get<List<Game>>("GameCacheKey");
            var game = games.First(_ => _.Id == gameId);
            game.Fire(playerId, row, column);
            _cache.Set("GameCacheKey", games);
            await SendUpdateGame(game);
            //await Clients.All.SendAsync("GameUpdated", game.Id);
        }

        public Game GetGame(string gameId) => _cache.Get<List<Game>>("GameCacheKey").FirstOrDefault(_ => _.Id == gameId);

        private Game GetSinglePlayerGame() => _cache.Get<List<Game>>("GameCacheKey").FirstOrDefault(_ => _.SpotAvailable);

        private async Task SendUpdateGame(Game game)
        {
            if (!string.IsNullOrEmpty(game.Player1.ConnectionId))
            {
                await Clients.Client(game.Player1.ConnectionId).SendAsync("GameUpdated", game.Id);
            }
            if (!string.IsNullOrEmpty(game.Player2.ConnectionId))
            {
                await Clients.Client(game.Player2.ConnectionId).SendAsync("GameUpdated", game.Id);
            }
        }
    }

}

