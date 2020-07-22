using Blazorships.Shared.GameObjects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazorships.Server.Hubs
{
    public class GameHub : Hub
    {
        private IMemoryCache _cache;
        public GameHub(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue("GameCacheKey", out List<Game> games))
            {
                _cache.Set("GameCacheKey", new List<Game>());
            }
        }
       
        public async Task SendMessage(string gameId, string playerId, string message)
        {
            var games = _cache.Get<List<Game>>("GameCacheKey");
            var game = games.FirstOrDefault(_ => _.Id == gameId);
            if (game != null)
            {
                if (!string.IsNullOrEmpty(game.Player1.ConnectionId))
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("GameMessage", game.GetPlayer(playerId).Name, message);
                }
                if (!string.IsNullOrEmpty(game.Player2.ConnectionId))
                {
                    await Clients.Client(game.Player2.ConnectionId).SendAsync("GameMessage", game.GetPlayer(playerId).Name, message);
                }
            }
        }

        public async Task InitGame(string user) //TODO: Get unique connection
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

        }

        public async Task Fire(string gameId, string playerId, int row, int column)
        {
            var games = _cache.Get<List<Game>>("GameCacheKey");
            var game = games.First(_ => _.Id == gameId);
            game.Fire(playerId, row, column);
            _cache.Set("GameCacheKey", games);
            await SendUpdateGame(game);
        }

        public async Task PlayToEnd(string gameId)
        {
            var games = _cache.Get<List<Game>>("GameCacheKey");
            var game = games.First(_ => _.Id == gameId);
            game.PlayToEnd();
            _cache.Set("GameCacheKey", games);
            await SendUpdateGame(game);
        }

        public Game GetGame(string gameId) => _cache.Get<List<Game>>("GameCacheKey").FirstOrDefault(_ => _.Id == gameId);

        private Game GetSinglePlayerGame() => _cache.Get<List<Game>>("GameCacheKey").FirstOrDefault(_ => _.SpotAvailable);

        private async Task SendUpdateGame(Game game)
        {
            if (!string.IsNullOrEmpty(game.Player1.ConnectionId))
            {
                await Clients.Client(game.Player1.ConnectionId).SendAsync("GameUpdated", game.Id);
            }
            if (!string.IsNullOrEmpty(game.Player2?.ConnectionId))
            {
                await Clients.Client(game.Player2.ConnectionId).SendAsync("GameUpdated", game.Id);
            }
        }

    }
}
