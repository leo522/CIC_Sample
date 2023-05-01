using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CIC.Models
{
    public partial class PointModel : DbContext
    {
        public PointModel()
            : base("name=PointModel")
        {
        }

        public virtual DbSet<CICBOOK> CICBOOK { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CICBOOK>()
                .Property(e => e.NAME_ZH)
                .IsUnicode(false);

            modelBuilder.Entity<CICBOOK>()
                .Property(e => e.RECEIPT_LIST)
                .IsUnicode(false);

            modelBuilder.Entity<CICBOOK>()
                .Property(e => e.NO_LIST)
                .IsUnicode(false);

            modelBuilder.Entity<CICBOOK>()
                .Property(e => e.DEPT)
                .IsUnicode(false);
        }
    }
}
