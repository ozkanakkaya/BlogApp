using AutoMapper;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Services;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Configurations;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
        private readonly IToastNotification _toastNotification;
        private readonly WebsiteInfo _websiteInfo;
        private readonly IWritableOptions<WebsiteInfo> _websiteInfoWriter;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWritableOptions<SmtpSettings> _smtpSettingsWriter;
        private readonly BlogRightSideBarWidgetOptions _blogRightSideBarWidgetOptions;
        private readonly IWritableOptions<BlogRightSideBarWidgetOptions> _blogRightSideBarWidgetOptionsWriter;
        private readonly CategoryApiService _categoryApiService;
        private readonly IMapper _mapper;

        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter, IToastNotification toastNotification, IOptionsSnapshot<WebsiteInfo> websiteInfo, IWritableOptions<WebsiteInfo> websiteInfoWriter, IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtpSettingsWriter, IOptionsSnapshot<BlogRightSideBarWidgetOptions> blogRightSideBarWidgetOptions, IWritableOptions<BlogRightSideBarWidgetOptions> blogRightSideBarWidgetOptionsWriter, CategoryApiService categoryApiService, IMapper mapper)
        {
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
            _toastNotification = toastNotification;
            _websiteInfo = websiteInfo.Value;
            _websiteInfoWriter = websiteInfoWriter;
            _smtpSettings = smtpSettings.Value;
            _smtpSettingsWriter = smtpSettingsWriter;
            _blogRightSideBarWidgetOptions = blogRightSideBarWidgetOptions.Value;
            _blogRightSideBarWidgetOptionsWriter = blogRightSideBarWidgetOptionsWriter;
            _categoryApiService = categoryApiService;
            _mapper = mapper;
        }

        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpPost]
        public IActionResult About(AboutUsPageInfo aboutUsPageInfo)
        {
            if (ModelState.IsValid)
            {
                _aboutUsPageInfoWriter.Update(x =>
                {
                    x.Title = aboutUsPageInfo.Title;
                    x.Content = aboutUsPageInfo.Content;
                });
                _toastNotification.AddSuccessToastMessage("Hakkımızda sayfa içerikleri başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem!"
                });
                return View(aboutUsPageInfo);
            }
            return View(aboutUsPageInfo);
        }

        public IActionResult GeneralSettings()
        {
            return View(_websiteInfo);
        }

        [HttpPost]
        public IActionResult GeneralSettings(WebsiteInfo websiteInfo)
        {
            if (ModelState.IsValid)
            {
                _websiteInfoWriter.Update(x =>
                {
                    x.Title = websiteInfo.Title;
                    x.MenuTitle = websiteInfo.MenuTitle;
                });
                _toastNotification.AddSuccessToastMessage("Sitenizin genel ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem!"
                });
                return View(websiteInfo);
            }
            return View(websiteInfo);
        }

        public IActionResult EmailSettings()
        {
            return View(_smtpSettings);
        }

        [HttpPost]
        public IActionResult EmailSettings(SmtpSettings smtpSettings)
        {
            if (ModelState.IsValid)
            {
                _smtpSettingsWriter.Update(x =>
                {
                    x.Server = smtpSettings.Server;
                    x.Port = smtpSettings.Port;
                    x.SenderName = smtpSettings.SenderName;
                    x.SenderEmail = smtpSettings.SenderEmail;
                    x.Username = smtpSettings.Username;
                    x.Password = smtpSettings.Password;
                });
                _toastNotification.AddSuccessToastMessage("Sitenizin e-posta ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem!"
                });
                return View(smtpSettings);
            }
            return View(smtpSettings);
        }

        public async Task<IActionResult> BlogRightSideBarWidgetSettings()
        {
            var categoriesResult = await _categoryApiService.GetAllByActiveAsync();
            var blogPostRightSideBarWidgetOptionsViewModel = _mapper.Map<BlogRightSideBarWidgetOptionsViewModel>(_blogRightSideBarWidgetOptions);
            blogPostRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
            return View(blogPostRightSideBarWidgetOptionsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BlogRightSideBarWidgetSettings(BlogRightSideBarWidgetOptionsViewModel blogRightSideBarWidgetOptionsViewModel)
        {
            var categoriesResult = await _categoryApiService.GetAllByActiveAsync();
            blogRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
            if (ModelState.IsValid)
            {
                _blogRightSideBarWidgetOptionsWriter.Update(x =>
                {
                    x.Header = blogRightSideBarWidgetOptionsViewModel.Header;
                    x.TakeSize = blogRightSideBarWidgetOptionsViewModel.TakeSize;
                    x.CategoryId = blogRightSideBarWidgetOptionsViewModel.CategoryId;
                    x.FilterBy = blogRightSideBarWidgetOptionsViewModel.FilterBy;
                    x.OrderBy = blogRightSideBarWidgetOptionsViewModel.OrderBy;
                    x.IsAscending = blogRightSideBarWidgetOptionsViewModel.IsAscending;
                    x.StartAt = blogRightSideBarWidgetOptionsViewModel.StartAt;
                    x.EndAt = blogRightSideBarWidgetOptionsViewModel.EndAt;
                    x.MaxViewCount = blogRightSideBarWidgetOptionsViewModel.MaxViewCount;
                    x.MinViewCount = blogRightSideBarWidgetOptionsViewModel.MinViewCount;
                    x.MaxCommentCount = blogRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                    x.MinCommentCount = blogRightSideBarWidgetOptionsViewModel.MinCommentCount;
                });
                _toastNotification.AddSuccessToastMessage("Blog yazısı sayfalarınızın widget ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem!"
                });
                return View(blogRightSideBarWidgetOptionsViewModel);
            }
            return View(blogRightSideBarWidgetOptionsViewModel);
        }
    }
}
