using CodeOverFlow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeOverFlow.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CodeOverFlow.Models.Question>? Question { get; set; }
        public DbSet<CodeOverFlow.Models.Answer>? Answer { get; set; }
        public DbSet<CodeOverFlow.Models.Comment>? Comment { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }

        public DbSet<Vote> Votes { get; set; }

        
    }
}