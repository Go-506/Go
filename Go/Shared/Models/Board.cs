using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Go.Shared.Models
{

    public interface IBoard
    {
        public int[,] getBoard();
        public ArrayList playMove(int[] move);
        public ArrayList moveIsLegal(int[] move);
    }
    public class Board: IBoard
    {
        //1 means black stone; 0 means empty; -1 means white stone
        private int[ , ] board;
        //[0]: black score, [1]: white score
        public int[] score;
        // [0]: white pieces captured (by black), [1]: black pieces captured (by white)
        public int[] captured;

        public Board(int boardSize)
        {
            board = new int[boardSize, boardSize];
            score = new int[] { 0, 0 };
            captured = new int[] { 0, 0 };
        }

        public int[ , ] getBoard()
        {
            return board;
        }


        /// <summary>
        /// Play a move and update the board if successful.
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns>Move successful</returns>
        public ArrayList playMove(int[] move)
        {
            ArrayList captured = moveIsLegal(move);
            if (captured != null)
            {
                if (move[2] == 1) this.captured[0] += captured.Count;
                else this.captured[1] += captured.Count;
                int[] newScore = getScore(this.board);
                this.score[0] = newScore[0];
                this.score[1] = newScore[1];
            }
            return captured;
        }

        /// <summary>
        /// Decides if a move is legal and updates the board if it is.
        /// A move is legal if:
        ///     -the coords are empty
        ///     -the stone will not be immediately captured (after removing stones which that stone captured)
        ///     -the move will not recreate a previous board state
        /// </summary>
        /// <param name="move">move[0]: the x coord. move[1]: the y coord. move[2]: 1 if black, -1 if white</param>
        /// <returns></returns>
        public ArrayList moveIsLegal(int[] move)
        {
            //check coord not empty
            if (board[move[0], move[1]] != 0)
            {
                return null;
            }

            //consider stones this move will capture (but don't update the main board)
            int[,] hypotheticalCapture = (int[,])board.Clone();
            ArrayList captured = checkCapture(hypotheticalCapture, move);
            removeStones(hypotheticalCapture, captured);

            //check if stone will be captured even after removing enemy stones it captures
            if (getLiberties(hypotheticalCapture, move).Count == 0)
            {
                return null;
            }

            //TODO: check if move will create previous board state

            //update main board if everything works
            board = hypotheticalCapture;
            return captured;
        }

        /// <summary>
        /// Gets a list of all stones connected to a given stone. Stones of the same color 
        /// which are directly up/down/left/right of each other (no diagonals) are connected, and
        /// connectedness is transitive. Stones are always connected to themselves.
        /// </summary>
        /// <param name="stone">x coord, y coord, and color of the stone.</param>
        /// <returns>ArrayList: an array of [x, y] coords, each corresponding to a connected stone</returns>
        private ArrayList getConnected(int[] stone, int[,] board)
        {
            ArrayList connected = new ArrayList();
            connected.Add(stone);

            int[] currStone = stone;
            Stack stonesToCheck = new Stack();
            stonesToCheck.Push(currStone);
            while (stonesToCheck.Count != 0)
            {
                currStone = (int[])stonesToCheck.Pop();
                Boolean[] adjacentsToCheck = { false, false, false, false };
                Boolean[] adj = findAdjacents(currStone, board);
                int x = currStone[0];
                int y = currStone[1];

                //the 4 cardinal directions
                for (int dir = 0; dir < 4; dir++)
                {
                    //up
                    if (dir == 0)
                    {
                        if (adj[dir] && !contains(connected, new int[] { x, y - 1 }))
                        {
                            connected.Add(new int[] { x, y - 1 });
                            adjacentsToCheck[0] = true;
                            stonesToCheck.Push(new int[] { x, y - 1 });
                        }
                    }

                    //down
                    if (dir == 1)
                    {
                        if (adj[dir] && !contains(connected, new int[] { x, y + 1 }))
                        {
                            connected.Add(new int[] { x, y + 1 });
                            adjacentsToCheck[1] = true;
                            stonesToCheck.Push(new int[] { x, y + 1 });
                        }
                    }

                    //left
                    if (dir == 2)
                    {
                        if (adj[dir] && !contains(connected, new int[] { x - 1, y }))
                        {
                            connected.Add(new int[] { x - 1, y });
                            adjacentsToCheck[2] = true;
                            stonesToCheck.Push(new int[] { x - 1, y });
                        }
                    }

                    //right
                    if (dir == 3)
                    {
                        if (adj[dir] && !contains(connected, new int[] { x + 1, y }))
                        {
                            connected.Add(new int[] { x + 1, y });
                            adjacentsToCheck[3] = true;
                            stonesToCheck.Push(new int[] { x + 1, y });
                        }
                    }
                }

            }
            return connected;
        }

        /// <summary>
        /// Gets a list of all liberties of a given stone on a given board. 
        /// A liberty of a stone is an empty space next to the stone or 
        /// any stone connected to that stone.
        /// 
        /// Checks every cell on the board exactly once and determines if it's
        /// an empty cell that's next to a stone connected to the given stone.
        /// </summary>
        /// <param name="stone">[x, y] corresponding to the stone</param>
        /// <returns>an array of [x, y] coords which correspond to empty spaces
        /// that are liberties of the given stone</returns>
        private ArrayList getLiberties(int[,] board, int[] stone)
        {
            ArrayList libs = new ArrayList();

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == 0)
                    {
                        Boolean[] b = bounds(new int[] { x, y });
                        Boolean isLib = false;
                        ArrayList connected = getConnected(stone, board);
                        //up
                        if (b[0] && contains(connected, new int[] { x, y - 1 }))
                        {
                            isLib = true;
                        }

                        //down
                        if (b[1] && contains(connected, new int[] { x, y + 1 }))
                        {
                            isLib = true;
                        }

                        //left
                        if (b[2] && contains(connected, new int[] { x - 1, y }))
                        {
                            isLib = true;
                        }

                        //right
                        if (b[3] && contains(connected, new int[] { x + 1, y }))
                        {
                            isLib = true;
                        }

                        if (isLib)
                        {
                            libs.Add(new int[] { x, y });
                        }
                    }
                }
            }

            return libs;
        }

        /// <summary>
        /// Determine which stones a move captures. A move captures a stone if it
        /// removes all liberties of that stone. This method looks to the intersections
        /// directly adjacent to it and if a stone of the opposite color is found, checks
        /// the liberties of that stone. If it is 0, it and all stones connected to it
        /// are captured.
        /// </summary>
        /// <param name="board">The board on which to look for captures. We want this to 
        /// be different from the global board because we want to figure out if a move 
        /// will capture any pieces before committing that move to the global board.</param>
        /// <param name="moveJustPlayed">[0]: x, [1]: y, [2]: color</param>
        /// <returns></returns>
        private ArrayList checkCapture(int[ , ] board, int[] moveJustPlayed)
        {
            ArrayList captured = new ArrayList();
            int x = moveJustPlayed[0];
            int y = moveJustPlayed[1];
            int color = moveJustPlayed[2];
            board[x, y] = color;
            Boolean[] b = bounds(moveJustPlayed);

            if (b[0] && board[x, y-1] == -1 * color)
            {
                int[] up = new int[] { x, y - 1 };
                if (getLiberties(board, up).Count == 0)
                {
                    if (!contains(captured, up))
                    captured.AddRange(getConnected(up, board));
                }
            }

            if (b[1] && board[x, y + 1] == -1 * color)
            {
                int[] down = new int[] { x, y + 1 };
                if (getLiberties(board, down).Count == 0)
                {
                    if (!contains(captured, down))
                        captured.AddRange(getConnected(down, board));
                }
            }

            if (b[2] && board[x - 1, y] == -1 * color)
            {
                int[] left = new int[] { x - 1, y };
                if (getLiberties(board, left).Count == 0)
                {
                    if (!contains(captured, left))
                        captured.AddRange(getConnected(left, board));
                }
            }

            if (b[3] && board[x + 1, y] == -1 * color)
            {
                int[] right = new int[] { x + 1, y };
                if (getLiberties(board, right).Count == 0)
                {
                    if (!contains(captured, right))
                        captured.AddRange(getConnected(right, board));
                }
            }

            return captured;
        }

        /// <summary>
        /// Remove the given stones from the given board.
        /// </summary>
        private void removeStones(int[,] board, ArrayList stones)
        {
            foreach (int[] pos in stones)
            {
                board[pos[0], pos[1]] = 0;
            }
        }

        /// <summary>
        /// Helper method to prevent out of bounds indexing.
        /// </summary>
        /// <param name="location">The location in question</param>
        /// <returns>[x, x, x, x]: x=false if the adjacent intersection 
        /// is out of bounds, x=true otherwise. Corresponds
        /// to "up/down/left/right". ie at location [0, 0], it will
        /// return [false, true, false, true].</returns>
        private Boolean[] bounds(int[] location)
        {
            Boolean[] bound = { false, false, false, false };

            if (location[1] > 0)
            {
                bound[0] = true;
            }
            if (location[1] < board.GetLength(0) - 1)
            {
                bound[1] = true;
            }
            if (location[0] > 0)
            {
                bound[2] = true;
            }
            if (location[0] < board.GetLength(1) - 1)
            {
                bound[3] = true;
            }

            return bound;
        }

        /// <summary>
        /// Helper method to find adjacent stones of the same color.
        /// Functions similarly to bounds().
        /// </summary>
        /// <param name="location">The location in question</param>
        /// <returns>[x, x, x, x] where x=false if there is no adjacent stone
        /// of the same color, true otherwise. Corresponds to 
        /// up/down/left/right</returns>
        private Boolean[] findAdjacents(int[] location, int[,] board)
        {
            Boolean[] adjacents = { false, false, false, false };
            int x = location[0];
            int y = location[1];
            Boolean[] b = bounds(location);
            if (b[0] && board[x, y-1] == board[x, y])
            {
                adjacents[0] = true;
            }
            if (b[1] && board[x, y+1] == board[x, y])
            {
                adjacents[1] = true;
            }
            if (b[2] && board[x-1, y] == board[x, y])
            {
                adjacents[2] = true;
            }
            if (b[3] && board[x+1, y] == board[x, y])
            {
                adjacents[3] = true;
            }
            return adjacents;
        }

        /// <summary>
        /// Helper method for getConnected(). 
        /// </summary>
        /// <param name="stone"></param>
        /// <param name="list"></param>
        /// <returns>True if the stone has an adjacent stone not in list,
        /// false otherwise</returns>
        private Boolean hasAdjacentsNotInList(int[] stone, ArrayList list)
        {
            Boolean[] adj = findAdjacents(stone, board);

            if (adj[0] && !list.Contains(new int[stone[0], stone[1] - 1]))
            {
                return true;
            }
            if (adj[1] && !list.Contains(new int[stone[0], stone[1] + 1]))
            {
                return true;
            }
            if (adj[2] && !list.Contains(new int[stone[0] - 1, stone[1]]))
            {
                return true;
            }
            if (adj[3] && !list.Contains(new int[stone[0] + 1, stone[1]]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helper method to find if an ArrayList contains a specified
        /// int[]. The built in .Contains() method cannot be used because
        /// we do not want to look for object references to the specific int[]
        /// but rather the values contained by the int[].
        /// </summary>
        /// <param name="arrayList"></param>
        /// <param name="stone"></param>
        /// <returns></returns>
        private Boolean contains(ArrayList arrayList, int[] stone)
        {
            Boolean contains = false;
            int[][] list = (int[][])arrayList.ToArray(typeof(int[]));
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i][0] == stone[0] && list[i][1] == stone[1])
                {
                    contains = true;
                }
            }
            return contains;
        }

        /// <summary>
        /// small helper function for getscore method
        /// </summary>
        /// <param name="bounds></param>
        /// <returns>True if all 4 bounds in the param array are true, else false</returns>
        public bool allBoundsReached(bool[] bounds)
        {
            for (int i = 0; i < 4; i++)
            {
                if (bounds[i] == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Return the score for the given board. The score for each player
        /// is simply the number of stones they have on the board.
        /// </summary>
        /// <param name="board"></param>
        /// 
        public int[] getScore(int[,] board)
        {
            int[] boardScore = new int[] { 0, 0 };
            int sectionSize = 0;
            int sectionColorTouched = 0;
            bool sectionControlled;
            // array for tracking if { top, left, bottom, right } bounds adj to section
            bool[] boundsReached = new bool[] { false, false, false, false };
            int[] current = new int[] { 0, 0, 0 };

            // makes a bool 2d array representative of whether each space on board has been visited
            bool[,] visited = new bool[board.GetLength(0), board.GetLength(1)];
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    visited[x, y] = false;
                }
            }

            // make queues for each section of a certain color (or empty)
            // as well as for the neighbors within that section of the same color
            Queue<int[]> neighbors = new Queue<int[]>();
            Queue<int[]> sections = new Queue<int[]>();
            bool neighborsEmpty = false;
            bool sectionsEmpty = false;
            sections.Enqueue(new int[] { 0, 0, board[0, 0] });

            do {
                sectionsEmpty = false;
                do
                {
                    if (sections.Count == 0)
                    {
                        sectionsEmpty = true;
                    }
                    else
                    {
                        current = sections.Dequeue();
                        
                    }
                } while (!sectionsEmpty && visited[current[0], current[1]]);
                if (!sectionsEmpty)
                {
                    sectionControlled = true;
                    sectionColorTouched = 0;
                    sectionSize = 0;
                    boundsReached = new bool[] { false, false, false, false };
                    do
                    {
                        neighborsEmpty = false;
                        if (sectionSize > 0)
                        {
                            do
                            {
                                if (neighbors.Count == 0)
                                {
                                    neighborsEmpty = true;
                                }
                                else
                                {
                                    current = neighbors.Dequeue();
                                }
                            } while (!neighborsEmpty && visited[current[0], current[1]]);
                        }
                        if (!neighborsEmpty)
                        {
                            sectionSize++;
                            visited[current[0], current[1]] = true;
                            // checks the space above current
                            if (current[1] == 0)
                            {
                                boundsReached[0] = true;
                            }
                            else if (!visited[current[0], current[1] - 1] && board[current[0], current[1] - 1] == current[2])
                            {
                                neighbors.Enqueue(new int[] { current[0], current[1] - 1, current[2] });
                            }
                            else if (!visited[current[0], current[1] - 1] && board[current[0], current[1] - 1] != current[2])
                            {
                                sections.Enqueue(new int[] { current[0], current[1] - 1, board[current[0], current[1] - 1] });
                            }
                            if (current[2] == 0 && current[1] != 0 && board[current[0], current[1] - 1] != 0)
                            {
                                if (sectionColorTouched == 0)
                                {
                                    sectionColorTouched = board[current[0], current[1] - 1];
                                }
                                else if (sectionColorTouched != board[current[0], current[1] - 1])
                                {
                                    sectionControlled = false;
                                }
                            }
                            // checks the space left of current
                            if (current[0] == 0)
                            {
                                boundsReached[1] = true;
                            }
                            else if (!visited[current[0] - 1, current[1]] && board[current[0] - 1, current[1]] == current[2])
                            {
                                neighbors.Enqueue(new int[] { current[0] - 1, current[1], current[2] });
                            }
                            else if (!visited[current[0] - 1, current[1]] && board[current[0] - 1, current[1]] != current[2])
                            {
                                sections.Enqueue(new int[] { current[0] - 1, current[1], board[current[0] - 1, current[1]] });
                            }
                            if (current[2] == 0 && current[0] != 0 && board[current[0] - 1, current[1]] != 0)
                            {
                                if (sectionColorTouched == 0)
                                {
                                    sectionColorTouched = board[current[0] - 1, current[1]];
                                }
                                else if (sectionColorTouched != board[current[0] - 1, current[1]])
                                {
                                    sectionControlled = false;
                                }
                            }
                            // checks the space beneath current
                            if (current[1] == board.GetLength(0) - 1)
                            {
                                boundsReached[2] = true;
                            }
                            else if (!visited[current[0], current[1] + 1] && board[current[0], current[1] + 1] == current[2])
                            {
                                neighbors.Enqueue(new int[] { current[0], current[1] + 1, current[2] });
                            }
                            else if (!visited[current[0], current[1] + 1] && board[current[0], current[1] + 1] != current[2])
                            {
                                sections.Enqueue(new int[] { current[0], current[1] + 1, board[current[0], current[1] + 1] });
                            }
                            if (current[2] == 0 && current[1] != board.GetLength(0) - 1 && board[current[0], current[1] + 1] != 0)
                            {
                                if (sectionColorTouched == 0)
                                {
                                    sectionColorTouched = board[current[0], current[1] + 1];
                                }
                                else if (sectionColorTouched != board[current[0], current[1] + 1])
                                {
                                    sectionControlled = false;
                                }
                            }
                            // checks the space right of current
                            if (current[0] == board.GetLength(1) - 1)
                            {
                                boundsReached[3] = true;
                            }
                            else if (!visited[current[0] + 1, current[1]] && board[current[0] + 1, current[1]] == current[2])
                            {
                                neighbors.Enqueue(new int[] { current[0] + 1, current[1], current[2] });
                            }
                            else if (!visited[current[0] + 1, current[1]] && board[current[0] + 1, current[1]] != current[2])
                            {
                                sections.Enqueue(new int[] { current[0] + 1, current[1], board[current[0] + 1, current[1]] });
                            }
                            if (current[2] == 0 && current[0] != board.GetLength(1) - 1 && board[current[0] + 1, current[1]] != 0)
                            {
                                if (sectionColorTouched == 0)
                                {
                                    sectionColorTouched = board[current[0] + 1, current[1]];
                                }
                                else if (sectionColorTouched != board[current[0] + 1, current[1]])
                                {
                                    sectionControlled = false;
                                }
                            }
                        }
                    } while (neighbors.Count != 0) ;
                    // update the score based on the previous segment checked
                    if (current[2] == 1)
                    {
                        boardScore[0] += sectionSize;
                    }
                    else if (current[2] == -1)
                    {
                        boardScore[1] += sectionSize;
                    }
                    else if (sectionControlled && sectionColorTouched == 1 && !allBoundsReached(boundsReached))
                    {
                        boardScore[0] += sectionSize;
                    }
                    else if (sectionControlled && sectionColorTouched == -1 && !allBoundsReached(boundsReached))
                    {
                        boardScore[1] += sectionSize;
                    }
                }
            } while (sections.Count != 0);
            boardScore[0] += captured[0];
            boardScore[1] += captured[1];
            return boardScore;
        }
    }
}
