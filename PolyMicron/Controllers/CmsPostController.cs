using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pm.Core;
using Pm.Models;
using ProjectPlaguemangler.Extensions;
using ProjectPlaguemangler.Filters;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjectPlaguemangler.Controllers
{
    [Route("cms/posts")]
    [Authorize]
    [ServiceFilter(typeof(ExceptionHandler))]
    public class CmsPostController: BaseController
    {
        private readonly PostService postService;

        public CmsPostController(PostService postService)
        {
            this.postService = postService;
        }

        [HttpGet("")]
        public async Task<IActionResult> List(int? page)
        {
            if (!page.HasValue || page < 1)
            {
                page = 1;
                return RedirectToAction("List", new { page });
            }

            var model = await postService.FetchAll(page.Value);

            return View("List", model);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View("Edit", new PostModel());
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await postService.FetchEditablePost(id);

            return View("Edit", post);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(PostModel model)
        {
            var id = await postService.SavePost(model, HttpContext.User.Id().Value);
            AddBox("Post saved", BoxType.Success);
            return RedirectToAction("Edit", new { id });
        }

        [HttpPost("{id:int}/publish")]
        public async Task<IActionResult> Publish(int id)
        {
            await postService.SetPublished(id, true);
            AddBox("Post published", BoxType.Success);
            return RedirectBack();
        }

        [HttpPost("{id:int}/unpublish")]
        public async Task<IActionResult> Unpublish(int id)
        {
            await postService.SetPublished(id, false);
            AddBox("Post unpublished", BoxType.Success);
            return RedirectBack();
        }

        [HttpPost("{id:int}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await postService.DeletePost(id);
            AddBox("Post deleted", BoxType.Success);
            return RedirectBack();
        }

        [HttpPost("{id:int}/upload-thumbnail")]
        public async Task<IActionResult> UploadThumbnail(int id, IFormFile thumbnailImage)
        {

            if (thumbnailImage == null)
            {
                await postService.SaveThumbnail(id, null, null);
                AddBox("Thumbnail removed", BoxType.Success);
            } 
            else
            {
                using (var memStream = new MemoryStream())
                {
                    await thumbnailImage.CopyToAsync(memStream);
                    await postService.SaveThumbnail(id, memStream.ToArray(), thumbnailImage.ContentType);
                }
                AddBox("Thumbnail added", BoxType.Success);
            }

            return RedirectBack();
        }
    }
}
