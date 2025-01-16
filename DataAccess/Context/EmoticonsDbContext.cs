using System;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class EmoticonsDbContext : DbContext
{
    public DbSet<EmoticonModel> Emoticons { get; set; }
    public DbSet<TagModel> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmoticonModel>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Emoticons);
    
    }


}
