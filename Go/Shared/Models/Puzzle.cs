using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Shared.Models
{
    public class Puzzle
    {
        Game game;
        public Puzzle(Game game)
        {
            this.game = game;
            this.game.SetMove(game.totalMoves);
        }
    }
}
