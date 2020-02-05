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

        public Guid GameId { get; private set; }
        public string GetUserId()
        {
            //return _userManager.GetUserId(_httpContext.User);
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
            int size = _config.GetValue<int>("GameSetup:BattlefieldSize");
            for (int ycolumn = 0; ycolumn < size; ycolumn++)
            {
                for (int xrow = 0; xrow < size; xrow++)
                {
                    GamePiece gp = new GamePiece() { CoordinateX = xrow, CoordinateY = ycolumn, GameId = this.GameId, OwnerId = GetUserId() };
                    _db.GamePieces.Add(gp);
                }
            }
            _db.SaveChanges();
        }

        public GameState GetCurrentPlayerGameState(Guid gameId = default)
        {
            if (gameId == default && this.GameId != default) gameId = this.GameId;

            var game = _db.Games.AsNoTracking().SingleOrDefault(g => g.Id == gameId);

            var currentGameState = GameState.GameCreating;
            if (game == default) return currentGameState;
            var currentPlayerId = GetUserId();

            if (game.Player1Id == currentPlayerId) currentGameState = game.GameStateP1;
            if (game.Player2Id == currentPlayerId) currentGameState = game.GameStateP2;

            return currentGameState;
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

            if (g == default || g.Player2Id != null) return false; //exception needed
            SetGameId(gameId);
            g.GameStateP2 = GameState.ShipDeploying;
            g.Player2Id = userId;
            AddGamePiecesToGame();
            return true;
        }

        public bool ContinueToGame(Guid gameId)
        {
            string userId = GetUserId();
            if (userId == default) throw new NullReferenceException("User not logged in...");

            Game g = _db.Games.SingleOrDefault(g => g.Id == gameId && (g.Player1Id == userId || g.Player2Id == userId));
            if (g == default) return false; //exception needed
            SetGameId(gameId);

            return true;
        }

        public Game GetGame(Guid? gameId = null, bool asNoTracking = true)
        {
            if (gameId == null && this.GameId != default) gameId = this.GameId;

            var game = _db.Games.Where(g => g.Id == gameId).Include(g => g.GamePieces).Include(g => g.Player1).Include(g => g.Player2);
            if (asNoTracking) return game.AsNoTracking().SingleOrDefault();

            return game.SingleOrDefault();
        }

        public IList<Game> GetMyGames()
        {
            var id = GetUserId();
            return _db.Games.Where(g => g.Player1Id == id || g.Player2Id == id).Include(g => g.Player1).Include(g => g.Player2).AsNoTracking().ToList();
        }
        public IList<Game> GetReadyToJoinGames()
        {
            var id = GetUserId();

            return _db.Games.Where(g => g.Player2Id == null && g.Player1Id != id).Include(g => g.Player1).AsNoTracking().ToList();
        }

        public bool DeleteGame(Guid id)
        {
            var game = _db.Games.SingleOrDefault(g => g.Id == id);

            var userId = GetUserId();
            if (game != null && (game.Player1Id == userId || game.Player2Id == userId))
            {
                _db.Games.Remove(game);
                _db.SaveChanges();
                if (id == this.GameId) UnloadGame();
                return true;
            }
            return false;
        }

        public bool DeployUndeployBoat(int gamePieceId)
        {
            var gamePiece = _db.GamePieces.Where(gp => gp.Id == gamePieceId && gp.OwnerId == GetUserId()).SingleOrDefault();
            if (gamePiece == default) return false;
            if (gamePiece.GameId != this.GameId) return false;
            if (GetCurrentPlayerGameState() != GameState.ShipDeploying) return false;

            if (gamePiece.Type == GamePieceType.ship) gamePiece.Type = GamePieceType.water;
            else if (gamePiece.Type == GamePieceType.water) gamePiece.Type = GamePieceType.ship;
            _db.SaveChanges();
            return true;
        }

        public bool ChargeBoat(int gamePieceId)
        {
            var currentUserId = GetUserId();
            var gamePiece = _db.GamePieces.Where(gp => gp.Id == gamePieceId && gp.OwnerId != currentUserId && gp.GameId == this.GameId).Include(gp => gp.GameLink).SingleOrDefault();
            if (gamePiece == default) return false;

            var game = gamePiece.GameLink;
            if (game == default || 
                game.GameState != GameState.Ready ||
                game.PlayerOnTurn != currentUserId) return false;

            if (!game.SwitchPlayerOnTurn()) return false;
            
            gamePiece.Hidden = false;
            if (gamePiece.Type == GamePieceType.ship) gamePiece.Type = GamePieceType.shipHitted;
            else if (gamePiece.Type == GamePieceType.water) gamePiece.Type = GamePieceType.waterHitted;
            
            _db.SaveChanges();
            return true;
        }

        public void StopDeploying(Guid? gameId = null)
        {
            var userId = GetUserId();
            Game g = GetGame(gameId, false);
           
            if (g == default) return;
            if (g.Player1Id != userId && g.Player2Id != userId) return;
            if (g.GameState != GameState.ShipDeploying && g.GameState != GameState.GameCreating) return;

            if (GetCurrentPlayerGameState((Guid) GameId) == GameState.ShipDeploying)
            {
                if (g.Player1Id == userId) g.GameStateP1 = GameState.Ready;
                else g.GameStateP2 = GameState.Ready;
                g.PlayerOnTurn = userId;
                _db.SaveChanges();
            } else
            if (GetCurrentPlayerGameState((Guid)GameId) == GameState.Ready)
            {
                if (g.Player1Id == userId) g.GameStateP1 = GameState.ShipDeploying;
                else g.GameStateP2 = GameState.ShipDeploying;
                _db.SaveChanges();
            }
        }
    }
}
