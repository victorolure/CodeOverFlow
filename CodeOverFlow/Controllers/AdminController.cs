using CodeOverFlow.Data;
using CodeOverFlow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeOverFlow.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task<ActionResult> Index()
        {
            List<Question> allQuestions = _context.Question.ToList();
            return View(allQuestions);
        }

        public async Task<ActionResult> DeleteQuestion(int? questionId)
        {
            Question question = _context.Question.First(q => q.Id == questionId);
            _context.Question.Remove(question);
            List<Answer> answers = _context.Answer.Where(a => a.Question == question).ToList();
            answers.ForEach(a =>
            {
                _context.Answer.Remove(a);
            });
            List<Comment> comments = _context.Comment.Where(c => c.Question == question || c.Answer.Question == question).ToList();
            comments.ForEach(c =>
            {
                _context.Comment.Remove(c);
            });
            List<Vote> votes = _context.Votes.Where(v=> v.Question == question || v.Answer.Question == question ).ToList();
            votes.ForEach(v =>
            {
                _context.Votes.Remove(v);
            });
            List<QuestionTag> questionTags = _context.QuestionTags.Where(q => q.Question == question).ToList();
            questionTags.ForEach(q =>
            {
                _context.QuestionTags.Remove(q);
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
