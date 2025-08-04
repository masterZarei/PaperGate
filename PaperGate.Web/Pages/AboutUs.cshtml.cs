using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages
{
    public class AboutUsModel : MyPageModel
    {
        private readonly IPublicInfoService _preferencesService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AboutUsModel(IPublicInfoService preferencesService, IMapper mapper, ILogger logger)
        {
            _preferencesService = preferencesService;
            _mapper = mapper;
            _logger = logger;
        }
        public AboutUsPageDto PageDto { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                PageDto = await _preferencesService.GetAboutUsInfoAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "DANGER DANGER DANGER AboutUs FAILED ON OnGet", PageDto);
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                return RedirectToIndex();
            }

        }
    }

}
