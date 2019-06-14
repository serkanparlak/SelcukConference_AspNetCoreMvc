using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    public class DbtechconfContext : DbContext
    {
        public virtual DbSet<Bildiri> Bildiri { get; set; }
        public virtual DbSet<Ek> Ek { get; set; }
        public virtual DbSet<Genel> Genel { get; set; }
        public virtual DbSet<HakemBildiriAtama> HakemBildiriAtama { get; set; }
        public virtual DbSet<KonuEtiketi> KonuEtiketi { get; set; }
        public virtual DbSet<PaperNotify> PaperNotify { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Mesaj> Mesaj { get; set; }
        public virtual DbSet<Sehir> Sehir { get; set; }
        public virtual DbSet<Ulke> Ulke { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DbTechConf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bildiri>(entity =>
            {
                entity.HasOne(d => d.Yazar)
                    .WithMany(p => p.Bildiri)
                    .HasForeignKey(d => d.YazarId)
                    .HasConstraintName("FK_Bildiri_Uye");
            });

            modelBuilder.Entity<Ek>(entity =>
            {
                entity.HasOne(d => d.Bildiri)
                    .WithMany(p => p.Ek)
                    .HasForeignKey(d => d.BildiriId)
                    .HasConstraintName("FK_Ek_Bildiri");
            });

            modelBuilder.Entity<HakemBildiriAtama>(entity =>
            {
                entity.HasOne(d => d.Bildiri)
                    .WithMany(p => p.HakemBildiriAtama)
                    .HasForeignKey(d => d.BildiriId)
                    .HasConstraintName("FK_HakemBildiriAtama_Bildiri");
            });

            modelBuilder.Entity<KonuEtiketi>(entity =>
            {
                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.KonuEtiketi)
                    .HasForeignKey(d => d.EditorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KonuEtiketi_Uye");
            });

            modelBuilder.Entity<Mesaj>(entity =>
            {
                entity.HasOne(d => d.Alici)
                    .WithMany(p => p.MesajAlici)
                    .HasForeignKey(d => d.AliciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mesaj_Uye1");

                entity.HasOne(d => d.Gonderen)
                    .WithMany(p => p.MesajGonderen)
                    .HasForeignKey(d => d.GonderenId)
                    .HasConstraintName("FK_Mesaj_Uye");
            });

            modelBuilder.Entity<Sehir>(entity =>
            {
                entity.HasOne(d => d.Ulke)
                    .WithMany(p => p.Sehir)
                    .HasForeignKey(d => d.UlkeId)
                    .HasConstraintName("FK_geo_cities_geo_countries");
            });

            modelBuilder.Entity<Uye>(entity =>
            {
                entity.HasOne(d => d.Sehir)
                    .WithMany(p => p.Uye)
                    .HasForeignKey(d => d.SehirId)
                    .HasConstraintName("FK_Uye_Sehir");
            });
        }
    }
}
