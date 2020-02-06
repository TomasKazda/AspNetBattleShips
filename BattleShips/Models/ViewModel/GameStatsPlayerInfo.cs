using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models.ViewModel
{
    public class GameStatsPlayerInfo
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int HittedShipCount { get; set; }
        public int AllShipCount { get; set; }

    }
}
