﻿using Microsoft.EntityFrameworkCore;
using ShopGatheringRestrictionsApi.Models;

namespace ShopGatheringRestrictionsApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Enter> Enters { get; set; }
        public DbSet<Exit> Exits { get; set; }
    }
}
