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
using Go.Shared.Models.MongoDB;
using Go.Shared.Models;
using Moq;
using Xunit;

namespace Testing.UnitTests.Shared.Models.Forms
{
    public class TestLoginForm: IDisposable
    {
        private Mock<ILocalStorageService> localStorageMock;
        private TestContext context;
        private IRenderedComponent<LoginForm> form;
        public TestLoginForm()
        {
            localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(s => s.RemoveItemAsync(It.IsAny<String>(), It.IsAny<CancellationToken?>()));
            localStorageMock.Setup(s => s.SetItemAsync<string>(It.IsAny<String>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()));


            context = new TestContext();
            context.Services.AddSingleton<ILocalStorageService>(localStorageMock.Object);

            LoginForm.Show();

            form = context.RenderComponent<LoginForm>();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void TestClose()
        {
            form.Find(".close").Click();
            Assert.Throws<ElementNotFoundException>(() => form.Find(".close"));
        }
    }
}
