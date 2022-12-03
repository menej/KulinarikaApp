using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KulinarikaApp.Authorization;
using KulinarikaApp.Authorization.RecipeAuthorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KulinarikaApp.Data;
using KulinarikaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace KulinarikaApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;


        public RecipesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var recipes = _context.Recipes
                .Include(r => r.User)
                .AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.Title.Contains(searchString));
            }

            return View(await recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            // Find the recipe object with given id 
            var recipe = await _context.Recipes
                .Include(r => r.User)
                .Include("Comments.User") // basically Include(x => x.Comments.Select(child => child.User))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,RecipeText")] Recipe recipe)
        {
            if (!ModelState.IsValid)
                return View(recipe);
            //Forbid();

            var currentUser = await _userManager.GetUserAsync(User);
            recipe.UserId = _userManager.GetUserId(User);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, recipe, RecipeOperations.Create);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            recipe.User = currentUser;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            /*
            if (ModelState.IsValid)
            {
                recipe.User = currentUser;
                
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Model is not valid
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
            */
        }

        // GET: Recipes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, recipe, RecipeOperations.Update);

            if (isAuthorized.Succeeded == false)
                return Forbid();


            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,RecipeText")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, recipe, RecipeOperations.Update);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            try
            {
                _context.Update(recipe);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(recipe.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));

            /*
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
            */
        }

        // GET: Recipes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            /*
            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            */
            if (recipe == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, recipe, RecipeOperations.Delete);

            // var isModerator = User.IsInRole(Constants.ModeratorRole);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recipes'  is null.");
            }

            // Authorization process
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, recipe, RecipeOperations.Delete);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            // Deletion process
            var relatedBookmarks = _context.Bookmarks.Where(t => t.RecipeId == recipe.Id);
            var relatedComments = _context.Comments.Where(t => t.RecipeId == recipe.Id);

            /*
            var relatedBookmarks =
                from i in _context.Bookmarks
                where i.RecipeId == recipe.Id
                select i;
            var relatedComments =
                from i in _context.Comments
                where i.RecipeId == recipe.Id
                select i;
*/

            foreach (var bookmark in relatedBookmarks)
            {
                _context.Bookmarks.Remove(bookmark);
            }

            foreach (var comment in relatedComments)
            {
                _context.Comments.Remove(comment);
            }


            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // CREATE COMMENT -> should probably create CommentController
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(string commentText, int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || id == 0)
                return NotFound();

            Comment newComment = new Comment();
            newComment.CommentText = commentText;
            newComment.User = user;
            newComment.Recipe = await _context.Recipes.FirstOrDefaultAsync(i => i.Id == id);

            try
            {
                _context.Update(newComment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("",
                    "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "See your system administrator.");
            }

            return RedirectToAction("Details", newComment.Recipe);
        }

        // DELETE COMMENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comments == null)
                return Problem("Entity set 'Application.Bookmarks' is null");

            var comment = await _context.Comments
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return NotFound();
            
            var recipeId = comment.RecipeId;

            if (recipeId == null)
                return Problem($"Comment with id {comment.Id} exists without recipe?");

            var recipe = await _context.Recipes.FirstOrDefaultAsync(i => i.Id == recipeId);
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), recipe);
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}