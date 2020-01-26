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
        Water,
        [Display(Name = "Vedle")]
        WaterHitted,
        [Display(Name = "Loď")]
        Ship,
        [Display(Name = "Zasaženo")]
        ShipHitted
    }
}
