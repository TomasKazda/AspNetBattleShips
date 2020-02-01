using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (g.GameState == Attack) return RedirectToPage("Play");
            if (g.GameState == WinnerPlayer1)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Hra ukončena - vyhrál {g.Player1.UserName}");
                _gs.UnloadGame();
                return RedirectToPage("ListGames");
            }
            if (g.GameState == WinnerPlayer2)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Hra ukončena - vyhrál {g.Player2.UserName}");
                _gs.UnloadGame();
                return RedirectToPage("ListGames");
            }

            return Page();
        }
    }
}