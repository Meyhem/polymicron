using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pm.Core;
using ProjectPlaguemangler.Filters;

namespace ProjectPlaguemangler.Controllers
{
    [ServiceFilter(typeof(ExceptionHandler))]
    public class HomeController : BaseController
    {
        private readonly PostService postService;

        public HomeController(PostService postService)
        {
            this.postService = postService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? page, string search)
        {
            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }

            var model = await postService.FetchPublicPosts(page.Value, search);

            return View(model);
        }
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/not-found")]
        public IActionResult NotFound404()
        {
            ViewBag.Path = HttpContext.Items["originalPath"];

            return View("NotFound");
        }
    }
}
