using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeOverFlow.Models.ViewModels
{
    public class TagSelectViewModel
    {
        public List<SelectListItem> TagSelect { get; set; }

        public TagSelectViewModel(List<Tag> Tags)
        {
            TagSelect = new List<SelectListItem>();
            Tags.ForEach(t =>
            {
                TagSelect.Add(new SelectListItem(t.Name, t.Id.ToString()));
            });
        }

    }
}
