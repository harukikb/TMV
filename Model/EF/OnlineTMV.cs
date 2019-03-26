namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OnlineTMV : DbContext
    {
        public OnlineTMV()
            : base("name=OnlineTMV2")
        {
        }

        public virtual DbSet<HT_NHANVIEN> HT_NHANVIEN { get; set; }
        public virtual DbSet<Introduce> Introduces { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
