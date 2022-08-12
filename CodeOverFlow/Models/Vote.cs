namespace CodeOverFlow.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public ApplicationUser? User { get; set; }

        public Question? Question { get; set; }
        public Answer? Answer { get; set; }

        public bool? IsVote { get; set; }
    }
}
