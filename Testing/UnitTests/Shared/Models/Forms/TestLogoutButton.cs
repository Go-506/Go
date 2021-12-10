using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Blazored.LocalStorage;

using Bunit;
using Go;
using Go.Shared.Forms;
using Moq;
using Xunit;

namespace Testing.UnitTests.Shared.Models.Forms
{
    public class TestLogoutButton
    {
        [Fact]
        public void TestClick()
        {
            using var context = new TestContext();
            var component = context.RenderComponent<LogoutButton>();

            Mock<ILocalStorageService> localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken>()));

            component.Find("headtext").Click();

            localStorageMock.Verify((s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken>())), Times.Once());
        }
    }
}
