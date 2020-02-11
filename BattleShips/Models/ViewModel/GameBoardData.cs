using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models.ViewModel
{
    public class GameBoardData
    {
        public GameBoardData(Game currentGame, string currentUserId)
        {
            CurrentGame = currentGame;
            CurrentUserId = currentUserId;
        }

        public bool RouteDataId { get; set; } = false;

        public string PageHandler { get; set; } = "";

        public string Page { get; set; } = "";

        public bool HideEnemy { get; set; } = true;

        public bool HideEnemyBoards { get; set; } = false;

        public Game CurrentGame { get; }
        public string CurrentUserId { get; }
        public Dictionary<string, IEnumerable<IOrderedEnumerable<GamePiece>>> GetGameBoards() {
            var data = CurrentGame.GamePieces.OrderBy(gp => gp.CoordinateY)
                        .GroupBy(g => g.OwnerId, (key, gp) =>
                            gp.GroupBy(g => g.CoordinateY, (key, gp) => gp.OrderBy(gp => gp.CoordinateX))
                        );

            var gameBoards = new Dictionary<string, IEnumerable<IOrderedEnumerable<GamePiece>>>();
            foreach (var item in data)
            {
                gameBoards.Add(item.FirstOrDefault().FirstOrDefault().OwnerId, item);
            }
            return gameBoards;
        }
    }
}
