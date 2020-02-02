using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Models.ViewModel
{
    public class GameBoardData
    {
        public string CurrentUserId { get; set; }
        public Game CurrentGame { get; set; }
        public Dictionary<string, IEnumerable<IOrderedEnumerable<GamePiece>>> GameBoards { get; set; }
    }
}
