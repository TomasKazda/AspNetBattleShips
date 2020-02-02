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
            PlayerOnTurn = null;
            GameStateP1 = GameState.ShipDeploying;
            GameStateP2 = GameState.GameCreating;
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

        public GameState GameStateP1 { get; set; }

        public GameState GameStateP2 { get; set; }

        [NotMapped]
        public GameState GameState
        {
            get
            {
                if (GameStateP2 == GameState.GameCreating) return GameState.GameCreating;

                if (GameStateP1 == GameState.ShipDeploying || GameStateP2 == GameState.ShipDeploying) return GameState.ShipDeploying;

                if (GameStateP1 == GameState.Ready && GameStateP1 == GameStateP2) return GameState.Ready;
                
                return GameState.End;
            }
        }

        public string PlayerOnTurn { get; set; }

        public ICollection<GamePiece> GamePieces { get; set; }

    }
}
