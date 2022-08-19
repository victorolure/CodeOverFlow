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
using Microsoft.AspNetCore.Authorization;

namespace CodeOverFlow.Controllers
{
    [Authorize]
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public VotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CastNewVoteQuestion(Vote newVote, Question question, ApplicationUser user, bool voteValue) {

            newVote.Question = question;
            newVote.Answer = null;
            newVote.User = user;
            newVote.IsVote = voteValue;
            _context.Votes.Add(newVote);
            question.Votes.Add(newVote);
            if (newVote.IsVote == true)
            {
                question.User.Reputation += 5;
            }
            else
            {
                question.User.Reputation -= 5;
            }
            await _context.SaveChangesAsync();
            return Redirect($"https://localhost:7278/Questions/Details?questionId={question.Id}");
        }

        public async Task<IActionResult> CastNewVoteAnswer(Vote newVote, Answer answer, ApplicationUser user, bool voteValue)
        {

            newVote.Answer = answer;
            newVote.Question = null;
            newVote.User = user;
            newVote.IsVote = voteValue;
            _context.Votes.Add(newVote);
            answer.Votes.Add(newVote);
            if (newVote.IsVote == true)
            {
                answer.User.Reputation += 5;
            }
            else
            {
                answer.User.Reputation -= 5;
            }
            await _context.SaveChangesAsync();
            return Redirect($"https://localhost:7278/Questions/Details?questionId={answer.Question.Id}");
        }
        public async Task<IActionResult>ToggleExistingVote(int? questionId, int?answerId, ApplicationUser user, bool voteValue)
        {
            if(questionId!= null && answerId== null)
            {
                Question question = await _context.Question.Include(q => q.User).Include(q => q.Answers).Include(q => q.Comments).Include(q => q.Votes).FirstOrDefaultAsync(q => q.Id == questionId);
                Vote vote = await _context.Votes.Include(v => v.User).Include(v => v.Question).ThenInclude(q => q.Comments).Include(v => v.Answer).ThenInclude(a => a.Comments).FirstAsync(v => v.Question.Id == question.Id && v.User.Id == user.Id);
                if (vote.IsVote == true)
                {
                    if (voteValue == true)
                    {
                        question.User.Reputation -= 5;
                        _context.Votes.Remove(vote);
                        question.Votes.Remove(vote);

                    }
                    else
                    {
                        question.User.Reputation -= 5;
                        vote.IsVote = voteValue;
                        question.Votes.First(v => v == vote).IsVote = voteValue;
                    }
                }
                else
                {
                    if (voteValue == true)
                    {
                        question.User.Reputation += 5;
                        vote.IsVote = voteValue;
                    }
                    else
                    {
                        question.User.Reputation += 5;
                        _context.Votes.Remove(vote);
                    }
                }
                //question.Votes.Remove(vote);
                await _context.SaveChangesAsync();
                return Redirect($"https://localhost:7278/Questions/Details?questionId={question.Id}");
            }
            else
            {

                Answer answer = await _context.Answer.Include(a => a.User).Include(a => a.Question).ThenInclude(q => q.Comments).Include(a => a.Comments).FirstAsync(a => a.Id == answerId);
                Vote vote = await _context.Votes.Include(v => v.User).Include(v => v.Question).ThenInclude(q => q.Comments).Include(v => v.Answer).ThenInclude(a => a.Comments).FirstAsync(a => a.Answer.Id == answer.Id && a.User.Id == user.Id);
                //_context.Votes.Remove(vote);
                if (vote.IsVote == true)
                {
                    if (voteValue == true)
                    {
                        answer.User.Reputation -= 5;
                        _context.Votes.Remove(vote);
                    }
                    else
                    {
                        answer.User.Reputation -= 5;
                        vote.IsVote = false;
                    }

                }
                else
                {
                    if (voteValue == false)
                    {
                        answer.User.Reputation += 5;
                        _context.Votes.Remove(vote);
                    }
                    else
                    {
                        answer.User.Reputation += 5;
                        vote.IsVote = voteValue;
                        answer.Votes.First(v => v.Id == vote.Id).IsVote = voteValue;
                    }
                }
                //answer.Votes.Remove(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Questions", new { questionId = answer.Question.Id });

            }

        }


        [HttpPost]
        public async Task<IActionResult> CastVote(int? questionId, int? answerId, bool voteValue)
        {
            String userName = User.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            Vote newVote = new Vote();
            if(questionId!= null && answerId == null)
            {
                Question question = await _context.Question.Include(q => q.User).Include(q => q.Answers).Include(q => q.Comments).Include(q => q.Votes).FirstOrDefaultAsync(q => q.Id == questionId);
                if(question.User.Id != user.Id)
                {
                    if(!_context.Votes.Any(v=> v.Question == question && v.User == user))
                    {
                        await CastNewVoteQuestion(newVote, question, user, voteValue);
                    }
                    else
                    {
                        await ToggleExistingVote(questionId, answerId, user, voteValue);
                        
                    }
                }
                ViewBag.Message = "Sorry, you can't react to your own question";
                return Redirect($"https://localhost:7278/Questions/Details?questionId={question.Id}");
            }
            else if(answerId!= null && questionId== null)
            {
                Answer answer = await _context.Answer.Include(a => a.User).Include(a => a.Question).ThenInclude(q => q.Comments).Include(a => a.Comments).FirstAsync(a => a.Id == answerId);               
                if (answer.User.Id != user.Id)
                {
                    if (!_context.Votes.Any(v => v.Answer.Id == answer.Id && v.User.Id == user.Id))
                    {
                        await CastNewVoteAnswer(newVote, answer, user, voteValue);
                    }
                    else
                    {
                        await ToggleExistingVote(questionId, answerId, user, voteValue);                    
                    }
                }
                ViewBag.Message = "Sorry, you can't react to your own answer";
                return Redirect($"https://localhost:7278/Questions/Details?questionId={answer.Question.Id}");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }       
    }   
}
