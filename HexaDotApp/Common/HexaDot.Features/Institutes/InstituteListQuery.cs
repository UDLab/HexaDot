using System.Collections.Generic;
using HexaDot.Data.ViewModels.Institute;
using MediatR;

namespace HexaDot.Features.Institutes
{
    public class InstituteListQuery : IAsyncRequest<List<InstituteSummaryViewModel>>
    {
    }
}
