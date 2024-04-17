using IntegratedSystems.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Interface
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAll();
        Patient Get(Guid? id);
        Patient Insert(Patient entity);
        Patient Update(Patient entity);
        Patient Delete(Guid? id);

    }
}
