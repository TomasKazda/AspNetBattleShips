using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BattleShips.Data;
using BattleShips.Models;

namespace BattleShips
{
    public class IndexModel : PageModel
    {
        private readonly BattleShips.Data.ApplicationDbContext _context;

        public IndexModel(BattleShips.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; }

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .Include(g => g.Player1)
                .Include(g => g.Player2).ToListAsync();
        }
    }
}
