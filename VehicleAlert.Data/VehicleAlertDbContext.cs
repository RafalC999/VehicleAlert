using Microsoft.EntityFrameworkCore;
using VehicleAlert.Data.Entities;

namespace VehicleAlert.Data
{
    public class VehicleAlertDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<ActionVehicle> VehicleActions { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Filename={Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "VehicleAlerts.sqlite")}");



        }

    }
}
