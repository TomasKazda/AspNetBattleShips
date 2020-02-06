using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BattleShips.Models.ViewModel;
using Helpers;

namespace BattleShips
{
    public class DetailsModel : PageModel
    {
        private readonly GameService _gs;

        public DetailsModel(GameService gs)
        {
            this._gs = gs;
        }

        public GameBoardData GameBoardData { get; set; }
        public Dictionary<string, GameStatsPlayerInfo> GameInfo { get; set; }

        public IActionResult OnGet(Guid? gameId)
        {
            var currentGame = _gs.GetGame(gameId);
            
            if (currentGame == default)
            {
                return NotFound();
            }
            if (currentGame.GameState != Models.GameState.End)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.warning, $"Infopanel nelze zpřístupnit před ukončením hry! ({currentGame.Id})");
                return RedirectToPage("/Index");
            }

            GameInfo = _gs.GetGameInfo(gameId: currentGame.Id);
            GameBoardData = new GameBoardData(currentGame, _gs.GetUserId())
            {
                HideEnemy = false
            };

            return Page();
        }
    }
}