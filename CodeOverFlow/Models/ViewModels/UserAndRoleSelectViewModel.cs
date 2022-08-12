using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeOverFlow.Models.ViewModels
{
    public class UserAndRoleSelectViewModel
    {
        public ICollection<SelectListItem> UserSelect { get; set; }

        public ICollection<SelectListItem> RoleSelect { get; set; }

        public string? Message { get; set; }

        public UserAndRoleSelectViewModel(ICollection<ApplicationUser> users, ICollection<IdentityRole> roles)
        {
            UserSelect = new HashSet<SelectListItem>();
            RoleSelect = new HashSet<SelectListItem>();
            
            foreach(ApplicationUser user in users)
            {
                UserSelect.Add(new SelectListItem(user.UserName, user.Id));
            }

            foreach(IdentityRole role in roles)
            {
                RoleSelect.Add(new SelectListItem(role.Name, role.Id));
            }
            
        }
    }
}
