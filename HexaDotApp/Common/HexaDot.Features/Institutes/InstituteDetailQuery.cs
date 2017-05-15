using HexaDot.Data.ViewModels.Institute;
using MediatR;



namespace HexaDot.Data.ViewModels.Institute
{
    public class InstituteDetailQuery : IAsyncRequest<InstituteDetailViewModel>
    {
        public int Id { get; set; }
   
    }
}
