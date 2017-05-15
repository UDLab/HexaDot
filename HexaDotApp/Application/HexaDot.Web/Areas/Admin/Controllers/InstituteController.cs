using System.Linq;
using HexaDot.Features.Institutes;
using MediatR;
using System.Threading.Tasks;
using HexaDot.Data.ViewModels.Institute;
using HexaDot.Data.ViewModels.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaDot.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("SystemAdmin")]
    public class InstituteController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IInstituteEditModelValidator _instituteValidator;

        public InstituteController(IMediator mediator, IInstituteEditModelValidator validator)
        {
            _mediator = mediator;
            _instituteValidator = validator;
        }

        // GET: Institute
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.SendAsync(new InstituteListQuery());
            return View(list);
        }

        // GET: Institute/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var institute = await _mediator.SendAsync(new InstituteDetailQuery { Id = id });
            if (institute == null)
            {
                return NotFound();
            }

            return View(institute);
        }

        // GET: Institute/Create
        public IActionResult Create()
        {
            return View("Edit");
        }

        // GET: Institute/Edit/5
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var institute = await _mediator.SendAsync(new InstituteEditQuery { Id = id });
        //    if (institute == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("Edit", institute);
        //}

        // POST: Institute/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(InstituteEditViewModel institute)
        //{
        //    if (institute == null)
        //    {
        //        return BadRequest();
        //    }

        //    var errors = _instituteValidator.Validate(institute);
        //    errors.ToList().ForEach(e => ModelState.AddModelError(e.Key, e.Value));

        //    if (ModelState.IsValid)
        //    {
        //        var isNameUnique = await _mediator.SendAsync(new InstituteNameUniqueQuery { InstituteName = institute.Name, InstituteId = institute.Id });
        //        if (isNameUnique)
        //        {
        //            var id = await _mediator.SendAsync(new EditInstituteCommand { Institute = institute });
        //            return RedirectToAction(nameof(Details), new { id, area = "Admin" });
        //        }

        //        ModelState.AddModelError(nameof(institute.Name), "Institute with same name already exists. Please use different name.");
        //    }

        //    return View("Edit", institute);
        //}

        // GET: Institute/Delete/5
        //[ActionName("Delete")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var viewModel = await _mediator.SendAsync(new DeleteQuery { OrgId = id });
        //    if (viewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    viewModel.Title = $"Delete {viewModel.Name}";

        //    return View(viewModel);
        //}

        // POST: Institute/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    await _mediator.SendAsync(new DeleteInstitute { Id = id });
        //    return RedirectToAction(nameof(Index), new { area = "Admin" });
        //}
    }
}