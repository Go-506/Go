using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Go.Shared.Models;

namespace Testing.UnitTests.Shared.Models
{
    public class TestGame : IDisposable
    {
        private readonly ITestOutputHelper output;
        private Game game;

        public TestGame(ITestOutputHelper output)
        {
            game = new Game(19, "player1", "player2");
            this.output = output;
        }
        public void Dispose()
        {
            game = null;
        }

        [Fact]
        public void TestPlayMoveNoCaptured()
        {
            ArrayList captured = game.playMove(0, 0);
            Assert.Empty(captured);
        }

        [Fact]
        public void TestIllegalMove()
        {
            try
            {
                game.playMove(0, 0);
                game.playMove(0, 0);
                Assert.True(false, "failed to raise IllegalMoveException");
            }
            catch (IllegalMoveException e)
            {

            }
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
                if (btp)
                {
                    Assert.Equal(status.BlacksTurn, game.state);
                }
                else
                {
                    Assert.Equal(status.WhitesTurn, game.state);
                }
                int[] move = new int[3] { i, i, btp ? 1 : -1 };
                Assert.NotNull(game.playMove(move[0], move[1]));
                btp = !btp;
            }
        }

        [Fact]
        public void TestGameEnd()
        {
            game.passTurn();
            game.passTurn();
            Assert.True(game.state != status.BlacksTurn && game.state != status.WhitesTurn);
        }

        [Fact]
        public void TestBlackStarts()
        {
            Assert.Equal(status.BlacksTurn, game.state);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public void TestSetMove(int x)
        {
            bool btp = true;
            for (int i = 0; i < 10; i++)
            {
                int[] move = new int[3] { i, i, btp ? 1 : -1 };
                Assert.NotNull(game.playMove(move[0], move[1]));
                btp = !btp;
            }
            game.SetMove(5);
            game.SetMove(x);
        }

        [Fact]
        public void TestResetToStart()
        {
            game.playMove(1, 2);
            game.ResetToStart();
            Assert.Equal(0, game.thisMove);
        }

        [Fact]
        public void TestResult()
        {
            game.playMove(0, 0);
            game.passTurn();
            game.passTurn();
            Assert.Equal("1-0", game.Result());

            game.ResetToStart();
            game.playMove(0, 0);
            game.playMove(1, 0);
            game.passTurn();
            game.playMove(0, 1);
            game.passTurn();
            game.passTurn();
            Assert.Equal("0-1", game.Result());

            game.ResetToStart();
            game.passTurn();
            game.passTurn();
            Assert.Equal("1/2-1/2", game.Result());

            game.ResetToStart();
            game.playMove(0, 0);
            Assert.Equal("0-0", game.Result());
        }
    }
}
