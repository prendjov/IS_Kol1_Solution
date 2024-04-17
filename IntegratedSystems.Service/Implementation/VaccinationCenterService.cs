using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Domain.DTO;
using IntegratedSystems.Repository.Interface;
using IntegratedSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Implementation
{
    public class VaccinationCenterService : IVaccinationCenterService
    {
        private readonly IRepository<VaccinationCenter> _repository;
        private readonly IRepository<Vaccine> _vaccineRepository;
        private readonly IRepository<Patient> _patientRepository;
        public VaccinationCenterService(IRepository<VaccinationCenter> repository, IRepository<Vaccine> vaccineRepository, IRepository<Patient> patientRepository)
        {
            _repository = repository;
            _vaccineRepository = vaccineRepository;
            _patientRepository = patientRepository;
        }
        public VaccinationCenter Delete(Guid? id)
        {
            if(id != null)
            {
                VaccinationCenter model = _repository.Get(id);

                if(model != null)
                {
                    return _repository.Delete(model);
                }
            }

            return null;
        }

        public VaccinationCenter Get(Guid? id)
        {
            VaccinationCenter center = _repository.Get(id);

            center.Vaccines = _vaccineRepository.GetAll().Where(s => s.VaccinationCenter == center.Id).ToList();

            foreach(var vaccine in center.Vaccines)
            {
                vaccine.PatientFor = _patientRepository.Get(vaccine.PatientId);
                vaccine.Center = center;
            }

            return center;
        }

        public IEnumerable<VaccinationCenter> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public VaccinationCenter Insert(VaccinationCenter entity)
        {
            return _repository.Insert(entity);
        }
        public VaccinationCenter Update(VaccinationCenter entity)
        {
            return _repository.Update(entity);
        }
        public EvidentVaccineDTO PrepareEvident(VaccinationCenter center)
        {
            EvidentVaccineDTO evidentVaccine = new EvidentVaccineDTO()
            {
                VaccinationCenter = center.Id,
                Patients = _patientRepository.GetAll().ToList(),
                Vaccines = new List<string>()
                {
                    "Astra Zeneca",
                    "Phizer",
                    "Synovak",
                    "Synofarm",
                    "Sputnik"
                }
            };

            return evidentVaccine;
        }
        public Vaccine EvidentVaccine(Vaccine vaccine)
        {
            vaccine.Id = Guid.NewGuid();
            vaccine.Certificate = Guid.NewGuid();
            vaccine.DateTaken = DateTime.Now;
            vaccine.Center = _repository.Get(vaccine.VaccinationCenter);
            vaccine.PatientFor = _patientRepository.Get(vaccine.PatientId);

            Vaccine newVaccine = _vaccineRepository.Insert(vaccine);

            return newVaccine;
        }

    }
}
