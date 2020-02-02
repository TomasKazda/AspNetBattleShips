using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Models;
using BattleShips.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Helpers;

namespace BattleShips
{
    public class ListGamesModel : PageModel
    {
        private readonly GameService _gs;

        public ListGamesModel(GameService gs)
        {
            this._gs = gs;
        }

        public IList<Game> MyGames { get; set; }
        public IList<Game> OtherGames { get; set; }

        public bool GameLoaded => _gs.IsGameLoaded;

        public Guid GameId => _gs.GameId;

        public void OnGet()
        {
            MyGames = _gs.GetMyGames();
            OtherGames = _gs.GetReadyToJoinGames();
        }

        public IActionResult OnGetDelete(Guid? id)
        {
            if (id == default)
            {
                return NotFound();
            }

            if (!_gs.DeleteGame((Guid)id))
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.warning, $"Hru nelze odstranit ({id})");
            }
            else
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Hra odstraněna ({id})");

            return RedirectToPage();
        }

        public IActionResult OnGetJoin(Guid? id)
        {
            if (id == default)
            {
                return NotFound();
            }

            if (!_gs.JoinToGame((Guid)id))
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.warning, $"Ke hře se nelze připojit! ({id})");
                return RedirectToPage();
            }
                
            TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Připojeno ke hře. ({id})");
            return RedirectToPage("Deploy");
        }
    }
}