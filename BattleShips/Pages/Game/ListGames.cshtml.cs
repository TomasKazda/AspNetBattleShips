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

        public void OnGet()
        {
            MyGames = _gs.GetMyGames();
            OtherGames = _gs.GetOtherGames();
        }

        public IActionResult OnGetDelete(Guid? id)
        {
            if (id == default)
            {
                return NotFound();
            }

            if (!_gs.DeleteGame((Guid)id))
            {
                return NotFound();
            }
            else
                TempData.AddMessage("MsgSuccess", $"Hra odstraněna ({id})");

            return RedirectToPage();
        }
    }
}