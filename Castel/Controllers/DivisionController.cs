using AutoMapper;
using Castel.Core.Unit;
using Castel.DTO;
using Castel.Extensions;
using Castel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Castel.Controllers
{
    public class DivisionController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DivisionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // GET: DivisionController
        public async Task<IActionResult> Index()
        {
            var Divisions = await unitOfWork.DivisionRepository.GetAll();
            var DivisionsList = mapper.Map<List<GetDivisionDTO>>(Divisions);
            return View(DivisionsList);
        }

        // GET: DivisionController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var Division = await unitOfWork.DivisionRepository.Get(d => d.id == id).ValidateNotFound();
            var DivisionDTO = mapper.Map<GetDivisionDTO>(Division);
            return View(DivisionDTO);
        }

        // GET: DivisionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DivisionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddDivisionDTO divisionDTO)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create));

            var division = mapper.Map<Division>(divisionDTO);
            division.CreatedById = CurrentUserId;
            await unitOfWork.DivisionRepository.Insert(division);
            await unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: DivisionController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var Division = await unitOfWork.DivisionRepository.Get(d => d.id == id).ValidateNotFound();
            var DivisionDTO = mapper.Map<GetDivisionDTO>(Division);
            return View(DivisionDTO);
        }

        // POST: DivisionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GetDivisionDTO divisionDTO)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index));
            var Division = mapper.Map<Division>(divisionDTO);
            Division.UpdatedAt = DateTime.UtcNow.ToLocalTime();
            Division.UpdatedById = CurrentUserId;
            await unitOfWork.DivisionRepository.Update(Division);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: DivisionController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Division = await unitOfWork.DivisionRepository.Get(d => d.id == id).ValidateNotFound();
            var DivisionDTO = mapper.Map<GetDivisionDTO>(Division);
            return View(DivisionDTO);
        }

        // POST: DivisionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, GetDivisionDTO divisionDTO)
        {
            await unitOfWork.DivisionRepository.Delete(divisionDTO);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
