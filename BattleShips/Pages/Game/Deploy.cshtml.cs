using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Models;
using BattleShips.Models.ViewModel;
using BattleShips.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static BattleShips.Models.GameState;

namespace BattleShips
{
    public class DeployModel : PageModel
    {
        private readonly GameService _gs;

        public DeployModel(GameService gs)
        {
            this._gs = gs;
        }

        public GameBoardData GameBoardData { get; set; }

        public GameState GameState { get; set; }
        public GameState CurrentPlayerGameState { get; set; }

        public IActionResult OnGet(Guid? gameId)
        {
            if (gameId != default && _gs.ContinueToGame((Guid)gameId))
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.info, $"Úspěšně připojeno ke hře... ({gameId})");
            }

            if (!_gs.IsGameLoaded)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.warning, $"Nejprve je potřeba vytvořit / vybrat hru...");
                return RedirectToPage("ListGames");
            }

            var g = _gs.GetGame();
            if (g.GameState == Ready) return RedirectToPage("Play");
            if (g.GameState == End)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Hra ukončena - vyhrál {(g.GameStateP1==Winner ? g.Player1.UserName : g.Player2.UserName)}");
                _gs.UnloadGame();
                return RedirectToPage("ListGames");
            }

            GameBoardData = new GameBoardData(g, _gs.GetUserId())
            {
                PageHandler = "deploy",
                RouteDataId = true,
                HideEnemyBoards = true
            };

            CurrentPlayerGameState = _gs.GetCurrentPlayerGameState();
            GameState = g.GameState;

            return Page();
        }

        public IActionResult OnGetDeploy(int? id)
        {
            if (id == null) return Page();
            _gs.DeployUndeployBoat((int)id);


            return RedirectToPage();
        }

        public IActionResult OnGetCharge()
        {
            _gs.StopDeploying();

            return RedirectToPage();
        }
    }
}