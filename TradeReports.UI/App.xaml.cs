﻿using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Sqlite;

using TradeReports.UI.Contracts.Services;
using TradeReports.UI.Contracts.Views;
using TradeReports.UI.Models;
using TradeReports.UI.Services;
using TradeReports.UI.ViewModels;
using TradeReports.UI.Views;
using TradeReports.Core.Repository;
using Microsoft.EntityFrameworkCore;
using TradeReports.Core.Services;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Analytics.Interfaces;
using TradeReports.Core.Analytics.Services;
using Serilog;

namespace TradeReports.UI
{
    // For more inforation about application lifecyle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

    // WPF UI elements use language en-US by default.
    // If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
    // Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
    public partial class App : Application
    {
        private IHost _host;

        public T GetService<T>()
            where T : class
            => _host.Services.GetService(typeof(T)) as T;

        public App()
        {
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // Specifying the configuration for serilog
            Log.Logger = new LoggerConfiguration() // initiate the logger configuration
                            .WriteTo.File(Path.Combine(appLocation, @"Logs/Log.log"), rollingInterval: RollingInterval.Day)
                            .Enrich.FromLogContext() //Adds more information to our logs from built in Serilog 
                            .WriteTo.Console() // decide where the logs are going to be shown
                            .CreateLogger(); //initialise the logger

            Log.Logger.Information("Application Starting");

            // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation);
                    })
                    .ConfigureServices(ConfigureServices)
                    .UseSerilog()
                    .Build();

            await _host.StartAsync();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {

            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Activation Handlers
            services.AddDbContext<OperationContext>(options =>
            {
                options.UseSqlite(@"Data Source=OperationsDB.db;");
                options.UseLazyLoadingProxies();
            }
            );

            // Core Services
            services.AddSingleton<IOperationsServiceAsync, OperationsService>();
            services.AddSingleton<ICategoryServiceAsync, CategoryServiceAsync>();
            services.AddSingleton<IPosServiceAsync, PosServiceAsync>();
            services.AddSingleton<ICapitalService, CapitalServiceAsync>();
            services.AddSingleton<IOperationsAnalysisService, OperationsAnalysisService>();

            // Services
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<OperationsViewModel>();
            services.AddTransient<OperationsPage>();

            services.AddTransient<ReportsViewModel>();
            services.AddTransient<ReportsPage>();

            services.AddTransient<AddOperationPage>();
            services.AddTransient<AddOperationViewModel>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            _host = null;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
        }
    }
}
