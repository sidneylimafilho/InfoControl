using System;
using System.Collections;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using User = InfoControl.Web.Security.DataEntities.User;
using System.Collections.Generic;

namespace Vivina.Erp.BusinessRules
{
    public enum EmployeeStatus
    {
        Active = 1,
        Inactive
    }

    public class HumanResourcesManager : BusinessManager<InfoControlDataContext>
    {
        public HumanResourcesManager(IDataAccessor container)
            : base(container)
        {
        }

        #region Aux Tables

        public IQueryable<Bond> GetBond()
        {
            return DbContext.Bonds.Sort("NAME");
        }

        /// <summary>
        /// This method returns rows of the side table "Shifts" of the same CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Shift> GetShifts()
        {
            return DbContext.Shifts.Sort("NAME");
        }

        /// <summary>
        /// This method returns rows of the side table "WorkJourney" of the same CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<WorkJourney> GetWorkJourneys()
        {
            return DbContext.WorkJourneys.AsQueryable();
        }

        /// <summary>
        /// This method returns rows of the side table "EmployeeFunction" of the same CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<EmployeeFunction> GetEmployeeFunctions(int companyId)
        {
            return DbContext.EmployeeFunctions.Where(x => x.CompanyId == companyId).Sort("NAME");
        }

        /// <summary>
        /// This method returns rows of the side table "EmployeeFunction" of the same CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Post> GetPosts(int companyId)
        {
            return DbContext.Posts.Where(x => x.CompanyId == companyId).Sort("NAME");
        }

        /// <summary>
        /// This method returns all alienations by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Alienation> GetAlienations()
        {
            return DbContext.Alienations.Sort("Name");
        }

        #endregion

        #region AdditionalInformation

        /// <summary>
        /// This method returns rows of the side table "AdditionalInformation" of the same companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<AdditionalInformation> GetAllAdditionalInformation(int companyId)
        {
            return DbContext.AdditionalInformations.Where(x => x.CompanyId == companyId);
        }

        public EmployeeAdditionalInformation GetEmployeeAdditionalInformation(int companyId, int addonInfoId, int employeeId)
        {
            return DbContext.EmployeeAdditionalInformations.Where(x => x.CompanyId == companyId
                                                                       && x.AddonInfoId == addonInfoId &&
                                                                       x.EmployeeId == employeeId).FirstOrDefault();
        }


        public IList GetAdditionalInformationToDataTable(int companyId)
        {
            return DbContext.AdditionalInformations.Where(x => x.CompanyId == companyId).ToList();
        }

        public IQueryable<AdditionalInformationData> GetAdditionalInformationData(int companyId, int addonInfoId)
        {
            return
                DbContext.AdditionalInformationDatas.Where(x => x.CompanyId == companyId && x.AddonInfoId == addonInfoId);
        }

        public void DeleteEmployeeAdditionalInformation(EmployeeAdditionalInformation entity)
        {
            DbContext.EmployeeAdditionalInformations.Attach(entity);
            DbContext.EmployeeAdditionalInformations.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method verify if the line exist in the database, if so, the line is deleted and updated
        /// if not the line is inserted
        /// </summary>
        /// <param name="entity"></param>
        public void InsertEmployeeAdditionalInformation(EmployeeAdditionalInformation entity)
        {
            EmployeeAdditionalInformation addInfo = GetEmployeeAdditionalInformation(entity.CompanyId,
                                                                                     entity.AddonInfoId,
                                                                                     entity.EmployeeId);
            if (addInfo != null)
                DeleteEmployeeAdditionalInformation(addInfo);
            DbContext.EmployeeAdditionalInformations.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region OrganizationLevel

        /// <summary>
        /// This method get company's organization level
        /// </summary>
        /// <param name="companyID">Can't be null</param>
        /// <returns>a DataTable of organizationLevel</returns>
        public DataTable GetCompanyOrganizationLevel(Int32 companyId)
        {
            return GetAllOrganizationLevel(companyId).ToDataTable();
        }

        public IQueryable<OrganizationLevel> GetAllOrganizationLevel(int companyId)
        {
            return DbContext.OrganizationLevels.Where(x => x.CompanyId == companyId).OrderBy(ol => ol.Parentid);
        }

        public DataTable GetAllOrganizationLevelToDataTable(int companyId)
        {
            return GetAllOrganizationLevel(companyId).OrderBy(t => t.Name).ToDataTable();
        }

        public IList GetParentOrganizationLevel(int companyId)
        {
            return DbContext.OrganizationLevels.Where(x => x.CompanyId == companyId && x.Parentid == null).ToList();
        }

        public IQueryable<OrganizationLevel> GetChildOrganizationLevel(int companyId, int parentId)
        {
            return DbContext.OrganizationLevels.Where(x => x.CompanyId == companyId && x.Parentid == parentId);
        }

        public OrganizationLevel GetOrganizationLevel(int companyId, int organizationLevelId)
        {
            return DbContext.OrganizationLevels.Where(x => x.OrganizationlevelId == organizationLevelId
                                                           && x.CompanyId == companyId).FirstOrDefault();
        }

        public void InsertOrganizationLevel(OrganizationLevel entity)
        {
            DbContext.OrganizationLevels.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        public void DeleteOrganizationLevel(OrganizationLevel entity)
        {
            DbContext.OrganizationLevels.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method update the Organization Level
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateOrganizationLevel(OrganizationLevel original_entity, OrganizationLevel entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        #endregion







        #region Employee

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Employee entity)
        {
            DbContext.Employees.Attach(entity);
            DbContext.Employees.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Employee entity)
        {
            entity.CreatedDate = DateTime.Now;
            DbContext.Employees.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Employee original_entity, Employee entity)
        {
            entity.ModifiedDate = DateTime.Now;
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method saves an employee
        /// </summary>
        /// <param name="employee"></param>
        public void SaveEmployee(Employee employee)
        {
            if (employee.EmployeeId == 0)
            {
                employee.CreatedDate = employee.ModifiedDate = DateTime.Now;
                Insert(employee); 
                return;
            }

            var originalEmployee = GetEmployee(employee.CompanyId, employee.EmployeeId);

            if (originalEmployee.EmployeeFunctionId != employee.EmployeeFunctionId)
                SetEmployeeFunctionHistory(originalEmployee, employee);

            if (originalEmployee.IsActive != employee.IsActive)
                SetEmployeeStatusHistory(originalEmployee, employee);

            originalEmployee.ModifiedDate = DateTime.Now;

            Update(originalEmployee, employee);
        }

        /// <summary>
        /// This method inserts an employee function history
        /// </summary>
        /// <param name="originalEmployee"></param>
        /// <param name="employee"></param>
        private void SetEmployeeFunctionHistory(Employee originalEmployee, Employee employee)
        {
            var employeeFunctionHistory = new EmployeeFunctionHistory();

            employeeFunctionHistory.CompanyId = originalEmployee.CompanyId;
            employeeFunctionHistory.EmployeeId = originalEmployee.EmployeeId;
            employeeFunctionHistory.EmployeeFunctionId = Convert.ToInt32(employee.EmployeeFunctionId);
            employeeFunctionHistory.ModifedDate = DateTime.Now;

            InsertFunctionHistory(employeeFunctionHistory);
        }

        /// <summary>
        /// This method inserts an employee status history
        /// </summary>
        /// <param name="originalEmployee"></param>
        /// <param name="employee"></param>
        private void SetEmployeeStatusHistory(Employee originalEmployee, Employee employee)
        {
            var employeeStatusHistory = new StatusHistory();

            employeeStatusHistory.CompanyId = originalEmployee.CompanyId;
            employeeStatusHistory.EmployeeId = originalEmployee.EmployeeId;
            employeeStatusHistory.AlienationId = !employee.IsActive.Value ? employee.AlienationId : null;
            employeeStatusHistory.ModifiedDate = DateTime.Now;

            InsertStatusHistory(employeeStatusHistory);
        }


        public IQueryable GetEmployeeByEnum(Int32 companyId, EmployeeStatus employeeStatus, string sortExpression,
                                            Int32 maximumRows, Int32 startRowIndex)
        {
            IQueryable query;

            switch (employeeStatus)
            {
                case EmployeeStatus.Inactive:
                    query = GetInactiveEmployees(companyId, sortExpression, maximumRows, startRowIndex);
                    break;
                case EmployeeStatus.Active:
                    query = GetActiveEmployees(companyId, sortExpression, maximumRows, startRowIndex);
                    break;
                default:
                    query = GetAllEmployees(companyId, sortExpression, maximumRows, startRowIndex);
                    break;
            }
            return query;
        }

        public Int32 GetEmployeeByEnumCount(Int32 companyId, EmployeeStatus employeeStatus, string sortExpression,
                                            Int32 maximumRows, Int32 startRowIndex)
        {
            return
                GetEmployeeByEnum(companyId, employeeStatus, sortExpression, maximumRows, startRowIndex).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// This method retrieve employees by status and/or with the start letter of name 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeStatus"></param>
        /// <param name="initialLetter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public IQueryable GetEmployeeByEnum(Int32 companyId, EmployeeStatus employeeStatus, String initialLetter,
                                            string sortExpression,
                                            Int32 maximumRows, Int32 startRowIndex)
        {
            return GetEmployeesByStatus(companyId, (int?)employeeStatus, initialLetter, sortExpression, maximumRows,
                                        startRowIndex);
        }

        /// <summary>
        /// This method retrieve employees by general text. This text can be name, email, enrollment and accoutNumber
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="text"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public IQueryable<Employee> GetEmployees(Int32 companyId, String text, string sortExpression,
                                       Int32 maximumRows, Int32 startRowIndex)
        {
            var query = from employee in GetEmployeesByStatus(companyId, (int)EmployeeStatus.Active)
                        where employee.Profile.Name.Contains(text) ||
                        employee.Profile.Email.Contains(text) || employee.Enrollment.Contains(text) || employee.AccountNumber.Contains(text)
                        select employee;



            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "");
        }


        public IQueryable<Recognizable> SearchEmployees(Int32 companyId, String text, Int32 maximumRows)
        {
            var query = from e in GetEmployeesByStatus(companyId, (int)EmployeeStatus.Active)
                        where e.Profile.Name.Contains(text)
                        select new Recognizable(e.EmployeeId.ToString(), e.Profile.CPF + " | " + e.Profile.Name);

            return query;
        }

        public Int32 GetEmployeesCount(Int32 companyId, String text, string sortExpression,
                                       Int32 maximumRows, Int32 startRowIndex)
        {
            return GetEmployees(companyId, text, sortExpression, maximumRows, startRowIndex).Cast<Object>().Count();
        }


        /// <summary>
        /// This method retrieve the quantity of results genereted of GetEmployeeByEnum method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeStatus"></param>
        /// <param name="initialLetter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public Int32 GetEmployeeByEnumCount(Int32 companyId, EmployeeStatus employeeStatus, String initialLetter,
                                            string sortExpression,
                                            Int32 maximumRows, Int32 startRowIndex)
        {
            return
                GetEmployeeByEnum(companyId, employeeStatus, initialLetter, sortExpression, maximumRows, startRowIndex).
                    Cast<IQueryable>().Count();
        }

        #region EmployeesByStatus

        //
        // Esse método tem mais parâmetros, portanto ele é o mais genérico 
        //

        public IQueryable GetEmployeesByStatus(int companyId, int? status, String initialLetter, string sortExpression,
                                               int maximumRows, int startRowIndex)
        {
            var employeeQuery = from employee in GetEmployeesByStatus(companyId, status)
                                join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                                select new
                                {
                                    employee.Enrollment,
                                    employee.EmployeeId,
                                    employee.CompanyId,
                                    employee.ProfileId,
                                    profile.CPF,
                                    profile.Name,
                                    employee.AlienationId,
                                    employee.IsActive
                                };

            if (!String.IsNullOrEmpty(initialLetter))
                employeeQuery = employeeQuery.Where(employee => employee.Name.StartsWith(initialLetter));

            return employeeQuery.SortAndPage(sortExpression, startRowIndex, maximumRows, "EmployeeId");
        }

        /// <summary>
        /// this method return employees by status
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="status"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        private IQueryable GetEmployeesByStatus(int companyId, int? status, string sortExpression,
                                                int maximumRows, int startRowIndex)
        {
            return GetEmployeeByEnum(companyId, (EmployeeStatus)status, "", sortExpression, maximumRows, startRowIndex);

            //return employeeQuery.SortAndPage(sortExpression, startRowIndex, maximumRows, "EmployeeId");
        }

        private IQueryable<Employee> GetEmployeesByStatus(int companyId, int? status)
        {
            IQueryable<Employee> employees = DbContext.Employees.Where(employee => employee.CompanyId == companyId);

            if (status.HasValue && status.Value == (Int32)EmployeeStatus.Active)
                employees = employees.Where(employee => employee.IsActive == true);

            if (status.HasValue && status.Value == (Int32)EmployeeStatus.Inactive)
                employees = employees.Where(employee => employee.IsActive == false);
            return employees;
        }

        /// <summary>
        /// this method return the active employees by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public IQueryable GetActiveEmployees(Int32 companyId, string sortExpression, Int32 maximumRows,
                                             Int32 startRowIndex)
        {
            return GetEmployeesByStatus(companyId, (Int32)EmployeeStatus.Active, sortExpression, maximumRows,
                                        startRowIndex);
        }

        /// <summary>
        /// this is the count Method of GetActiveEmployees Method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public Int32 GetActiveEmployeesCount(Int32 companyId, string sortExpression, Int32 maximumRows,
                                             Int32 startRowIndex)
        {
            return GetActiveEmployees(companyId, sortExpression, maximumRows, startRowIndex).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return the active employees by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public IQueryable GetInactiveEmployees(Int32 companyId, string sortExpression, Int32 maximumRows,
                                               Int32 startRowIndex)
        {
            return GetEmployeesByStatus(companyId, (Int32)EmployeeStatus.Inactive, sortExpression, maximumRows,
                                        startRowIndex);
        }

        /// <summary>
        /// this is the count method of GetInactiveEmployees
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public Int32 GetInactiveEmployeesCount(Int32 companyId, string sortExpression, Int32 maximumRows,
                                               Int32 startRowIndex)
        {
            return
                GetInactiveEmployees(companyId, sortExpression, maximumRows, startRowIndex).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all employee by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public IQueryable GetAllEmployees(Int32 companyId, string sortExpression, Int32 maximumRows, Int32 startRowIndex)
        {
            return GetEmployeesByStatus(companyId, null, sortExpression, maximumRows, startRowIndex);
        }

        /// <summary>
        /// this is the count method of GetAllEmployee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public Int32 GetAllEmployeesCount(Int32 companyId, string sortExpression, Int32 maximumRows, Int32 startRowIndex)
        {
            return GetAllEmployees(companyId, sortExpression, maximumRows, startRowIndex).Cast<IQueryable>().Count();
        }

        #endregion

        #region Employee

        /// <summary>
        ///  This method returns a Employee in accordance with your profile.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByProfile(int profileId, int companyId)
        {
            return DbContext.Employees.Where(x => x.CompanyId == companyId && x.ProfileId == profileId).FirstOrDefault();
        }

        /// <summary>
        /// this method search the employee by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByName(string name, Int32 companyId)
        {
            return GetEmployeeByCompany(companyId).Where(e => e.Profile.Name.Contains(name)).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves a single Employee.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=employeeId>employeeId</param>
        /// <param name=companyId>companyId</param>
        public Employee GetEmployee(Int32 companyId, Int32 employeeId)
        {
            // 
            return GetEmployeeByCompany(companyId).Where(employee => employee.EmployeeId == employeeId).FirstOrDefault();
        }

        /// <summary>
        /// This method return a employee in accordance with your cpf
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Employee RetrieveEmployeeByCpf(int companyId, string cpf)
        {
            return GetEmployeeByCompany(companyId).Where(x => x.Profile.CPF == cpf).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a employee by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByUser(User logedUser, int companyId)
        {
            Employee employee = (from emp in DbContext.Employees
                                 join profile in DbContext.Profiles on emp.ProfileId equals profile.ProfileId
                                 join user in DbContext.Users on emp.ProfileId equals user.ProfileId
                                 where (user.UserId == logedUser.UserId && emp.CompanyId == companyId)
                                 select emp).FirstOrDefault();

            //
            // Create a employee with that profile
            //
            if (employee == null)
            {
                var humanResourcesManager = new HumanResourcesManager(this);
                employee = new Employee();
                employee.ProfileId = Convert.ToInt32(logedUser.ProfileId);
                employee.CompanyId = companyId;
                humanResourcesManager.Insert(employee);
            }

            return employee;
        }

        /// <summary>
        /// This method return a employee by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByUser(Int32 userId)
        {
            IQueryable<Employee> query = from employee in DbContext.Employees
                                         join profile in DbContext.Profiles on employee.ProfileId equals
                                             profile.ProfileId
                                         join user in DbContext.Users on employee.ProfileId equals user.ProfileId
                                         where (user.UserId == userId)
                                         select employee;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// This method return a employee by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataClasses.User GetUserByEmployee(Int32 employeeId)
        {
            IQueryable<DataClasses.User> query =
                from employee in DbContext.Employees.Where(e => e.EmployeeId == employeeId)
                join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                join user in DbContext.Users on employee.ProfileId equals user.ProfileId
                select user;
            return query.FirstOrDefault();
        }

        #endregion

        #region Employee Dependents

        /// <summary>
        /// This method returns all employee dependents in accordance with parameters pessed.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<EmployeeDependent> GetEmployeeDependents(int employeeId, int companyId)
        {
            return DbContext.EmployeeDependents.Where(x => x.CompanyId == companyId && x.EmployeeId == employeeId);
        }

        public IQueryable<EmployeeDependent> GetEmployeeDependents(int employeeId, int companyId, string sortExpression,
                                                                   int maximumRows, int startRowIndex)
        {
            return
                DbContext.EmployeeDependents.Where(x => x.CompanyId == companyId && x.EmployeeId == employeeId).
                    SortAndPage(sortExpression, startRowIndex, maximumRows, "EmployeeDependentId");
        }

        public Int32 GetEmployeeDependentsCount(int employeeId, int companyId, string sortExpression, int maximumRows,
                                                int startRowIndex)
        {
            return GetEmployeeDependents(employeeId, companyId).Count();
        }

        /// <summary>
        /// This method insert a new employee dependent.
        /// </summary>
        /// <param name="entity"></param>
        public void InsertEmployeeDependent(EmployeeDependent entity)
        {
            DbContext.EmployeeDependents.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete a employee dependent of the database.
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEmployeeDependent(EmployeeDependent entity)
        {
            DbContext.EmployeeDependents.Attach(entity);
            DbContext.EmployeeDependents.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a employee dependent on the database
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateEmployeeDependent(EmployeeDependent original_entity, EmployeeDependent entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns a employee dependent in accordance with id passed
        /// </summary>
        /// <param name="employeeDependentId"></param>
        /// <returns></returns>
        public EmployeeDependent GetEmployeeDependent(int employeeDependentId)
        {
            return
                DbContext.EmployeeDependents.Where(x => x.EmployeeDependentId == employeeDependentId).FirstOrDefault();
        }

        # endregion

        #region EmployeeCompetency

        /// <summary>
        /// This method returns the employeeCompetencies by specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetEmployeeCompetenciesByCompany(Int32 companyId)
        {
            var query = from employee in GetEmployees(companyId, true)
                        join competency in DbContext.EmployeeCompetencies on employee.EmployeeId equals
                            competency.EmployeeId
                        where employee.CompanyId == companyId
                        group competency by competency.Name
                            into gCompetencyNames
                            select new
                            {
                                CompetencyName = gCompetencyNames.Key
                            };

            return query;
        }

        #endregion

        #region AdvancedCrud

        /// <summary>
        /// Insert the Employee
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="addInfoList"></param>
        /// <returns></returns>
        public int InsertEmployee(Employee entity, List<EmployeeAdditionalInformation> addInfoList)
        {
            Insert(entity);

            var hManager = new HumanResourcesManager(this);
            foreach (EmployeeAdditionalInformation item in addInfoList)
            {
                item.EmployeeId = entity.EmployeeId;
                hManager.InsertEmployeeAdditionalInformation(item);
            }

            return entity.EmployeeId;
        }

        /// <summary>
        /// Update the Employee
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="original_entity"></param>
        /// <param name="addInfoList"></param>
        /// <returns></returns>
        public int UpdateEmployee(Employee entity, Employee original_entity,
                                  List<EmployeeAdditionalInformation> addInfoList)
        {
            entity.AlienationId = entity.AlienationId ?? (int)EmployeeStatus.Active;

            foreach (EmployeeAdditionalInformation item in addInfoList)
            {
                var hManager = new HumanResourcesManager(this);
                item.EmployeeId = entity.EmployeeId;
                hManager.InsertEmployeeAdditionalInformation(item);
            }

            //insert one row in  history If the status change
            if (entity.AlienationId != original_entity.AlienationId)
            {
                var humanResourcesManager = new HumanResourcesManager(this);
                var sHistory = new StatusHistory();
                sHistory.CompanyId = entity.CompanyId;
                sHistory.EmployeeStatusId = (int)entity.AlienationId;
                sHistory.AlienationId = entity.AlienationId;
                sHistory.EmployeeId = entity.EmployeeId;
                sHistory.ModifiedDate = DateTime.Now;
                humanResourcesManager.InsertStatusHistory(sHistory);
            }
            original_entity.CopyPropertiesFrom(entity);
            original_entity.Profile.CopyPropertiesFrom(entity.Profile);
            DbContext.SubmitChanges();
            return entity.EmployeeId;
        }

        /// <summary>
        /// This method delete a employee in accordance with id passed
        /// </summary>
        /// <param name="employeeId"></param>
        public void DeleteEmployee(Employee entity)
        {
            DbContext.Employees.DeleteAllOnSubmit(DbContext.Employees.Where(x => x.EmployeeId == entity.EmployeeId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete the employee by Id
        /// </summary>
        /// <param name="employeeId"></param>
        public void DeleteEmployee(int companyId, int employeeId)
        {
            DataClasses.User user = new CompanyManager(this).GetUserByEmployee(employeeId);
            if (user != null)
                DbContext.CompanyUsers.DeleteAllOnSubmit(
                    DbContext.CompanyUsers.Where(x => x.CompanyId == companyId && x.UserId == user.UserId));

            DbContext.Employees.DeleteAllOnSubmit(
                DbContext.Employees.Where(x => x.EmployeeId == employeeId));
            DbContext.SubmitChanges();
        }

        #endregion

        #region Folha de Pagamento

        #region Support Tables

        private static object obj = new object();
        private static List<InssInterval> _inssInterval;
        private static List<IrrfInterval> _irrfInterval;
        private static List<VacationInterval> _vacationInterval;
        private static List<FamilyRendInterval> _familRendInterval;

        private static List<FamilyRendInterval> FamilyRendInterval
        {
            get
            {
                if (_familRendInterval == null)
                {
                    using (var humanResourcesManager = new HumanResourcesManager(null))
                    {
                        _familRendInterval = humanResourcesManager.GetFamilyRendInterval();
                    }
                    ;
                }
                return _familRendInterval;
            }
        }

        private static List<VacationInterval> VacationInterval
        {
            get
            {
                if (_vacationInterval == null)
                {
                    using (var humanResourcesManager = new HumanResourcesManager(null))
                    {
                        _vacationInterval = humanResourcesManager.GetVacationInterval();
                    }
                    ;
                }
                return _vacationInterval;
            }
        }

        private static List<IrrfInterval> IrrfInterval
        {
            get
            {
                if (_irrfInterval == null)
                    using (var humanResourcesManager = new HumanResourcesManager(null))
                        _irrfInterval = humanResourcesManager.GetIrrfInterval();

                return _irrfInterval;
            }
        }

        private static List<InssInterval> InssInterval
        {
            get
            {
                if (_inssInterval == null)
                    using (var humanResourcesManager = new HumanResourcesManager(null))
                        _inssInterval = humanResourcesManager.GetInssInterval();

                return _inssInterval;
            }
        }

        private List<InssInterval> GetInssInterval()
        {
            return DbContext.InssIntervals.ToList();
        }

        private List<IrrfInterval> GetIrrfInterval()
        {
            return DbContext.IrrfIntervals.ToList();
        }

        private List<VacationInterval> GetVacationInterval()
        {
            return DbContext.VacationIntervals.ToList();
        }

        private List<FamilyRendInterval> GetFamilyRendInterval()
        {
            return DbContext.FamilyRendIntervals.ToList();
        }

        #endregion

        //private Int32 GetEmployeeWorkedDaysInDateInterval(Int32 companyId, Int32 employeeId,
        //                                                  DateTimeInterval dateTimeInterval)
        //{
        //    IQueryable<Int32> query = from appointment in DbContext.Appointments
        //                              where appointment.CompanyId == companyId && appointment.EmployeeId == employeeId
        //                                    && appointment.BeginTime.Date >= dateTimeInterval.BeginDate &&
        //                                    appointment.EndTime <= dateTimeInterval.EndDate
        //                              select appointment.BeginTime.DayOfYear;
        //    return query.Distinct().Count();
        //}

        private Decimal CalculateEmployeeTransportValue(Int32 companyId, Int32 employeeId,
                                                        DateTimeInterval dateTimeInterval)
        {
            Employee employee = GetEmployee(companyId, employeeId);

            if (employee == null || !employee.TransportPerDay.HasValue)
                return Decimal.Zero;

            return Convert.ToDecimal(employee.TransportPerDay) * 0m;
            //GetEmployeeWorkedDaysInDateInterval(companyId, employeeId, dateTimeInterval);
        }

        private Decimal CalculateSalaryInInterval(Employee employee, DateTimeInterval interval)
        {
            if (employee == null || !employee.HH.HasValue)
                return Decimal.Zero;

            if (employee.Salary.HasValue)
                return employee.Salary.Value;

            //var appointmentManager = new AppointmentManager(this);
            return 0m;
            //Convert.ToDecimal(appointmentManager.GetEmployeeWorkedHoursInDateInterval(employee.CompanyId,
            //                                                                          employee.EmployeeId, interval)) * employee.HH.Value;
        }

        private Employee ProcessPayRoll(Employee employee, DateTimeInterval interval)
        {
            decimal salary = CalculateSalaryInInterval(employee, interval);
            decimal inss = ProcessInss(salary, interval);
            decimal netSalary = NetSalary(salary, inss);

            decimal irrf = ProcessIrrf(salary, interval);

            employee.CurrentEvents.Add("Sal. Bruto", salary);
            employee.CurrentEvents.Add("Sal. Liq.", netSalary);
            employee.CurrentEvents.Add("IRRF", irrf);
            employee.CurrentEvents.Add("INSS", inss);
            //employee.CurrentEvents.Add("INSS", inss); 

            return employee;
        }

        private Employee ProcessFgts(Employee employee)
        {
            return employee;
        }

        private decimal NetSalary(decimal salary, decimal inss)
        {
            return salary - inss;
        }

        private static decimal ProcessInss(decimal salary, DateTimeInterval interval)
        {
            return (from inssInt in InssInterval
                    where inssInt.Limit > salary && inssInt.Year == interval.EndDate.Year
                    orderby inssInt.Limit
                    select inssInt.Tax).FirstOrDefault() * salary;
        }

        private decimal ProcessIrrf(decimal salary, DateTimeInterval interval)
        {
            return Convert.ToDecimal((from irrfInt in IrrfInterval
                                      where irrfInt.Limit > salary && irrfInt.Year == interval.EndDate.Year
                                      orderby irrfInt.Limit
                                      select irrfInt.Tax).FirstOrDefault() * salary);
        }

        private Employee ProcessVacation(Employee employee)
        {
            return employee;
        }

        private Employee ProcessFGTS(Employee employee)
        {
            return employee;
        }

        #region Process PayRoll

        private Decimal CalculateBaseIrrfInterval(Decimal salary)
        {
            IrrfInterval irrfInterval = IrrfInterval.Where(x => x.Limit > salary).OrderBy(x => x.Limit).FirstOrDefault();
            return irrfInterval == null
                       ? 0
                       : Convert.ToDecimal(irrfInterval.Tax);
        }

        private Decimal CalculateIrrfInterval(Decimal salary)
        {
            return CalculateBaseIrrfInterval(salary) * salary;
        }

        private Decimal CalculateVacationInterval(Decimal salary)
        {
            return Decimal.Zero;
            //return Convert.ToDecimal(VacationInterval.Where(x => x.Limit > salary).OrderBy(x => x.Limit).FirstOrDefault(). * salary);
        }

        private Decimal CalculateFamilyRendInterval(Decimal salary)
        {
            return Decimal.Zero;
            //return Convert.ToDecimal(FamilyRendInterval.Where(x => x.Limit > salary).OrderBy(x => x.Limit).FirstOrDefault().Tax * salary);
        }

        public IQueryable GetPayRolls(Int32 companyId, DateTimeInterval dateTimeInterval)
        {
            //?? (Convert.ToDecimal(employee.HH) * (employee.Appointments.Sum(app => app.EndTime.Subtract(app.BeginTime).Hours))))
            IQueryable<Employee> query = from employee in GetEmployees(companyId, true)
                                         join profile in DbContext.Profiles on employee.ProfileId equals
                                             profile.ProfileId
                                         select ProcessPayRoll(employee, dateTimeInterval);
            return query;
        }

        #endregion

        #endregion

        #region TechnicalEmployee

        /// <summary>
        /// this method return all Technical employees by company
        /// </summary>
        /// <param name="companyId"></param>
#warning o método que deve ser usado é o GetActiveTechnicalEmployee(companyId)
        public DataTable GetTechnicalEmployeeAsDataTable(Int32 companyId)
        {
            var query = from employee in GetEmployeeByCompany(companyId)
                        join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                        where employee.IsTechnical == true
                        select new
                        {
                            profile.Name,
                            employee.EmployeeId
                        };
            return query.ToDataTable();
        }

        /// <summary>
        /// this method is a base method to GetTechnicalEmployee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeStatus"></param>
        /// <returns></returns>
        private IQueryable GetTechnicalEmployee(Int32 companyId, Int32 employeeStatus)
        {
            var queryTechnicalEmployee = from employee in GetEmployeesByStatus(companyId, employeeStatus)
                                         join profile in DbContext.Profiles on employee.ProfileId equals
                                             profile.ProfileId
                                         where employee.IsTechnical == true
                                         select new
                                         {
                                             profile.Name,
                                             employee.EmployeeId
                                         };
            return queryTechnicalEmployee;
        }

        /// <summary>
        /// this method returns the active technical employee By Company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetActiveTechnicalEmployee(Int32 companyId)
        {
            return GetTechnicalEmployee(companyId, (Int32)EmployeeStatus.Active);
        }

        /// <summary>
        /// this method returns the inactive technical employee By Company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetInactiveTechnicalEmployee(Int32 companyId)
        {
            return GetTechnicalEmployee(companyId, (Int32)EmployeeStatus.Inactive);
        }

#warning este método está fora dos padrões de nomenclatura. Nome apropriado: GetTechnicalEmployees
        /// <summary>
        /// this method return technical employee 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Employee> GetTechnicalEmployee(Int32 companyId)
        {
            return GetEmployeeByCompany(companyId).Where(e => e.IsTechnical == true);
        }

        /// <summary>
        /// this method return all available technical
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="initialTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public IQueryable<Employee> GetAvailableTechnical(Int32 companyId, DateTime initialTime, DateTime endTime)
        {
            return null;
            // GetTechnicalEmployee(companyId).Except(GetUnavailableTechnical(companyId, initialTime, endTime));
        }

        /// <summary>
        /// this method return employeeName and employeeId of an available employee 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="initialTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetAvailableTechnicalAsDataTable(Int32 companyId, DateTime initialTime, DateTime endTime)
        {
            var query = from employee in GetAvailableTechnical(companyId, initialTime, endTime)
                        join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                        select new
                        {
                            employeeName = profile.Name,
                            employeeId = employee.EmployeeId
                        };
            return query.ToDataTable();
        }

        /// <summary>
        /// this method return the unavailable employees
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="initialTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        //public IQueryable<Employee> GetUnavailableTechnical(Int32 companyId, DateTime initialTime, DateTime endTime)
        //{
        //    IQueryable<Employee> queryAppointment = from appointment in DbContext.Appointments
        //                                            where appointment.CompanyId == companyId &&
        //                                                  appointment.BeginTime < endTime &&
        //                                                  appointment.EndTime > initialTime
        //                                            select appointment.Employee;
        //    //group appointment by new { EmployeeId = appointment.Employee.EmployeeId, Name = appointment.Employee.Profile.Name } into gAppointment
        //    //select gAppointment;
        //    return queryAppointment;
        //}
        /// <summary>
        /// this method verify the availability of an employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <param name="initialTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool CheckAvailableEmployee(Int32 companyId, Int32 employeeId, DateTime initialTime, DateTime endTime)
        {
            return GetAvailableTechnical(companyId, initialTime, endTime).Contains(GetEmployee(companyId, employeeId));
        }

        #endregion

        /// <summary>
        /// This method returns all employees that is vendors
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList GetSalesPerson(int companyId)
        {
            var query = from emp in DbContext.Employees
                        join prof in DbContext.Profiles on emp.ProfileId equals prof.ProfileId
                        where emp.CompanyId == companyId && emp.IsSalesperson
                        select new
                        {
                            emp.EmployeeId,
                            prof.Name,
                            prof.AbreviatedName
                        };
            return query.OrderBy(x => x.Name).ToList();
        }


        /// <summary>
        /// This method retrieves Employee by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=companyId>companyId</param>
        public IQueryable<Employee> GetEmployeeByCompany(Int32 companyId)
        {
            // 
            return DbContext.Employees.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// this method returns employees by company as list
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<Employee> GetEmployeeByCompanyAsList(Int32 companyId)
        {
            return GetEmployees(companyId, true).ToList();
        }

        /// <summary>
        /// This method retrieves all Employees.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Employee> GetAllEmployees()
        {
            return DbContext.Employees;
        }

        /// <summary>
        /// This method gets record counts of all Employees.
        /// Do not change this method.
        /// </summary>
        private int GetAllEmployeesCount()
        {
            return GetAllEmployees().Count();
        }

        /// <summary>
        /// This method gets sorted and paged records of all Employees filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Employee> GetEmployees(string tableName, Int32 companyCompanyId, string sortExpression,
                                                  int startRowIndex, int maximumRows)
        {
            IQueryable<Employee> x = GetFilteredEmployees(tableName, companyCompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "EmployeeId");
        }

        /// <summary>
        /// This method retreves the employees by status
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private IQueryable<Employee> GetEmployees(Int32 companyId, Boolean isActive)
        {
            return DbContext.Employees.Where(e => e.CompanyId.Equals(companyId) && e.IsActive == isActive);
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Employee> GetFilteredEmployees(string tableName, Int32 Company_CompanyId)
        {
            switch (tableName)
            {
                case "Company_Employees":
                    return GetEmployeeByCompany(Company_CompanyId);
                default:
                    return GetAllEmployees();
            }
        }

        /// <summary>
        /// This method gets records counts of all Employees filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        private int GetEmployeesCount(string tableName, Int32 companyCompanyId)
        {
            IQueryable<Employee> x = GetFilteredEmployees(tableName, companyCompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method returns an Iqueriable with the names of employees by company  
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns></returns>
        public IQueryable GetEmployeesAsProfileByCompany(int companyId)
        {
            var query = from user in DbContext.Users
                        join profile in DbContext.Profiles on user.ProfileId equals profile.ProfileId
                        join employee in DbContext.Employees on profile.ProfileId equals employee.ProfileId
                        where employee.CompanyId == companyId && employee.IsTechnical.Value
                        select new
                        {
                            user,
                            user.UserId,
                            user.Profile.AbreviatedName,
                            employee.EmployeeId
                        };


            return query;
        }

        /// <summary>
        /// This method returns the employees by an organizationLevel
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="organizationLevelId"></param>
        /// <returns></returns>
        public IQueryable<Employee> GetEmployeesByOrganizationLevel(Int32 companyId, Int32 organizationLevelId)
        {
            return
                GetEmployeesByStatus(companyId, (Int32)EmployeeStatus.Active).Where(
                    employee => employee.OrganizationlevelId == organizationLevelId);
        }

        #region Purchase Competency

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetPurchaseCompetencyTree(Int32 companyId)
        {
            var queryOrganizationLevel = from ol in DbContext.OrganizationLevels
                                         where ol.CompanyId == companyId
                                         select new
                                         {
                                             ol.Name,
                                             Value = default(decimal?),
                                             CentralBuyer = (bool?)false,
                                             Id = ol.OrganizationlevelId * (-1),
                                             ParentId = ol.Parentid * (-1)
                                         };
            var queryEmployee = from employee in GetEmployeeByCompany(companyId)
                                select new
                                {
                                    employee.Profile.Name,
                                    Value = employee.PurchaseCeilingValue,
                                    employee.CentralBuyer,
                                    Id = employee.EmployeeId,
                                    ParentId = employee.OrganizationlevelId * (-1)
                                };
            return queryOrganizationLevel.Union(queryEmployee)
                .OrderBy(key => key.ParentId)
                .OrderByDescending(k => k.Value).ToDataTable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="ceilingValue"></param>
        /// <param name="centralBuyer"></param>
        public void SetEmployeeCompetency(Employee employee, decimal? ceilingValue, bool centralBuyer)
        {
            //
            // Clean up previous central buyer
            //
            //if (centralBuyer)
            //{
            //    Employee centralBuyerEmployee = GetCentralBuyer(employee.CompanyId);
            //    if (centralBuyerEmployee != null)
            //        centralBuyerEmployee.CentralBuyer = false;
            //    DbContext.SubmitChanges();
            //}

            employee = GetEmployee(employee.CompanyId, employee.EmployeeId);
            employee.PurchaseCeilingValue = ceilingValue > 0
                                                ? ceilingValue
                                                : null;
            employee.CentralBuyer = centralBuyer;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataClasses.User GetCentralBuyer(int companyId)
        {
            IQueryable<DataClasses.User> query =
                from emp in GetEmployees(companyId, true).Where(emp => emp.CentralBuyer == true)
                join user in DbContext.Users on emp.ProfileId equals user.ProfileId
                select user;
            return query.FirstOrDefault();
        }

        #endregion

        #region Employee Competency

        /// <summary>
        /// Populate the GridView of skills of the selected employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<EmployeeCompetency> GetEmployeeCompetency(Int32 employeeId, string sortExpression,
                                                                    int startRowIndex, int maximumRows)
        {
            return
                DbContext.EmployeeCompetencies.Where(ec => ec.EmployeeId.Equals(employeeId)).SortAndPage(sortExpression,
                                                                                                         startRowIndex,
                                                                                                         maximumRows,
                                                                                                         "Rating desc");
        }

        public Int32 GetEmployeeCompetencyCount(Int32 employeeId, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            return GetEmployeeCompetency(employeeId, sortExpression,
                                         startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// Verify and insert new or update a skill for the selected employee.
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEmployeeCompetency(EmployeeCompetency entity)
        {
            EmployeeCompetency original_competency =
                DbContext.EmployeeCompetencies.Where(
                    c => c.EmployeeId.Equals(entity.EmployeeId) && c.Name.Equals(entity.Name)).FirstOrDefault();

            if (original_competency == null)
                DbContext.EmployeeCompetencies.InsertOnSubmit(entity);
            else
                original_competency.Rating = entity.Rating;

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Remove the selected skill from a specific employee
        /// </summary>
        /// <param name="employeeCompetencyId"></param>
        public void DeleteEmployeeCompetency(Int32 employeeCompetencyId)
        {
            EmployeeCompetency entity =
                DbContext.EmployeeCompetencies.Where(ec => ec.EmployeeCompetencyId.Equals(employeeCompetencyId)).
                    FirstOrDefault();
            DbContext.EmployeeCompetencies.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion


        #endregion

        #region EmployeeFunctions

        public EmployeeFunction GetEmployeeFunction(Int32 companyID, Int32 employeeFunctionID)
        {
            return
                DbContext.EmployeeFunctions.Where(
                    ef => ef.CompanyId == companyID && ef.EmployeeFunctionId == employeeFunctionID).FirstOrDefault();
        }

        #endregion

        #region EmployeeHistory

        #region functions

        /// <summary>
        /// This method returns the functionHistory of specified employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public IQueryable GetEmployeeFunctionHistories(int companyId, int employeeId)
        {
            var query = from employeeFunctionHistory in DbContext.EmployeeFunctionHistories
                        join employeeFunction in DbContext.EmployeeFunctions on employeeFunctionHistory.EmployeeFunctionId equals
                            employeeFunction.EmployeeFunctionId
                        where employeeFunctionHistory.EmployeeId == employeeId && employeeFunctionHistory.CompanyId == companyId
                        select new 
                        {                             
                            employeeFunctionHistory.EmployeeFunctionHistoryId,
                            employeeFunction.EmployeeFunctionId,
                            EmployeeFunctionName = employeeFunction.Name,
                            employeeFunctionHistory.ModifedDate, 
 
                        };

            return query.Sort("ModifedDate");
        }




        /// <summary>
        /// Return a single row from the table EmployeeFunctionHistory
        /// </summary>
        /// <param name="serviceHistoryId"></param>
        /// <returns></returns>
        public EmployeeFunctionHistory GetEmployeeFunctionHistory(int employeeFunctionHistoryId)
        {
            return
                DbContext.EmployeeFunctionHistories.Where(x => x.EmployeeFunctionHistoryId == employeeFunctionHistoryId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        public void InsertFunctionHistory(EmployeeFunctionHistory entity)
        {
            DbContext.EmployeeFunctionHistories.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic Update Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="original_entity"></param>
        public void UpdateFunctionHistory(EmployeeFunctionHistory entity, EmployeeFunctionHistory original_entity)
        {
            DbContext.EmployeeFunctionHistories.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic Delete Method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteFunctionHistory(EmployeeFunctionHistory entity)
        {
            DbContext.EmployeeFunctionHistories.Attach(entity);
            DbContext.EmployeeFunctionHistories.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region status

        /// <summary>
        /// Returns a DataSet with all information about the history, and the EmployeeStatus name.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public IQueryable GetEmployeeStatusHistories(int companyId, int employeeId)
        {
            var query = from employeeStatusHistory in DbContext.StatusHistories
                        where employeeStatusHistory.EmployeeId == employeeId
                        && employeeStatusHistory.CompanyId == companyId
                        select new
                        {
                            employeeStatusHistory.EmployeeId,
                            employeeStatusHistory.StatusHistoryId,
                            AlienationName = employeeStatusHistory.Alienation != null ? employeeStatusHistory.Alienation.Name : String.Empty, 
                            employeeStatusHistory.AlienationId,
                            employeeStatusHistory.ModifiedDate,
                            employeeStatusHistory.Employee.AlienationDate
                        };

            return query;
        }

        /// <summary>
        /// Return a single row from the table EmployeeStatusHistory
        /// </summary>
        /// <param name="serviceHistoryId"></param>
        /// <returns></returns>
        public StatusHistory GetEmployeeStatusHistory(int statusHistoryId)
        {
            return DbContext.StatusHistories.Where(x => x.StatusHistoryId == statusHistoryId).FirstOrDefault();
        }

        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        public void InsertStatusHistory(StatusHistory entity)
        {
            DbContext.StatusHistories.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic Update Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="original_entity"></param>
        public void UpdateStatusHistory(StatusHistory entity, StatusHistory original_entity)
        {
            DbContext.StatusHistories.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic Delete Method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteStatusHistory(StatusHistory entity)
        {   
            DbContext.StatusHistories.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }


        #endregion

        #endregion

        #region OtherSchool

        /// <summary>
        /// This method returns rows of the side table "OtherSchool" of the same companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<OtherSchool> GetOtherSchool(int companyId)
        {
            return DbContext.OtherSchools.Where(x => x.CompanyId == companyId).Sort("Name");
        }

        public IList GetEmployeeOtherSchool(int companyId, int employeeId)
        {
            var query = from eos in DbContext.EmployeeOtherSchools
                        join os in DbContext.OtherSchools on eos.OtherSchoolId equals os.OtherSchoolId
                        where eos.EmployeeId == employeeId && eos.CompanyId == companyId
                        select new { eos, name = os.Name };
            return query.ToList();
        }

        public EmployeeOtherSchool GetEmployeeOtherSchool(int companyId, int employeeId, int OtherSchoolId)
        {
            return DbContext.EmployeeOtherSchools.Where(x => x.CompanyId == companyId
                                                             && x.EmployeeId == employeeId &&
                                                             x.OtherSchoolId == OtherSchoolId).FirstOrDefault();
        }

        /// <summary>
        /// Returns only the courses that are not inserted on the EmployeeOtherSchool Table
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataReader GetRemainingOtherSchool(int companyId)
        {
            DataManager.Parameters.Add("@companyId", companyId);
            return
                DataManager.ExecuteReader(
                    @"SELECT Name, OtherSchoolId
                                               FROM OtherSchool AS os
                                               WHERE (CompanyId=@companyId) AND (NOT EXISTS
                                               (SELECT os.Name
                                               FROM EmployeeOtherSchool AS eos
                                               WHERE (CompanyId = @companyId) 
                                               AND (OtherSchoolId = os.OtherSchoolId)))");
        }

        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        public void InsertEmployeeOtherSchool(EmployeeOtherSchool entity)
        {
            DbContext.EmployeeOtherSchools.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic Delete Method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEmployeeOtherSchool(EmployeeOtherSchool entity)
        {
            DbContext.EmployeeOtherSchools.Attach(entity);
            DbContext.EmployeeOtherSchools.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion
    }
}