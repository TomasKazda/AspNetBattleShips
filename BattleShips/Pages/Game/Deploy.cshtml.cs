using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Services;
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

        [TempData]
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (!_gs.IsGameLoaded) return RedirectToPage("ListGames");

            var g = _gs.GetGame();
            if (g.GameState == Attack) return RedirectToPage("Play");
            if (g.GameState == WinnerPlayer1)
            {
                Message = $"Hra ukončena - vyhrál {g.Player1.UserName}";
                return RedirectToPage("ListGames");
            }
            if (g.GameState == WinnerPlayer2)
            {
                Message = $"Hra ukončena - vyhrál {g.Player2.UserName}";
                return RedirectToPage("ListGames");
            }

            return Page();
        }
    }
}