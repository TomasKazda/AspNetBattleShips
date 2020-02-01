using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BattleShips
{
    public class DetailsModel : PageModel
    {
        private readonly GameService _gs;

        public DetailsModel(GameService gs)
        {
            this._gs = gs;
        }

        public Dictionary<string, IEnumerable<IOrderedEnumerable<Models.GamePiece>>> GameBoards { get; set; }

        public string ThisPlayer => _gs.GetUserId();

        public IActionResult OnGet(Guid? gameId)
        {
            GameBoards = _gs.GetGameBoards(gameId);

            if (GameBoards == default)
            {
                return NotFound();
            }


            return Page();
        }
    }
}