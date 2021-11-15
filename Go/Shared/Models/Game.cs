using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;

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
        public Board board { get; protected set; }
        public string player1 { get; protected set; }
        public string player2 { get; protected set; }
        public status state { get; protected set; }
        public int passesInARow { get; protected set; }

        public Game(int size, string player1, string player2)
        {
            moveList = new List<int[]>();
            this.player1 = player1;
            this.player2 = player2;
            this.state = status.BlacksTurn;
            this.passesInARow = 0;
            board = new Board(size);
        }

        public ArrayList playMove(int row, int col)
        {
            int[] move = new int[2] { row, col };
            //moveList.Append(move);
            int color = 0;
            if (state == status.BlacksTurn)
                color = 1;
            else
                color = -1;
           
            ArrayList capturedPeices = board.playMove(new int[3] { row, col, color });
            if (capturedPeices == null)
                throw new IllegalMoveException();
            else
            {
                passesInARow = 0;
                moveList.Add(move);
                if (state == status.BlacksTurn)
                    state = status.WhitesTurn;
                else if (state == status.WhitesTurn)
                    state = status.BlacksTurn;
            }
            return capturedPeices;
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
    }
}
