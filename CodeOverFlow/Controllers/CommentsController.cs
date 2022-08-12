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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Comment(int?questionId, int?answerId)
        {
            Question question;
            Answer answer;
            ViewBag.Message = null;
            if(questionId != null && answerId== null)
            {
                question = await _context.Question.Include(q => q.Answers).Include(q=> q.Comments).FirstAsync(q => q.Id == questionId);
                ViewBag.Message = question;
                ViewBag.Message2 = question.Id;
                ViewBag.Message5 = question.QuestionString;
                return View();                
            }else if(answerId != null && questionId== null)
            {
                answer = await _context.Answer.Include(a=> a.Comments).Include(a=> a.Question).FirstAsync(a=> a.Id == answerId);
                ViewBag.Message = answer;
                ViewBag.Message3 = answer.Id;
                ViewBag.Message5 = answer.Question.QuestionString;
                return View();
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Comment(string comment,int? questionId, int? answerId)
        { 
            string userName = User.Identity.Name;
            Comment newComment = new Comment();
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            
            if (questionId != null && answerId== null)
            {
                Question question = await _context.Question.Include(q => q.Comments).FirstAsync(q => q.Id == questionId);
                newComment.Question = question;
                newComment.CommentString = comment;
                newComment.User = user;
                newComment.Answer = null;
                user.Comments.Add(newComment);
                question.Comments.Add(newComment);
                _context.Comment.Add(newComment);
                _context.SaveChanges();
                question = await _context.Question.Include(q => q.Answers).Include(q => q.Comments).FirstAsync(q => q.Id == questionId);
                ViewBag.Message = question;
                ViewBag.Message2 = question.Id;
                ViewBag.Message5 = question.QuestionString;
                return View();
            }else if(answerId!= null && questionId == null)
            {
                Answer answer = await _context.Answer.Include(a => a.Comments).FirstAsync(a => a.Id == answerId);
                newComment.Answer = answer;
                newComment.Question = null;
                newComment.User = user;
                newComment.CommentString = comment;
                answer.Comments.Add(newComment);
                user.Comments.Add(newComment);
                _context.Comment.Add(newComment);
                _context.SaveChanges();
                answer = await _context.Answer.Include(a => a.Comments).Include(a => a.Question).FirstAsync(a => a.Id == answerId);
                ViewBag.Message = answer;
                ViewBag.Message3 = answer.Id;
                ViewBag.Message5 = answer.Question.QuestionString;
                return View();
                
                //return RedirectToAction("Index", "Questions");
            }
            return RedirectToAction("Index", "Questions");
           
        }


        // GET: Comments
        public async Task<IActionResult> Index()
        {
              return _context.Comment != null ? 
                          View(await _context.Comment.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Comment'  is null.");
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,CommentString")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,CommentString")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
