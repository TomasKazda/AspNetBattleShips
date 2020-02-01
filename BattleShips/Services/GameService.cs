using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Models;
using BattleShips.Data;

namespace BattleShips.Services
{
    public class GameService
    {
        readonly HttpContext _httpContext;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        //readonly UserManager<IdentityUser> _userManager;

        public GameService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IConfiguration config/*, UserManager<IdentityUser> userManager*/)
        {

            _httpContext = httpContextAccessor.HttpContext;
            //_userManager = userManager;
            GameId = LoadOrCreateFromSession();
            this._db = db;
            this._config = config;
        }

        private Guid GameId { get; set; }
        private string GetUserId()
        {
            return _httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }
        private Guid LoadOrCreateFromSession()
        {
            //if (typeof(T).IsClass && result == null) result = (T)Activator.CreateInstance(typeof(T));
            return _httpContext.Session.Get<Guid>("GameKey");
        }
        private void SetGameId(Guid data)
        {
            this.GameId = data;
            _httpContext.Session.Set("GameKey", data);
        }

        private void AddGamePiecesToGame()
        {
            if (this.GameId == default) throw new KeyNotFoundException("Hra není vybrána");
            int size = _config.GetValue<int>("BattlefieldSize");
            for (int ycolumn = 0; ycolumn < size; ycolumn++)
            {
                for (int xrow = 0; xrow < size; xrow++)
                {
                    GamePiece gp = new GamePiece() { CoordinateX = xrow, CoordinateY = ycolumn, GameId = this.GameId, OwnerId = GetUserId()};
                    _db.GamePieces.Add(gp);
                }
            }
            _db.SaveChanges();
        }

        public void UnloadGame()
        {
            this.GameId = default;
            SetGameId(this.GameId);
        }

        public bool IsGameLoaded => this.GameId != default;

        public Guid NewGame()
        {
            Guid gameId = Guid.NewGuid();
            string userId = GetUserId();
            if (userId == default) throw new NullReferenceException("User not logged in...");
            Game g = new Game(gameId, userId);
            _db.Games.Add(g);
            _db.SaveChanges();
            SetGameId(gameId);
            AddGamePiecesToGame();
            return gameId;
        }

        public bool JoinToGame(Guid gameId)
        {
            string userId = GetUserId();
            if (userId == default) throw new NullReferenceException("User not logged in...");
            Game g = _db.Games.SingleOrDefault(g => g.Id == gameId && g.Player1Id != userId);

            if (g.Player2Id != null) return false; //exception needed
                
            g.Player2Id = userId;
            AddGamePiecesToGame();
            return true;
        }

        public Game GetGame(Guid id)
        {
            return _db.Games.Where(g => g.Id == id).Include(g => g.GamePieces).Include(g => g.Player1).Include(g => g.Player2).AsNoTracking().SingleOrDefault();
        }
        public Game GetGame()
        {
            return GetGame(GameId);
        }
        public IList<Game> GetMyGames()
        {
            var id = GetUserId();
            return _db.Games.Where(g => g.Player1Id == id && g.GameState != GameState.WinnerPlayer1 && g.GameState != GameState.WinnerPlayer2).Include(g => g.Player1).Include(g => g.Player2).AsNoTracking().ToList();
        }
        public IList<Game> GetOtherGames()
        {
            var id = GetUserId();

            return _db.Games.Where(g => g.Player2Id == null && g.Player1Id != id).Include(g => g.Player1).AsNoTracking().ToList();
        }

        public bool DeleteGame(Guid id)
        {
            var game = _db.Games.SingleOrDefault(g => g.Id == id);

            if (game != null && game.Player1Id == GetUserId())
            {
                _db.Games.Remove(game);
                _db.SaveChanges();
                UnloadGame();
                return true;
            }
            return false;
        }
        //public string GetUserId()
        //{

        //    return _userManager.GetUserId(_httpContext.User);
        //}
    }
}
