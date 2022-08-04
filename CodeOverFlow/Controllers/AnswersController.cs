using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeOverFlow.Data;
using CodeOverFlow.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeOverFlow.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        public IActionResult Answer(int? questionId)
        {
            Question question = _context.Question.Include(q=> q.Answers).Include(q=> q.Tags).First(q => q.Id == questionId);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(string? answer, int?questionId)
        {
            string userName = User.Identity.Name;
            Question question = await _context.Question.Include(q=> q.Answers).Include(q=> q.Tags).FirstAsync(q => q.Id == questionId);
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            Answer newAnswer = new Answer();
            if (answer != null)
            {
                newAnswer.AnswerString = answer;
                newAnswer.User = user;
                newAnswer.Question = question;
                question.Answers.Add(newAnswer);
                user.Answers.Add(newAnswer);
                _context.Answer.Add(newAnswer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Questions");
            }
            return RedirectToAction("Index");

        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
              return _context.Answer != null ? 
                          View(await _context.Answer.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Answer'  is null.");
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,AnswerString")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,AnswerString")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Answer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Answer'  is null.");
            }
            var answer = await _context.Answer.FindAsync(id);
            if (answer != null)
            {
                _context.Answer.Remove(answer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
          return (_context.Answer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
