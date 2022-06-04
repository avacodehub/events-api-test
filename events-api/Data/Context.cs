using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using events_api.Data;

namespace events_api.Data
{

    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Place> Place { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<Photo> Photo { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //optionsBuilder.UseSqlServer("Data Source=LAPTOP-0ITIO8EU\\BULKASQL;Initial Catalog=IT_Company2;Integrated Security=True");
        //    }
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost:5433;Database=eventsdb;Username=postgres;Password=admin");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasKey(e => e.Id);



                entity.HasIndex(e => e.Name, "UQ_Name")
                    .IsUnique();

            });

            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.Id);


                entity.HasIndex(e => e.Name, "UQ_Name")
                    .IsUnique();


                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.Qualifications)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Student>(entity =>
            {
               entity.HasIndex(u => u.Email).IsUnique();//

                entity.HasKey(e => e.Id);
                
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);///?

            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id);


                entity.HasIndex(e => e.Name, "UQ_Name")
                    .IsUnique();


                entity.HasOne(d => d.Qualification)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.QualificationId)
                    .OnDelete(DeleteBehavior.Cascade);///

            });
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);



                entity.HasIndex(e => e.Name, "UQ_Name")
                    .IsUnique();

            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);


                


                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade);/////

            });
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasMany(e => e.Students)
                    .WithMany(s => s.Events);

                entity
                    .HasMany(e => e.Groups)
                    .WithMany(s => s.Events);

                entity
                    .HasMany(e => e.Employees)
                    .WithMany(s => s.Events);

              

                //.UsingEntity(j => j.ToTable("PostTags"));

            });
            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientCascade); ///?

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientCascade);

            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientCascade); ///?


            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientCascade); 


            });



        }

       

       
    }
}
