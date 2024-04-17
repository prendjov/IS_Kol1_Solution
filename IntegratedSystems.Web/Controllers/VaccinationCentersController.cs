using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository;
using IntegratedSystems.Service.Interface;
using IntegratedSystems.Domain.DTO;

namespace IntegratedSystems.Web.Controllers
{
    public class VaccinationCentersController : Controller
    {
        private readonly IVaccinationCenterService _service;

        public VaccinationCentersController(IVaccinationCenterService service)
        {
            _service = service;
        }

        // GET: VaccinationCenters
        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: VaccinationCenters/Details/5
        public IActionResult Details(Guid? id)
        {
            var vaccinationCenter = _service.Get(id);

            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VaccinationCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            if (ModelState.IsValid)
            {
                vaccinationCenter.Id = Guid.NewGuid();
                VaccinationCenter center = _service.Insert(vaccinationCenter);
                return RedirectToAction(nameof(Index));
            }
            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Edit/5
        public IActionResult Edit(Guid? id)
        {
            var vaccinationCenter = _service.Get(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }
            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            if (id != vaccinationCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Update(vaccinationCenter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationCenterExists(vaccinationCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Delete/5
        public IActionResult Delete(Guid? id)
        {
            VaccinationCenter vaccinationCenter = _service.Get(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VaccinationCenterExists(Guid id)
        {
            return _service.Get(id) != null;
        }
        // GET: VaccinationCenters/EvidentVaccine
        public IActionResult EvidentVaccine(Guid? id)
        {
            VaccinationCenter center = _service.Get(id);

            if (center.MaxCapacity <= center.Vaccines.Count)
            {
                return View("VaccinationCenterFull");
            }

            EvidentVaccineDTO model = _service.PrepareEvident(center);

            return View(model);
        }

        // POST: VaccinationCenters/EvidentVaccine
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EvidentVaccine([Bind("PatientId, VaccinationCenter, Manufacturer")] EvidentVaccineDTO vaccineInfo)
        {
            Vaccine vaccine = new Vaccine()
            {
                PatientId = vaccineInfo.PatientId,
                Manufacturer = vaccineInfo.Manufacturer,
                VaccinationCenter = vaccineInfo.VaccinationCenter,
            };
            vaccine = _service.EvidentVaccine(vaccine);

            if (vaccine != null)
            {
                return View("Details", vaccine.Center);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
