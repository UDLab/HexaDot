using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexaDot.Data.ViewModels.Institute;
using HexaDot.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HexaDot.Features.Institutes
{
    public class InstituteListQueryHandler : IAsyncRequestHandler<InstituteListQuery, List<InstituteSummaryViewModel>>
    {
        private HexaDotContext _context;
        public InstituteListQueryHandler(HexaDotContext context)
        {
            _context = context;
        }

        public async Task<List<InstituteSummaryViewModel>> Handle(InstituteListQuery message)
        {
            return await _context.Institutes.Select(t => new InstituteSummaryViewModel
            {
                Id = t.Id,
                LogoUrl = t.LogoUrl,
                Name = t.Name,
                WebUrl = t.WebUrl
            })
            .ToListAsync();
        }
    }
}
