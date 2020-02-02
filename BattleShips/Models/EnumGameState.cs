using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models
{
    public enum GameState
    {
        [Display(Name = "Čekání na hráče")]
        GameCreating = 0,
        [Display(Name = "Umísťování lodí")]
        ShipDeploying,
        [Display(Name = "Útočná fáze")]
        Ready,
        [Display(Name = "Výhra hráče")]
        Winner,
        [Display(Name = "Prohra hráče")]
        Loss,
        [Display(Name = "Konec hry")]
        End
    }
}
