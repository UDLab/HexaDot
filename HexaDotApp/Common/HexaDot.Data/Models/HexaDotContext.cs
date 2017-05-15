using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HexaDot.Data.Models
{

    public class HexaDotContext : IdentityDbContext<ApplicationUser>
    {
        public HexaDotContext() { }

        public HexaDotContext(DbContextOptions options) : base(options) { }

          
        public virtual DbSet<Institute> Institutes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Keep old database table naming convention
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            Map(modelBuilder.Entity<Institute>());
            //Map(modelBuilder.Entity<ApplicationUser>());
        }

        private void Map(EntityTypeBuilder<Institute> builder)
        {
            //builder.HasOne(t => t.Location);
            //builder.HasMany(t => t.InstituteContacts);
            builder.Property(t => t.Name).IsRequired();
        }

        //private void Map(EntityTypeBuilder<ApplicationUser> builder)
        //{
        //    builder.HasMany(u => u.AssociatedSkills).WithOne(us => us.User);
        //}


    }
}
