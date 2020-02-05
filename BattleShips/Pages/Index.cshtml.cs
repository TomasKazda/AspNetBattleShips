using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BattleShips.Services;

namespace BattleShips.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly GameService _gs;

        public IndexModel(GameService gs)
        {
            this._gs = gs;
        }

        public bool GameLoaded => _gs.IsGameLoaded;

        public void OnGet()
        {
           
        }
    }
}
