using System.Collections.Generic;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public interface ISecondarySourceOfEnquiryRepository
    {
        List<SecondarySourceOfEnquiry> GetAll();
        SecondarySourceOfEnquiry GetById(long id);
        long Insert(SecondarySourceOfEnquiry model, long createUser);
        void Update(SecondarySourceOfEnquiry model, long updateUser);
        void ChangeStatus(long id, string statusFlag, long updateUser);
        void Delete(long id);
    }
}