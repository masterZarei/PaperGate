using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities.Template;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Infra.Data;
using ILogger = Serilog.ILogger;

namespace PaperGate.Infra.Implementations.Service;
public class PublicInfoService : IPublicInfoService
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public PublicInfoService(AppDbContext context, ILogger logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<AboutUsPageDto> GetAboutUsInfoAsync()
    {
        try
        {
            var info = await _context.AboutUs
                .OrderByDescending(a => a.CreatedOn)
                .FirstOrDefaultAsync();
            if (info is null)
            {
                await _context.AboutUs.AddAsync(new AboutUsInfo
                {
                    Image = string.Empty,
                    Description = string.Empty
                });
                await _context.SaveChangesAsync();
                info = await _context.AboutUs
                    .OrderByDescending(a => a.CreatedOn)
                    .FirstOrDefaultAsync();
            }
            AboutUsPageDto dto = _mapper.Map<AboutUsPageDto>(info);
            dto.ContactWays = await _context.ContactWays.ToListAsync();

            return dto;
        }
        catch (Exception ex)
        {

            _logger.Fatal(ex, $"AboutUs {nameof(GetAboutUsInfoAsync)} has been failed");
            return null;
        }
    }
}
