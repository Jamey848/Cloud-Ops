
using Microsoft.EntityFrameworkCore;
using Recommendation_engine;

namespace Recommendations_engine
{
    public class AppDbContext : DbContext // => DbContext: database manager. Allows CRUD operations 
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        { // = Options are a set of instructions telling the database manager how to opperate.
        } // Stuff like which database to provide, where the database is located, other database configurations.
          // Then the options are passed to the parent: DBContext
        public DbSet<game> Games => Set<game>(); // "A DbSet represents a collection of game records that EF Core can query and manipulate."

        protected override void OnModelCreating(ModelBuilder modelBuilder) /*
                                                                                protected means the method is available to this class and certain derived classes.
                                                                                override means you're replacing the inherited version of this method with your own implementation.
                                                                                OnModelCreating is the method EF Core uses to configure its model.
                                                                                ModelBuilder modelBuilder is the tool you use to configure that model.
                                                                            */
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<game>(entity =>
            {
                entity.HasKey(g => g.Appid);
                entity.ToTable("games");

                // Popularity (1:1)
                entity.HasOne(g => g.Popularity)
                    .WithOne(p => p.game)
                    .HasForeignKey<Popularity>(p => p.AppId);

                // Price (1:1)
                entity.HasOne(g => g.Price)
                    .WithOne(p => p.game)
                    .HasForeignKey<Price>(p => p.AppId);

                // Screenshots (1:N)
                entity.HasMany(g => g.Screenshots)
                    .WithOne(s => s.game)
                    .HasForeignKey(s => s.AppId);

                // Game -> GameGenres
                entity.HasMany(g => g.GameGenres)
                    .WithOne(gg => gg.game)
                    .HasForeignKey(gg => gg.AppId);

                // Game -> GameTags
                entity.HasMany(g => g.GameTags)
                    .WithOne(gt => gt.game)
                    .HasForeignKey(gt => gt.AppId);

                // Game -> GameCategories
                entity.HasMany(g => g.GameCategories)
                    .WithOne(gc => gc.game)
                    .HasForeignKey(gc => gc.AppId);
            });

            // GameGenre -> Genre
            modelBuilder.Entity<GameGenre>()
                .HasOne(x => x.Linked_genre)
                .WithMany()
                .HasForeignKey(x => x.GenreId)
                .HasPrincipalKey(x => x.GenreId);

            // GameTag -> Tag
            modelBuilder.Entity<GameTag>()
                .HasOne(x => x.Linked_tag)
                .WithMany()
                .HasForeignKey(x => x.TagId)
                .HasPrincipalKey(x => x.TagId);

            // GameCategory -> Category
            modelBuilder.Entity<GameCategory>()
                .HasOne(x => x.Linked_category)
                .WithMany()
                .HasForeignKey(x => x.CategorieId)
                .HasPrincipalKey(x => x.CategorieId);
        }
    }
}
