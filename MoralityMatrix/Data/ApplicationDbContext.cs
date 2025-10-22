using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MoralityMatrix.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Option { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Question>().ToTable("Questions");
            builder.Entity<Option>().ToTable("Options");
        }
    }

    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public virtual ICollection<Option>? Options { get; set; }
        public int? CorrectOptionIndex { get; set; }
        public int? Chapter { get; set; }
    }

    public class Option
    {
        [Key]
        public int Id { get; set; }
        public string? Value { get; set; }
        public int? Index { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question? Question { get; set; }
    }
}