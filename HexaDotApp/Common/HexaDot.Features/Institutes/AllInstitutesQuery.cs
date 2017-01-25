using System.Collections.Generic;
using HexaDot.Data.Models;
using MediatR;

namespace HexaDot.Features.Institutes
{   
    public class AllInstitutesQuery : IAsyncRequest<IEnumerable<Institute>>
    {
    }
   
}
