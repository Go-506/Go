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
        [Fact]
        public void TestClick()
        {
            Mock<ILocalStorageService> localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken?>()));


            using var context = new TestContext();
            context.Services.AddSingleton<ILocalStorageService>(localStorageMock.Object);
            var component = context.RenderComponent<LogoutButton>();

            component.Find(".headtext").Click();

            localStorageMock.Verify((s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken?>())), Times.Once());
        }
    }
}
