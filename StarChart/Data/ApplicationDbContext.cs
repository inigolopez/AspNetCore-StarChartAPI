using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var sun = new CelestialObject()
            {
                Id = 1,
                Name = "Sun",
                OrbitalPeriod = new TimeSpan(0, 0, 0, 0),
                OrbitedObjectId = 0,
                Satellites = new List<CelestialObject>()
            };

            var earth = new CelestialObject()
            {
                Id = 2,
                Name = "Earth",
                OrbitalPeriod = new TimeSpan(365, 0, 0, 0),
                OrbitedObjectId = 1,
                Satellites = new List<CelestialObject>()
            };

            var moon = new CelestialObject()
            {
                Id = 3,
                Name = "Moon",
                OrbitalPeriod = new TimeSpan(28, 0, 0, 0),
                OrbitedObjectId = 2,
                Satellites = new List<CelestialObject>()
            };

            var jupiter = new CelestialObject()
            {
                Id = 4,
                Name = "Jupiter",
                OrbitalPeriod = new TimeSpan(876, 0, 0, 0),
                OrbitedObjectId = 1,
                Satellites = new List<CelestialObject>()
            };

            var calixto = new CelestialObject()
            {
                Id = 5,
                Name = "Calixto",
                OrbitalPeriod = new TimeSpan(13, 0, 0, 0),
                OrbitedObjectId = 4,
                Satellites = new List<CelestialObject>()
            };

            sun.Satellites.Add(earth);
            earth.Satellites.Add(moon);

            modelBuilder.Entity<CelestialObject>().HasData(moon);
            modelBuilder.Entity<CelestialObject>().HasData(earth); 
            modelBuilder.Entity<CelestialObject>().HasData(sun);
            modelBuilder.Entity<CelestialObject>().HasData(jupiter);
            modelBuilder.Entity<CelestialObject>().HasData(calixto);
        }

        public DbSet<CelestialObject> CelestialObjects { get; set; }
    }
}
