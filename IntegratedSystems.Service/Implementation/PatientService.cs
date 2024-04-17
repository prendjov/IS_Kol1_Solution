using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository.Interface;
using IntegratedSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;
        private readonly IRepository<Vaccine> _vaccineRepository;
        private readonly IRepository<VaccinationCenter> _vaccinationCenterRepository;
        public PatientService(IRepository<Patient> repository, IRepository<Vaccine> vaccineRepository, IRepository<VaccinationCenter> vaccinationCenterRepository)
        {
            _repository = repository;
            _vaccineRepository = vaccineRepository;
            _vaccinationCenterRepository = vaccinationCenterRepository;
        }
        public Patient Delete(Guid? id)
        {
            if(id != null)
            {
                Patient patient = _repository.Get(id);

                if(patient != null) 
                {

                    _repository.Delete(patient);
                }
                return patient;
            }
            return null;
        }

        public Patient Get(Guid? id)
        {
            Patient patient = _repository.Get(id);

            patient.VaccinationSchedule = _vaccineRepository.GetAll().Where(d => d.PatientId == patient.Id).ToList();

            foreach(Vaccine vaccine in patient.VaccinationSchedule)
            {
                vaccine.PatientFor = patient;
                vaccine.Center = _vaccinationCenterRepository.Get(vaccine.VaccinationCenter);
            }

            return patient;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Patient Insert(Patient entity)
        {
            return _repository.Insert(entity);
        }

        public Patient Update(Patient entity)
        {
            return _repository.Update(entity);
        }
    }
}
