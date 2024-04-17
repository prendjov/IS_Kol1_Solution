using IntegratedSystems.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Domain.DTO
{
    public class EvidentVaccineDTO
    {

        public Guid PatientId { get; set; }
        public string Manufacturer {  get; set; }
        public Guid VaccinationCenter { get; set; }
        public List<Patient> Patients { get; set; }
        public List<string> Vaccines { get; set; }
    }
}
