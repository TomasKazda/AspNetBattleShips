using BattleShips.Models;
using BattleShips.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace BattleShips
{
    public class CreateNewModel : PageModel
    {
        readonly GameService _gs;

        public CreateNewModel(GameService gs)
        {
            _gs = gs;
        }
        public Game GameData { get; set; }

        private Guid newGame()
        {
            return _gs.NewGame();
        }
        public IActionResult OnGet()
        {
            if (!_gs.IsGameLoaded) return RedirectToPage("Deploy", new { gameKey = newGame() });

            GameData = _gs.GetGame();
            return Page();
        }
        public IActionResult OnGetForce()
        {
            return RedirectToPage("Deploy", new { gameKey = newGame() });
        }
    }
}