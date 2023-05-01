using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CIC.Models
{
    public partial class PointBookModel : DbContext
    {
        public PointBookModel()
            : base("name=PointBookModel")
        {
        }

        public virtual DbSet<PointBook> PointBook { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
