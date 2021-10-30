using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Shared.Models
{
    public class Board
    {
        //1 means black stone; 0 means empty; -1 means white stone
        private int[][] board;
        private bool playable = true;
        private bool blackToPlay = true;

        public int[][] getBoard()
        {
            return board;
        }

        public bool getPlayable()
        {
            return playable;
        }

        public bool getBlackToPlay()
        {
            return blackToPlay;
        }

        /// <summary>
        /// Play a move.
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns>Move successful</returns>
        public bool playMove(int[] move)
        {
            return moveIsLegal(move);
        }

        /// <summary>
        /// Decides if a move is legal.
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns></returns>
        public bool moveIsLegal(int[] move)
        {
            return true;
        }
    }
}
