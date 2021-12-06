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
    public class IllegalMoveException : Exception {
        
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
        public int size { get; protected set; }
        public int thisMove { get; protected set; }
        public int totalMoves { get; protected set; }
        public string player1 { get; protected set; }
        public string player2 { get; protected set; }
        public status state { get; protected set; }
        public int passesInARow { get; protected set; }
        public string date { get; protected set; }

        public Game(int size, string player1, string player2)
        {
            moveList = new List<int[]>();
            deltas = new List<Delta>();
            this.player1 = player1;
            this.player2 = player2;
            this.state = status.BlacksTurn;
            this.passesInARow = 0;
            this.size = size;
            this.thisMove = 0;
            this.totalMoves = 0;
            board = new Board(size);
            this.date = DateTime.Now.Date.ToString("d");
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
                moveList.Add(new int[3] { move[0], move[1], color });
                deltas.Add(new Delta(oldBoard, board));
                if (state == status.BlacksTurn)
                    state = status.WhitesTurn;
                else if (state == status.WhitesTurn)
                    state = status.BlacksTurn;
            }
            this.thisMove++;
            this.totalMoves++;
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
            deltas.Add(new Delta());
            thisMove++;
            totalMoves++;
        }

        public int[] getScore()
        {
            return board.score;
        }

        public void SetMove(int move)
        {
            if (thisMove == move)
            {
                return;
            }
            else if (thisMove < move)
            {
                do
                {
                    StepForward();
                } while (thisMove < move);
            }
            else
            {
                do
                {
                    StepBack();
                } while (thisMove > move);
            }
        }

        public bool StepForward()
        {
            if (thisMove < totalMoves - 2)
            {
                board = deltas[thisMove].Apply(board, 1);
                thisMove++;
                return true;
            }
            return false;
        }

        public bool StepBack()
        {
            if (thisMove > 0)
            {
                board = deltas[thisMove - 1].Apply(board, -1);
                thisMove--;
                return true;
            }
            return false;
        }

        // Tracks changes to the board
        public class Delta
        {
            public List<int[]> deltas { get; private set; }
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

            public Delta()
            {
                deltas = new List<int[]>();
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

        public void ResetToStart()
        {
            this.thisMove = 0;
            this.board = new Board(size);
            this.state = status.BlacksTurn;
        }

        public string Result()
        {
            if (state == status.BlackWon)
            {
                return "1-0";
            } else if (state == status.WhiteWon)
            {
                return "0-1";
            } else if (state == status.Draw)
            {
                return "1/2-1/2";
            } else
            {
                return "0-0";
            }
        }
    }
}
