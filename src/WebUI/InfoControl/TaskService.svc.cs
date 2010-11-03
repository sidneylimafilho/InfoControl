using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Web.Services;
using Vivina.Erp.SystemFramework;
using InfoControl.Web;

namespace Vivina.Erp.WebUI.InfoControl
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TaskService : DataServiceBase
    {
        [OperationContract]
        [JavaScriptSerializer]
        public void CompleteTask(Int32 companyId, Int32 taskId, Int32 userId)
        {
            using (var manager = new TaskManager(null))
            {
                Task originalTask = manager.GetTask(taskId);
                Task task = originalTask.Duplicate();
                task.TaskStatusId = TaskStatus.Concluded;
                task.CreatorUserId = userId;
                manager.SaveTask(originalTask, task, null);
            }
        }

        [OperationContract]
        [JavaScriptSerializer]
        public ClientResponse GetTasks(string name, int status, int view, string inicio, string fim, int? parentId)
        {
            view = view != 0 ? view : 1;
            return new ClientResponse(() =>
            {
                return new TaskManager(null).GetTasks(User.Identity.UserId, status, view.ToEnum<FilterType>(), name,
                                                      null, parentId, null, null, null, null, "").Select(t => t.Duplicate()).ToArray();
            });
        }
    }
}
