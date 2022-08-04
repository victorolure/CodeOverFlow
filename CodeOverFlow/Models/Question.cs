namespace CodeOverFlow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string QuestionString { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }
        public ICollection<Answer>? Answers { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Tag>? Tags { get; set; } = new HashSet<Tag>();
    }
}
