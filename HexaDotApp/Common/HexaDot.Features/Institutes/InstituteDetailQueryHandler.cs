using System.Linq;
using System.Threading.Tasks;
//using HexaDot.Extensions;
using HexaDot.Data.ViewModels.Institute;
using HexaDot.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HexaDot.Data.ViewModels.Institute
{
    public class InstituteDetailQueryHandler : IAsyncRequestHandler<InstituteDetailQuery, InstituteDetailViewModel>
    {
        private HexaDotContext _context;

        public InstituteDetailQueryHandler(HexaDotContext context)
        {
            _context = context;
        }

        public async Task<InstituteDetailViewModel> Handle(InstituteDetailQuery message)
        {
            var t = await _context.Institutes
                .AsNoTracking()
                .Where(ten => ten.Id == message.Id)
                .SingleOrDefaultAsync();

            if (t == null)
            {
                return null;
            }

            var institute = new InstituteDetailViewModel
            {
                Id = t.Id,
                Name = t.Name,

                LogoUrl = t.LogoUrl,
                WebUrl = t.WebUrl,

            };

            return institute;
        }

    }
}