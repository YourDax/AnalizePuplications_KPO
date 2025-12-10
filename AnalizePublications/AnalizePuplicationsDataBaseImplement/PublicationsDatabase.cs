using AnalyzePublicationsDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AnalyzePublicationsDatabaseImplement
{
    public class PublicationsDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    @"Host=localhost;Port=5432;Database=AnalyzePublications;Username=postgres;Password=postgres");
            }

            base.OnConfiguring(optionsBuilder);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<PublicationAuthor> PublicationAuthors { get; set; }
    }
}
