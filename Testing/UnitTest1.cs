using System;
using Xunit;
using Go;

namespace Testing
{
    public class UnitTest1
    {
        [Fact]
        public void TestPasses()
        {
            Assert.True(true);
        }

        [Fact]
        public void TestFails()
        {
            Assert.True(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TestIsOne(int inlineData)
        {
            Assert.Equal(1, inlineData);
        }
    }
}
