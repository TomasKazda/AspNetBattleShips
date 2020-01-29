using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BattleShips.Models
{
    public class Game
    {
        public Game()
        {
            Player1OnTurn = true;
            GameState = GameState.ShipDeploying;
            Player2 = null;
            Player2Id = null;
            GameCreatedAt = DateTime.UtcNow;
        }

        public Game(Guid id, string player1Id): this()
        {
            Player1Id = player1Id;
            Id = id;
        }

        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime GameCreatedAt { get; set; }

        [Required]
        public IdentityUser Player1 { get; set; }

        [ForeignKey("Player1")]
        public string Player1Id { get; set; }

        public IdentityUser Player2 { get; set; }

        [ForeignKey("Player2")]
        public string Player2Id { get; set; }

        public GameState GameState { get; set; }

        public bool Player1OnTurn { get; set; }

        public ICollection<GamePiece> GamePieces { get; set; }

    }
}
