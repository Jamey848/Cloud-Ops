
using Microsoft.EntityFrameworkCore;

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

                // Replace with your actual Supabase table name.
                entity.ToTable("games");
            });
        }
    }
}
