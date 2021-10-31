using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Shared.Models
{
    public class Board
    {
        //1 means black stone; 0 means empty; -1 means white stone
        private int[ , ] board;
        private bool playable = true;
        private bool blackToPlay = true;

        public Board(int boardSize)
        {
            board = new int[boardSize, boardSize];
        }

        public int[ , ] getBoard()
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
        /// Play a move and update the board if successful.
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns>Move successful</returns>
        public bool playMove(int[] move)
        {
            if (moveIsLegal(move))
            {
                board[move[0], move[1]] = move[2];
                blackToPlay = !blackToPlay;
            }
            return moveIsLegal(move);
        }

        /// <summary>
        /// Decides if a move is legal.
        /// A move is legal if:
        ///     -the coords are empty
        ///     -the stone will not be immediately captured (after removing stones which that stone captured)
        ///     -the move will not recreate a previous board state
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns></returns>
        public bool moveIsLegal(int[] move)
        {
            Boolean valid = true;

            //coord not empty
            if (board[move[0], move[1]] != 0)
            {
                valid = false;
            }

            //TODO: stone will be captured

            //TODO: move will create previous board state

            return valid;
        }

        /// <summary>
        /// Gets a list of all connected stones of the same color. Stones of the same color 
        /// which are directly up/down/left/right of each other (no diagonals) are connected, and
        /// connectedness is transitive.
        /// </summary>
        /// <param name="stone">x coord, y coord, and color of the stone.</param>
        /// <returns>int[ , ]: an array of [x, y] coords, each corresponding to a connected stone</returns>
        private int[ , ] getConnected(int[] stone)
        {
            return new int[0, 0];
        }

        /// <summary>
        /// Gets a list of all liberties of a given stone. A liberty of a stone is an empty space 
        /// next to the stone or any stone connected to that stone.
        /// </summary>
        /// <param name="stone">[x, y] corresponding to the stone</param>
        /// <returns>an array of [x, y] coords which correspond to empty spaces
        /// that are liberties of the given stone</returns>
        private int[ , ] getLiberties(int[] stone)
        {
            return new int[0, 0];
        }


        /// <summary>
        /// Method to be called when a capture is detected.
        /// </summary>
        private void capture()
        {

        }
    }
}
