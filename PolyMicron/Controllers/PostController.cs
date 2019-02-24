using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pm.Common.Exceptions;
using Pm.Core;
using Pm.Models;
using System.Threading.Tasks;

namespace ProjectPlaguemangler.Controllers
{
    [Route("post")]
    public class PostController: BaseController
    {
        private readonly PostService postService;
        private readonly IConfiguration config;

        public PostController(PostService postService, IConfiguration config)
        {
            this.postService = postService;
            this.config = config;
        }

        [HttpGet("{id:int}/{slug?}")]
        public async Task<IActionResult> Post(int id)
        {
            var uvi = HttpContext.Request.Cookies["uvi"];

            var model = await postService.FetchOnePublicPost(id, uvi);
            model.ReCaptchaKey = config["GoogleReCaptcha:key"];

            return View(model);
        }

        [HttpPost("{id:int}/vote/{dir}")]
        public async Task<IActionResult> Vote(int id, string dir)
        {
            PmAssert.AssertOrValidationError(dir == "up" || dir == "down", "Invalid vote direction");
            var uvi = HttpContext.Request.Cookies["uvi"];
            await postService.Vote(id, dir, uvi);

            return RedirectBack();
        }


        [Route("{id:int}/thumbnail")]
        public async Task<IActionResult> Thumbnail(int id)
        {
            var thumb = await postService.FetchThumbnail(id);

            return File(thumb.Data, thumb.Mime);
        }

        [HttpPost("{id:int}/comment")]
        public async Task<IActionResult> Comment(int id, CommentModel model)
        {
            var recap = ReCaptchaPassed(Request.Form["g-recaptcha-response"], config.GetSection("GoogleReCaptcha:secret").Value);
            if (!recap)
            {
                AddBox("Bad captcha", BoxType.Error, "Captcha");
            }

            if (!ModelState.IsValid)
            {

                if (model.Name == null)
                {
                    AddBox("Provide a name to be distinguished", BoxType.Error, "InputName");
                }
                else if (model.Name.Length > 20)
                {
                    AddBox("Must be up to 20 chars", BoxType.Error, "InputName");
                }

                if (model.Text == null)
                {
                    AddBox("Provide a comment text", BoxType.Error, "InputText");
                } else if (model.Text.Length > 255)
                {
                    AddBox("255 chars max", BoxType.Error, "InputText");
                }

            }
            else
            {
                await postService.AddComment(id, model);
            }

            return RedirectToAction("Post", "Post", new { id });
        }
    }
}
