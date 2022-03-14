using Microsoft.EntityFrameworkCore;
using ProjetoSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoSQL.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        //responsavel para criar o banco
        public DbSet<CursosModel> Cursos { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursosModel>(e=>
            {
                e.ToTable("Cursos");
                e.HasKey(p => p.Id);
                e.Property(p => p.Titulo).HasColumnType("varchar(40)").IsRequired();
                e.Property(p => p.duracao).HasColumnType("varchar(40)").IsRequired();
                e.Property(p => p.Status).HasColumnType("varchar(40)").IsRequired();

            });


        }

    }
}
