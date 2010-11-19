using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using InfoControl.Security.Cryptography;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules.Comments
{
    public class CommentsManager : BusinessManager<InfoControlDataContext>
    {
        public CommentsManager(IDataAccessor container)
            : base(container)
        {
        }

        public IQueryable<Comment> GetComments(Int32 subjectId, String pageName)
        {
            return
                DbContext.Comments.Where(comment => comment.SubjectId == subjectId && comment.PageName.Equals(pageName));
        }

        public IQueryable<Comment> GetPageCommentsByCompany(Int32 companyId)
        {
            return from comm in DbContext.Comments
                   where comm.CompanyId == companyId && comm.PageName.ToLower() == "comments.aspx"
                   orderby comm.CreatedDate descending 
                   select comm;
        }        

        /// <summary>
        /// this method insert a comment
        /// </summary>
        /// <param name="entity"></param>
        public void Save(Comment comment)
        {
            if (comment.PageName.ToLower().Contains("task"))
                AddCommentInTask(comment);
            else if (comment.PageName.ToLower().Contains("customercall"))
                AddCommentInCustomerCall(comment);
            else
                Insert(comment);
        }

        /// <summary>
        /// Inserts a comment
        /// </summary>
        /// <param name="comment"></param>
        private void Insert(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;
            DbContext.Comments.InsertOnSubmit(comment);
            DbContext.SubmitChanges();
        }



        #region Task

        public void AddCommentInTask(int id, string text)
        {
            AddCommentInTask(new Comment()
            {
                Description = text,
                SubjectId = id
            });
        }

        public void AddCommentInTask(Comment comment)
        {
            comment.PageName = "task.aspx";
            Insert(comment);
            AddAlertWhenPostCommentInTask(comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        private void AddAlertWhenPostCommentInTask(Comment comment)
        {
            if (comment.PageName.ToLower() == "task.aspx")
            {
                var manager = new AlertManager(this);
                var task = new TaskManager(this).GetTask(comment.SubjectId);

                string link = TaskManager.CreateTaskHtmlLink(new Task
                {
                    TaskId = comment.SubjectId,
                    Name = "\"" + comment.Description.Shortly(140) + "\""
                });

                string alertDescription = string.Format(@"
                    <b>{0}</b> falou: {1}
                    <center>
                        <a href='javascript:;' onclick='$(this).next().show()'>Clique aqui para responder!</a>
                        <iframe id=""comments"" style='display:none' src='../app_shared/comments.aspx?id={2}&pageName=task.aspx'>
                    </center>",
                    comment.UserName,
                    link,
                    comment.SubjectId);

                foreach (TaskUser tu in task.TaskUsers)
                    if (comment.UserName != tu.User.Profile.Name)
                        manager.InsertAlert(tu.UserId, alertDescription);
            }
        }
        #endregion

        #region CustomerCall
        public void AddCommentInCustomerCall(int companyId, int id, string description)
        {
            var user = new CustomerManager(this).GetCustomerCall(id).User;

            string userName = "Usuário não identificado";
            string email = "user@default.com";

            if (user != null)
            {
                userName = user.Profile.Name;
                email = user.UserName;
            }

            AddCommentInCustomerCall(new Comment()
            {
                CompanyId = companyId,
                SubjectId = id,
                Description = description,
                UserName = userName,
                Email = email,
                PageName = "customercall.aspx"
            });
        }

        public void AddCommentInCustomerCall(Comment comment)
        {
            Insert(comment);
            AddAlertWhenPostCommentInCustomerCall(comment);
        }

        private void AddAlertWhenPostCommentInCustomerCall(Comment comment)
        {
            if (comment.PageName.ToLower() == "customercall.aspx")
            {
                var alertManager = new AlertManager(this);
                var customerManager = new CustomerManager(this);

                var call = customerManager.GetCustomerCall(comment.SubjectId);

                //
                // defines Alert description
                //
                string alertDescription = string.Format(@"
                    No chamado <a href='javascript:;' 
                                  onclick=""top.$.LightBoxObject.show('CRM/CustomerCall.aspx?lightbox[iframe]=true&CustomerCallId={0}&ReadOnly=true&ModalPopup=1')"" 
                                  target='content'>{1}</a>, {2} disse: {3} &nbsp;&nbsp;&nbsp;&nbsp;<br />
                    <center>
                        <a href='javascript:;' onclick='$(this).next().show()'>Clique aqui para responder!</a>
                        <iframe id=""comments"" style='display:none' src='../app_shared/comments.aspx?id={4}&pageName=customercall.aspx'>
                    </center>", call.CustomerCallId.ToString(),
                               call.CallNumber,
                                    comment.UserName,
                                    comment.Description.Shortly(140),
                                    comment.SubjectId);

                //
                // Send alert to CreatorUser from CustomerCall
                // 
                if (call.UserId.HasValue)
                    if (call.User.Profile.AbreviatedName != comment.UserName) // Don't alert yourself
                        alertManager.InsertAlert(call.UserId.Value, alertDescription);


                //
                // Send alert to Technical
                // 
                if (call.TechnicalEmployeeId.HasValue)
                {
                    var humanResourcesManager = new HumanResourcesManager(this);
                    User user = humanResourcesManager.GetUserByEmployee(call.TechnicalEmployeeId.Value);
                    if (user != null && user.Profile.AbreviatedName != comment.UserName) // Don't alert yourself
                        alertManager.InsertAlert(user.UserId, alertDescription);
                }
            }
        }
        #endregion

    }
}