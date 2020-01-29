using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Session.Helpers;
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

        //readonly UserManager<IdentityUser> _userManager;

        public GameService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor/*, UserManager<IdentityUser> userManager*/)
        {

            _httpContext = httpContextAccessor.HttpContext;
            //_userManager = userManager;
            GameId = LoadOrCreate();
            this._db = db;
        }

        private Guid GameId { get; set; }
        private string GetUserId()
        {
            return _httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }
        private Guid LoadOrCreate()
        {
            //if (typeof(T).IsClass && result == null) result = (T)Activator.CreateInstance(typeof(T));
            return _httpContext.Session.Get<Guid>("GameKey");
        }
        private void Save(Guid data)
        {
            _httpContext.Session.Set("GameKey", data);
        }

        public bool IsGameLoaded => this.GameId != default;

        public Guid NewGame()
        {
            Guid result = Guid.NewGuid();
            string id = GetUserId();
            if (id == default) throw new NullReferenceException("User not logged in...");
            Game g = new Game(result, id);
            _db.Games.Add(g);
            _db.SaveChanges();
            Save(result);
            return result;
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
