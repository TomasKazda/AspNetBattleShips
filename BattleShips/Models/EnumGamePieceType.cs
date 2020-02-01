using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models
{
    public enum GamePieceType
    {
        [Display(Name = "Voda")]
        water,
        [Display(Name = "Vedle")]
        waterHitted,
        [Display(Name = "Loď")]
        ship,
        [Display(Name = "Zasaženo")]
        shipHitted
    }
}
