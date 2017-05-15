using System.Collections.Generic;
using HexaDot.Data.Models;
using MediatR;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HexaDot.Features.Institutes
{
    public class AllInstitutesQueryHandler
    {
        private readonly HexaDotContext _context;

        public AllInstitutesQueryHandler(HexaDotContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Institute>> Handle(AllInstitutesQuery message)
        {
            return await _context.Institutes.ToListAsync();
        }
    }
}
