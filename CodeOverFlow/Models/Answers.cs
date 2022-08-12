namespace CodeOverFlow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string AnswerString { get; set; }
        public ApplicationUser User { get; set; }
        public Question Question { get; set; }
        public ICollection<Comment>Comments { get; set; }

        public bool IsCorrect { get; set; } = false;
        public ICollection<Vote>? Votes { get; set; } = new HashSet<Vote>();
    }
}
