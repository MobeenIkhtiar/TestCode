using Backend.Common.CommonFunctions;
using Backend.Domain.BackendSetup;
using Backend.Dto.BackendSetup;

namespace Backend.WebHost.HostModel
{
    public static class BackendTaskHostModel
    {


        public static BackendTask AssignToUpdateModel(BackendTask _getDetailsObj, BackendTaskPutDto _objPut)
        {
            _getDetailsObj.Name = _objPut.Name;
           

            _getDetailsObj.IsActive = true;

            return _getDetailsObj;
        }
        public static BackendTask CreatedTracking(BackendTask model, Nullable<int> loginId)
        {

            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedBy = loginId;
            model.CreatedAt = DateTime.Now;
            return model;
        }

        public static BackendTask UpdatedTracking(BackendTask model, Nullable<int> loginId)
        {
            model.UpdatedBy = loginId;
            model.UpdatedAt = DateTime.Now;
            return model;
        }
    }
}
