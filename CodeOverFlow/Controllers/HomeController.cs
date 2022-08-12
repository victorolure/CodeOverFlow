using CodeOverFlow.Data;
using CodeOverFlow.Models;
using CodeOverFlow.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeOverFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> AddRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole()
        {
            UserAndRoleSelectViewModel vm = new UserAndRoleSelectViewModel(_context.Users.ToList(), _context.Roles.ToList());
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            bool isInRole = await  _userManager.IsInRoleAsync(user, role.Name);
            

            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
                await _context.SaveChangesAsync();
            }
            UserAndRoleSelectViewModel vm = new UserAndRoleSelectViewModel(_context.Users.ToList(), _context.Roles.ToList());
            vm.Message = $"Added {user.UserName} to role {role.Name}";
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string? roleName)
        {
            ViewBag.Message = $"{roleName.ToUpper()} Role added";
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            await _context.SaveChangesAsync();
            return View();
        }
        public async Task <IActionResult> Index()
        {           
            return View();
        }

        public async Task<IActionResult> ViewQuestions()
        {
            List<Question> questions = _context.Question.ToList();
            return View(questions);
        }

        public async Task<IActionResult> ShowAnswers(int? questionId)
        {
            Question question = _context.Question.Include(q=> q.Comments).Include(q=> q.Votes).Include(q=> q.Answers).ThenInclude(a=> a.Comments).Include(q=> q.Answers).ThenInclude(a=> a.Votes).First(q=> q.Id == questionId);
            return View(question.Answers);
        }

        public async Task<IActionResult> ShowComments(int? answerId, int? questionId)
        {
            List<Comment> comments;
            if (answerId != null && questionId == null)
            {
                comments = _context.Answer.Include(a => a.Comments).First(a => a.Id == answerId).Comments.ToList();
            }
            else
            {
                comments = _context.Question.Include(q => q.Comments).First(q => q.Id == questionId).Comments.ToList();
            }
            return View(comments);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}