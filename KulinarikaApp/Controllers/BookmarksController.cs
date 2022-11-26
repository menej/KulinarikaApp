using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KulinarikaApp.Data;
using KulinarikaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KulinarikaApp.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookmarksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Bookmark
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var currentUserId = _userManager.GetUserId(User);
            //var bookmarks = _context.Bookmarks.Include(i => i.UserId == currentUserId);
            var bookmarks =
                from i in _context.Bookmarks
                where i.UserId == currentUserId
                select i;

            return View(await bookmarks.ToListAsync());
        }


        // GET: Bookmark/Create
        public async Task<IActionResult> Create(int? id)
        {
            //var currentUser = _userManager.GetUserAsync(User);
            var recipe = await _context.Recipes.FindAsync(id);
            var currentUserId = _userManager.GetUserId(User);
            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null || recipe == null)
                return NotFound();
            
            // Ustvarimo nov objekt Bookmark
            Bookmark bookmark = new Bookmark();
            bookmark.User = user;
            bookmark.Recipe = recipe;
            bookmark.BookmarkName = recipe.Title;

            // Preverjanje za duplikate
            var userBookmarks = 
                from i in _context.Bookmarks 
                where i.UserId == user.Id
                select i;
            foreach (var booky in userBookmarks)
            {
                if (booky.Recipe != null && booky.Recipe.Id == recipe.Id)
                    return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Update(bookmark);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
            
        }

        
        // GET: Bookmark/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (_context.Bookmarks == null)
                return Problem("Entity set 'Application.Bookmarks' is null.");

            var bookmark = await _context.Bookmarks.FindAsync(id);

            if (bookmark == null)
                return NotFound();

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}