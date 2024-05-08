using Microsoft.EntityFrameworkCore;
using System.Windows;
using VehicleAlert.Core;
using VehicleAlert.Data;

namespace VehicleAlert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static IHost? AppHost { get; private set; }
        //public App()
        //{
        //    AppHost = Host.CreateDefaultBuilder()
        //        .ConfigureServices((hostContext, services) =>
        //        {

        //        }).Build();
        //}
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            var database = new VehicleAlertDbContext();

            database.Database.EnsureCreated();

            database.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=OFF");

            DatabaseLocator.Database = database;

        }
    }
}
