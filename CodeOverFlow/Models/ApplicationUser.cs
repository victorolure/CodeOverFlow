using Microsoft.AspNetCore.Identity;

namespace CodeOverFlow.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Question>? Questions { get; set; } = new HashSet<Question>();
        public ICollection<Answer>? Answers { get; set; } = new HashSet<Answer>();
        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();

        public int Reputation { get; set; } = 0;

        public ApplicationUser() : base()
        {
            
        }

    }
}
