using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BattleShips.Models
{
    public class GamePiece
    {
        public GamePiece()
        {
            Hidden = true;
            Type = GamePieceType.Water;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public IdentityUser Owner { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        public Game GameLink { get; set; }

        [ForeignKey("GameLink")]
        public Guid GameId { get; set; }

        [Required]
        public GamePieceType Type { get; set; }

        [Required]
        public bool Hidden { get; set; }

        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
    }
}
