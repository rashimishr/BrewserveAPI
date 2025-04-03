using System.Diagnostics.CodeAnalysis;
using BrewServe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewServeData.EF_Core;

/// <summary>
///     Represents the database context for the Beer application.
/// </summary>
[ExcludeFromCodeCoverage]
public class BrewServeDbContext(DbContextOptions<BrewServeDbContext> options) : DbContext(options)
{
    /// <summary>
    ///     Gets or sets the DbSet of beers.
    /// </summary>
    public DbSet<Beer> Beers { get; set; }

    /// <summary>
    ///     Gets or sets the DbSet of breweries.
    /// </summary>
    public DbSet<Brewery> Breweries { get; set; }

    /// <summary>
    ///     Gets or sets the DbSet of bars.
    /// </summary>
    public DbSet<Bar> Bars { get; set; }
    /// <summary>
    ///     Gets or sets the DbSet of bar-beers-link.
    /// </summary>
    public DbSet<BarBeerLink> BarBeers { get; set; }
    /// <summary>
    ///     Gets or sets the DbSet of brewery-beers-link.
    /// </summary>
    public DbSet<BreweryBeerLink> BreweryBeers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //table relationship and table configurations
        modelBuilder.Entity<Beer>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Brewery>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Bar>()
            .HasKey(b => b.Id);

        //Many - to - Many relation
        modelBuilder.Entity<BarBeerLink>()
            .HasKey(bb => new { bb.BarId, bb.BeerId });
        modelBuilder.Entity<BarBeerLink>()
            .HasOne(bb => bb.Bar)
            .WithMany(b => b.BarBeers)
            .HasForeignKey(b => b.BarId);
        modelBuilder.Entity<BarBeerLink>()
            .HasOne(bb => bb.Beer)
            .WithMany(b => b.BarBeers)
            .HasForeignKey(b => b.BeerId);
        modelBuilder.Entity<BreweryBeerLink>()
            .HasKey(bb => new { bb.BreweryId, bb.BeerId });
        modelBuilder.Entity<BreweryBeerLink>()
            .HasOne(bb => bb.Brewery)
            .WithMany(b => b.BreweryBeers)
            .HasForeignKey(b => b.BreweryId);
        modelBuilder.Entity<BreweryBeerLink>()
            .HasOne(bb => bb.Beer)
            .WithMany(b => b.BreweryBeers)
            .HasForeignKey(b => b.BeerId);
    }
}