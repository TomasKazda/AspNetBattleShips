using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BattleShips.Services;

namespace BattleShips
{
    public class CreateNewModel : PageModel
    {
        private readonly GameService gs;

        public CreateNewModel(GameService gs)
        {
            this.gs = gs;
        }

        public Guid GameGuid { get; set; }
        public string UserId { get; set; }

        public void OnGet()
        {
            GameGuid = gs.GameId;
            UserId = gs.GetUserId();
        }
    }
}