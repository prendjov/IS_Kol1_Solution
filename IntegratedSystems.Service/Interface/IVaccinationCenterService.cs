using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Interface
{
    public interface IVaccinationCenterService
    {
        IEnumerable<VaccinationCenter> GetAll();
        VaccinationCenter Get(Guid? id);
        VaccinationCenter Insert(VaccinationCenter entity);
        VaccinationCenter Update(VaccinationCenter entity);
        VaccinationCenter Delete(Guid? id);
        EvidentVaccineDTO PrepareEvident(VaccinationCenter center);
        Vaccine EvidentVaccine(Vaccine vaccine);
    }
}
