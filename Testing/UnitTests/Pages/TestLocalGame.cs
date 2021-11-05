using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bunit;
using Go.Pages;
using Go.Shared.Models;
using Moq;
using Xunit;

namespace Testing.UnitTests.Pages
{
    public class TestLocalGame
    {
        [Fact]
        public void TestClick()
        {
            string longcomment = @"int x = 40;
            int y = 40;

            // https://bunit.dev/
            using var context = new TestContext();
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            var component = context.RenderComponent<LocalGame>(parameters => parameters.Add(p => p.dimensions, '19'));

            Mock<IBoard> local = new Mock<IBoard>();
            //local.Setup(s => s.playMove(It.IsAny<int[]>())).Returns(true);

            component.Find('canvas').Click(clientX: x, clientY: y);

            local.Verify((s => s.playMove(It.IsAny<int[]>())), Times.Once());
            ";
        }
    }
}
