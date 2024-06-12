using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;
using Microsoft.EntityFrameworkCore;


namespace Backend.Entity.DatabaseContext
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<BackendTask> BackendTasks { set; get; }
        public virtual DbSet<UserToken> UserTokens { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            //builder.Entity<Company>().HasOne(bc => new { bc.Country }).WithMany(x => x.Country.Companies).HasForeignKey(p => p.CountryId);
            //builder.Entity<BookCategory>().HasKey(bc => new { bc.BookId, bc.CategoryId });

        }
    }
}

