using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NanoSurvey.Data.Entity;

namespace NanoSurvey.Data
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> ops) : base(ops)
        { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>().HasIndex(entity => entity.Title).IsUnique(true);

            modelBuilder.Entity<Question>().HasIndex(entity => new { entity.IdSurvey, entity.Text }).IsUnique(true);

            modelBuilder.Entity<Answer>().HasIndex(entity => new { entity.IdQuestion, entity.Text }).IsUnique(true);

            modelBuilder.Entity<Result>().HasKey(entity => new { entity.IdInterview, entity.IdQuestion, entity.IdAnswer });
        }
    }
}