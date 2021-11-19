using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;
using Go.Shared.Models;

namespace Testing.UnitTests.Shared.Models
{
    public class TestBoard : IDisposable
    {

        private readonly ITestOutputHelper output;
        private Board board;

        public TestBoard(ITestOutputHelper output)
        {
            board = new Board(19);
            this.output = output;
        }

        public void Dispose()
        {
            board = null;
        }

        [Theory]
        [InlineData(5)]
        [InlineData(19)]
        public void TestCreateBoard(int dim)
        {
            Board test = new Board(dim);
            int x = test.getBoard().GetLength(0);
            int y = test.getBoard().GetLength(1);
            Assert.Equal(x, dim);
            Assert.Equal(y, dim);
        }

        [Fact]
        public void TestBoardPlayable()
        {
            //Assert.True(board.getPlayable());
        }

        [Fact]
        public void TestBlackStarts()
        {
            //Assert.True(board.getBlackToPlay());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void TestLegalMovesHappyPath(int numMoves)
        {
            bool btp = true;
            for (int i = 0; i < numMoves; i++)
            {
                int[] move = new int[3] { i, i, btp ? 1 : -1 };
                //Assert.Equal(board.getBlackToPlay(), btp);
                Assert.NotNull(board.playMove(move));
                btp = !btp;
            }
        }

        [Fact]
        public void TestBasicCapture()
        {
            int[][] moves = new int[3][] {
                new int[3] { 0, 1, 1 },
                new int[3] { 0, 0, -1 },
                new int[3] { 1, 0, 1 }
            };

            foreach (int[] move in moves)
            {
                Assert.NotNull(board.playMove(move));
            }
            // Checking that the second token has been cleared.
            Assert.Equal(0, board.getBoard()[0, 0]);
            // Checking that only correct tokens have been removed
            Assert.Equal(1, board.getBoard()[0, 1]);
            Assert.Equal(1, board.getBoard()[1, 0]);
        }

        [Fact]
        public void TestComplexCapture()
        {
            int[][] moves = new int[6][]
            {
                new int[3] {0, 0, 1 },
                new int[3] {1, 1, -1 },
                new int[3] {1, 0, 1 },
                new int[3] {0, 2, -1 },
                new int[3] {0, 1, 1 },
                new int[3] {2, 0, -1 }
            };

            int[][] captured = new int[3][]
            {
                new int[2] {0, 0 },
                new int[2] {0, 1 },
                new int[2] {1, 0 }
            };

            int[][] white = new int[3][]
            {
                new int[2] {1, 1 },
                new int[2] {2, 0 },
                new int[2] {0, 2 },

            };

            foreach (int[] move in moves)
            {
                Assert.NotNull(board.playMove(move));
            }

            int[,] endBoard = board.getBoard();

            foreach (int[] square in captured)
            {
                Assert.Equal(0, endBoard[square[0], square[1]]);
            }

            foreach (int[] square in white)
            {
                Assert.Equal(-1, endBoard[square[0], square[1]]);
            }
            
        }

        [Fact]
        public void TestSimpleCaptureCenterBoard()
        {
            int[][] moves = new int[8][]
            {
                new int[3] {10, 10, 1 },
                new int[3] {11, 10, -1 },
                new int[3] {0, 0, 1 },
                new int[3] {9, 10, -1 },
                new int[3] {0, 1, 1 },
                new int[3] {10, 11, -1 },
                new int[3] {1, 0, 1 },
                new int[3] {10, 9, -1 },
            };

            int[] captured = new int[2] { 10, 10 };

            int[][] white = new int[4][]
            {
                new int[2] {11, 10 },
                new int[2] {9, 10 },
                new int[2] {10, 11 },
                new int[2] {10, 9 }
            };

            int[][] black = new int[3][]
            {
                new int[2] {0, 0},
                new int[2] {0, 1},
                new int[2] {1, 0}
            };

            foreach (int[] move in moves)
            {
                Assert.NotNull(board.playMove(move));
            }

            int[,] endBoard = board.getBoard();

            Assert.Equal(0, endBoard[captured[0], captured[1]]);

            foreach (int[] square in white)
            {
                Assert.Equal(-1, endBoard[square[0], square[1]]);
            }

            foreach (int[] square in black)
            {
                Assert.Equal(1, endBoard[square[0], square[1]]);
            }
        }

        [Fact]
        public void TestComplexCaptureCenterBoard()
        {
            int[][] moves = new int[][]
            {
                new int[3] {10, 10, 1 },
                new int[3] {11, 10, -1 },
                new int[3] {10, 11, 1 },
                new int[3] {9,  10, -1 },
                new int[3] {0,  1, 1 },
                new int[3] {10, 12, -1 },
                new int[3] {1,  0, 1 },
                new int[3] {10, 9, -1 },
                new int[3] {2,  0, 1},
                new int[3] {9,  11, -1},
                new int[3] {2,  1, 1},
                new int[3] {11, 11, -1}
            };

            int[][] captured = new int[][] { 
                new int[2] { 10, 10 }, 
                new int[2] { 10, 11 } };

            int[][] white = new int[][]
            {
                new int[2] {11, 10 },
                new int[2] {9, 10 },
                new int[2] {10, 12 },
                new int[2] {10, 9 },
                new int[2] {9, 11},
                new int[2] {11, 11}
            };

            int[][] black = new int[][]
            {
                new int[2] {0, 1},
                new int[2] {1, 0},
                new int[2] {2, 0 },
                new int[2] {2, 1 }
            };

            foreach (int[] move in moves)
            {
                Assert.NotNull(board.playMove(move));
            }

            int[,] endBoard = board.getBoard();

            foreach (int[] square in captured)
            {
                Assert.Equal(0, endBoard[square[0], square[1]]);
            }

            foreach (int[] square in white)
            {
                Assert.Equal(-1, endBoard[square[0], square[1]]);
            }

            foreach (int[] square in black)
            {
                Assert.Equal(1, endBoard[square[0], square[1]]);
            }
        }

        [Fact]
        public void TestDoubleMoveIllegal()
        {
            Assert.NotNull(board.playMove(new int[3] { 0, 0, 1 }));
            Assert.Null(board.playMove(new int[3] { 0, 0, -1 }));
            Assert.Equal(1, board.getBoard()[0, 0]);
        }

        [Fact]
        public void TestSuicideIllegal()
        {
            int[][] moves = new int[][]
            {
                new int[] {1, 0, 1 },
                new int[] {10, 10, -1 },
                new int[] {0, 1, 1 },
                new int[] {0, 0, -1}
            };

            for (int i = 0; i < 3; i++)
            {
                Assert.NotNull(board.playMove(moves[i]));
            }
            Assert.Null(board.playMove(moves[3]));
        }

        [Fact]
        public void TestCaptureNotSuicide()
        {
            int[][] moves = new int[][]
            {
                new int[] {2, 0, 1},
                new int[] {1, 0, -1},
                new int[] {1, 1, 1},
                new int[] {0, 1, -1},
                new int[] {0, 0, 1},
            };
            
            foreach (int[] move in moves)
            {
                Assert.NotNull(board.playMove(move));
            }
        }

        [Fact]
        public void TestGamePlayable()
        {
            //Assert.True(board.getPlayable());
        }

        [Fact]
        public void TestGameEnd()
        {
            //board.skip();
           // board.skip();
            //Assert.False(board.getPlayable());
        }
    }
}