namespace CodeOverFlow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string CommentString { get; set; }
        public ApplicationUser User { get; set; }

        public Question? Question { get; set; }
        public Answer? Answer { get; set; }
    }
}
