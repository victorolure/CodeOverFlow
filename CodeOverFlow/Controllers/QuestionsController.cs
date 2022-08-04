using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeOverFlow.Data;
using CodeOverFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CodeOverFlow.Models.ViewModels;
using PagedList;

namespace CodeOverFlow.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }
        public IActionResult AskQuestion()
        {
            TagSelectViewModel tm = new TagSelectViewModel(_context.Tags.ToList());
            ViewBag.Message = tm.TagSelect;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AskQuestion(string? title, string? question, List<string>tagIDs)
        {
            List<Tag> Tags = new List<Tag>();
            foreach(string tag in tagIDs)
            {
                Tags.Add(await _context.Tags.FirstAsync(t => t.Id == Int32.Parse(tag)));
            }
            string userName = User.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            Question newQuestion = new Question();
            if (title!=null && question != null)
            {
                newQuestion.Title = title;
                newQuestion.QuestionString = question;
                newQuestion.User = user;
                Tags.ForEach(t=> newQuestion.Tags.Add(t));
                user.Questions.Add(newQuestion);
                foreach(Tag tag in Tags)
                {
                    QuestionTag questionTag = new QuestionTag();
                    questionTag.Tag = tag;
                    questionTag.Question = newQuestion;
                    _context.QuestionTags.Add(questionTag);
                }
                _context.Question.Add(newQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
            
        }
        

        // GET: Questions
        public async Task<IActionResult> Index(string?sortBy, int? page)
        {
            
            int pageNumber = page ?? 1;
            IQueryable<Question> questions =  _context.Question.Include(q=> q.Answers);
            if (sortBy == null)
            {
                questions = questions.OrderBy(q=>q.Id);
                return View(questions.ToList().ToPagedList(pageNumber, 3));
            }else if(sortBy == "sortByDate")
            {
                questions = questions.OrderBy(q => q.DateCreated);
                return View(questions.ToList().ToPagedList(pageNumber, 3));
            }
            else if(sortBy == "sortByAnswers")
            {
                questions = questions.OrderByDescending(q => q.Answers.Count);
                return View(questions.ToList().ToPagedList(pageNumber, 3));
            }
     
            return View(questions.ToList().ToPagedList(pageNumber, 3));
                          
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,QuestionString,DateCreated")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,QuestionString,DateCreated")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Question == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Question'  is null.");
            }
            var question = await _context.Question.FindAsync(id);
            if (question != null)
            {
                _context.Question.Remove(question);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
          return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
