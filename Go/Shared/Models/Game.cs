using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;

using Go.Shared.Models.MongoDB;

namespace Go.Shared.Models
{
    class IllegalMoveException : Exception {
        
    }
    public enum status
    {
        BlacksTurn,
        WhitesTurn,
        BlackWon,
        WhiteWon,
        Draw
    }

    [BsonIgnoreExtraElements]
    public class Game
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public List<int[]> moveList { get; protected set; }
        public List<Delta> deltas { get; protected set; }
        public Board board { get; protected set; }
        public string player1 { get; protected set; }
        public string player2 { get; protected set; }
        public status state { get; protected set; }
        public int passesInARow { get; protected set; }

        public Game(int size, string player1, string player2)
        {
            moveList = new List<int[]>();
            deltas = new List<Delta>();
            this.player1 = player1;
            this.player2 = player2;
            this.state = status.BlacksTurn;
            this.passesInARow = 0;
            board = new Board(size);
        }

        public ArrayList playMove(int row, int col)
        {
            Board oldBoard = new Board(board.getBoard());
            int[] move = new int[2] { row, col };
            //moveList.Append(move);
            int color = 0;
            if (state == status.BlacksTurn)
                color = 1;
            else
                color = -1;
           
            ArrayList capturedPieces = board.playMove(new int[3] { row, col, color });
            if (capturedPieces == null)
                throw new IllegalMoveException();
            else
            {
                passesInARow = 0;
                moveList.Add(move);
                deltas.Add(new Delta(oldBoard, board));
                if (state == status.BlacksTurn)
                    state = status.WhitesTurn;
                else if (state == status.WhitesTurn)
                    state = status.BlacksTurn;
            }
            return capturedPieces;
        }

        public void passTurn()
        {
            passesInARow++;
            if (state == status.BlacksTurn)
                state = status.WhitesTurn;
            else if (state == status.WhitesTurn)
                state = status.BlacksTurn;
            if (passesInARow >= 2)
            {
                int[] score = this.getScore();
                if (score[0] > score[1])
                    this.state = status.BlackWon;
                else if (score[0] < score[1])
                    this.state = status.WhiteWon;
                else
                    this.state = status.Draw;
            }
        }

        public int[] getScore()
        {
            return board.score;
        }

        // Tracks changes to the board
        public class Delta
        {
            public List<int[]> deltas { get; }
            public Delta(Board state1, Board state2)
            {
                int[,] b1 = state1.getBoard();
                int[,] b2 = state2.getBoard();
                deltas = new List<int[]>();
                if (b1.GetLength(0) != b2.GetLength(0) ||
                    b1.GetLength(1) != b2.GetLength(1))
                {
                    throw new ArgumentException("Board dimensions don't match.");
                }

                int X = b1.GetLength(0);
                int Y = b1.GetLength(1);

                for (int i = 0; i < X; i++)
                {
                    for (int j = 0; j < Y; j++)
                    {
                        if (b1[i, j] != b2[i, j])
                        {
                            deltas.Add(new int[3] { i, j, b2[i, j] - b1[i, j] });
                        }
                    }
                }
            }

            /// <summary>
            /// Applies or reverts the move
            /// </summary>
            /// <param name="init">The initial board state</param>
            /// <param name="direction">The direction; 1 to apply or -1 to revert. OPTIONAL, defaults to 1.</param>
            /// <returns>New board with move applied/reverted</returns>
            public Board Apply(Board init, int direction = 1)
            {
                int[,] newBoard = init.getBoard();
                foreach (int[] delta in deltas)
                {
                    newBoard[delta[0], delta[1]] += delta[2] * direction;
                }
                return new Board(newBoard);
            }
        }
    }
}
