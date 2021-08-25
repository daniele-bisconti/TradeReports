using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TradeReports.UI.Contracts.Services;
using TradeReports.UI.Models;
using TradeReports.UI.Services;
using TradeReports.UI.ViewModels;
using TradeReports.UI.Views;

namespace TradeReports.UI.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        private readonly IHost _host;

        public PagesTests()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(c => c.SetBasePath(appLocation))
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Core Services

            // Services
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddTransient<ReportsViewModel>();
            services.AddTransient<OperationsViewModel>();
            services.AddTransient<MainViewModel>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        // TODO WTS: Add tests for functionality you add to ReportsViewModel.
        [TestMethod]
        public void TestReportsViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ReportsViewModel));
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TestGetReportsPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ReportsViewModel).FullName);
                Assert.AreEqual(typeof(ReportsPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to OperationsViewModel.
        [TestMethod]
        public void TestOperationsViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(OperationsViewModel));
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TestGetOperationsPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(OperationsViewModel).FullName);
                Assert.AreEqual(typeof(OperationsPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to MainViewModel.
        [TestMethod]
        public void TestMainViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(MainViewModel));
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TestGetMainPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
                Assert.AreEqual(typeof(MainPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }
    }
}
