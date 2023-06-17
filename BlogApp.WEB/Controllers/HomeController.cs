using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Services;
using BlogApp.WEB.Configurations;
using BlogApp.WEB.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogApiService _blogApiService;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IMailService _mailService;
        private readonly IValidator<EmailSendDto> _emailDtoValidator;

        public HomeController(BlogApiService blogApiService, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IMailService mailService, IValidator<EmailSendDto> emailDtoValidator)
        {
            _blogApiService = blogApiService;
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _mailService = mailService;
            _emailDtoValidator = emailDtoValidator;
        }

        //[Route("index")]
        //[Route("anasayfa")]
        //[Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {
            var blogsResult = await (categoryId == null
                ? _blogApiService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
                : _blogApiService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending));

            return View(blogsResult);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            var resultValidate = _emailDtoValidator.Validate(emailSendDto);

            if (resultValidate.IsValid)
            {

                var result = _mailService.SendContactEmail(emailSendDto);
                if (!result.Errors.Any())
                {

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }
            resultValidate.Errors.ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());

            return Json(new { success = false, errors = errors });
        }
    }
}