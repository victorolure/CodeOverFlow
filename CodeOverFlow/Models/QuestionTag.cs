using System.ComponentModel.DataAnnotations;

namespace CodeOverFlow.Models
{
    public class QuestionTag
    {
        [Key]
        public int Id { get; set; }
        public Question? Question { get; set; }
        public Tag? Tag { get; set; }
    }
}
