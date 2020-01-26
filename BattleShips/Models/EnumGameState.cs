using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models
{
    public enum GameState
    {
        [Display(Name = "Umísťování lodí")]
        ShipDeploying = 1,
        [Display(Name = "Útočná fáze")]
        Attack,
        [Display(Name = "Výhra hráče č.1")]
        WinnerPlayer1,
        [Display(Name = "Výhra hráče č.2")]
        WinnerPlayer2
    }
}
