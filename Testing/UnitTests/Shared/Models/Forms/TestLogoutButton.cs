using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

using Bunit;
using Go.Shared.Forms;
using Moq;
using Xunit;

namespace Testing.UnitTests.Shared.Models.Forms
{ 
    public class TestLogoutButton
    {

        private Mock<ILocalStorageService> localStorageMock;
        private TestContext context;
        public TestLogoutButton()
        {
            localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken?>()));


            context = new TestContext();
            context.Services.AddSingleton<ILocalStorageService>(localStorageMock.Object);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void TestClick()
        {
            var component = context.RenderComponent<LogoutButton>();

            component.Find(".headtext").Click();

            localStorageMock.Verify((s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken?>())), Times.Once());
        }
    }
}
