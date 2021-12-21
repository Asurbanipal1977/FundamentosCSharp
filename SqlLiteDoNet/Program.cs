using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SqlLiteDoNet
{
    public class Program
    {
        static string database = "dbSqlLite.db";
        async static Task Main(string[] args)
        {
            using (var db = new DataBaseContext())
            {
                //Asegurarte que se ha crado la base de datos
                await db.Database.EnsureCreatedAsync();
                Post post1 = new Post() { Id = 1, UserId=1, Title="Prueba 1", Body="Prueba 1"};
                Post post2 = new Post() { Id = 2, UserId = 2, Title = "Prueba 2", Body = "Prueba 2" };

                db.Add(post1);
                db.Add(post2); 
                await db.SaveChangesAsync();
            }
        }

        public class DataBaseContext : DbContext
        {
            public DbSet<Post> Posts { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(connectionString: "Filename=" + database, options =>
                {
                    //Nombre de nuestro ensamblado que se genera
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
                base.OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Post>().ToTable("Posts");

                //Para añadir la clave primaria
                modelBuilder.Entity<Post>(entity =>
                {
                    entity.HasKey(e => e.Id);
                });

                base.OnModelCreating(modelBuilder);
            }
        }
    }
    
}
