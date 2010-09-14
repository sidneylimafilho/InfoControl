using System;
using System.Collections;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Auditing;
using Vivina.Erp.DataClasses;
using Event=Vivina.Erp.DataClasses.Event;

namespace Vivina.Erp.BusinessRules
{
    public class EventManager : BusinessManager<InfoControlDataContext>
    {
        public EventManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Events.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Event> GetAllEvents()
        {
            return DbContext.Events.Where(e => e.EventStatusId != EventStatus.Closed).AsQueryable();
        }

        /// <summary>
        /// This method gets record counts of all Events.
        /// Do not change this method.
        /// </summary>
        public int GetAllEventsCount()
        {
            return GetAllEvents().Count();
        }

        /// <summary>
        /// This method retrieves a single Event.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=EventId>EventId</param>
        public Event GetEvent(Int32 EventId)
        {
            return DbContext.Events.Where(x => x.EventId == EventId).FirstOrDefault();
        }

        /// <summary>
        /// This methos return a single event
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public DataRow GetEventAsDataRow(Int32 EventId)
        {
            var query = from events in DbContext.Events
                        join eventStatus in DbContext.EventStatus on events.EventStatusId equals
                            eventStatus.EventStatusId into gEventStatuses
                        from eventStatus in gEventStatuses.DefaultIfEmpty()
                        join users in DbContext.Users on events.UserId equals users.UserId
                        join profiles in DbContext.Profiles on users.ProfileId equals profiles.ProfileId into gProfiles
                        from profiles in gProfiles.DefaultIfEmpty()
                        join companyUser in DbContext.CompanyUsers on users.UserId equals companyUser.UserId
                        join companies in DbContext.Companies on companyUser.CompanyId equals companies.CompanyId
                        join legalEntityProfile in DbContext.LegalEntityProfiles on companies.LegalEntityProfileId
                            equals legalEntityProfile.LegalEntityProfileId
                        join application in DbContext.Applications on events.ApplicationId equals
                            application.ApplicationId
                        where (events.EventId == EventId) && companyUser.IsMain
                        select new
                                   {
                                       events.EventId,
                                       events.EventType,
                                       events.Name,
                                       events.Message,
                                       events.Source,
                                       events.StackTrace,
                                       events.Path,
                                       events.RefererUrl,
                                       events.HelpLink,
                                       events.TargetSite,
                                       events.CurrentDate,
                                       events.ExceptionCode,
                                       events.ApplicationId,
                                       events.UserId,
                                    //   events.TechnicalUserId,
                                       companies.CompanyId,
                                       events.EventStatusId,
                                       events.Version,
                                       events.Module,
                                       events.Rating,
                                       EventStatusName = eventStatus.Name,
                                       legalEntityProfile.CompanyName,
                                       UserName = profiles.Name,
                                       ApplicationName = application.Name
                                   };
            return query.ToDataTable().Rows[0];
        }

        /// <summary>
        /// This method retrieves Event by Application.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ApplicationId>ApplicationId</param>
        public IQueryable<Event> GetEventByApplication(Int32 ApplicationId)
        {
            return DbContext.Events.Where(x => x.ApplicationId == ApplicationId);
        }

        /// <summary>
        /// This method retrieves Event by User.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=UserId>UserId</param>
        public IQueryable<Event> GetEventByUser(Int32 UserId)
        {
            return DbContext.Events.Where(x => x.UserId == UserId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Events filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetEvents(string tableName, Int32 Application_ApplicationId, Int32 User_UserId,
                               string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Event> x = GetFilteredEvents(tableName, Application_ApplicationId, User_UserId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "EventId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Event> GetFilteredEvents(string tableName, Int32 Application_ApplicationId, Int32 User_UserId)
        {
            switch (tableName)
            {
                case "Application_Events":
                    return GetEventByApplication(Application_ApplicationId);
                case "User_Events":
                    return GetEventByUser(User_UserId);
                default:
                    return GetAllEvents();
            }
        }

        /// <summary>
        /// This method gets records counts of all Events filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetEventsCount(string tableName, Int32 Application_ApplicationId, Int32 User_UserId)
        {
            IQueryable<Event> x = GetFilteredEvents(tableName, Application_ApplicationId, User_UserId);
            return x.Count();
        }

#warning este método possui código duplicado pois não é possivel delegar sua execução visto que o tipo de retorno não suporta as condições usadas neste método
        /// <summary>
        /// this method return all events by eventType
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        private IQueryable GetEventsByEventType(Int32? eventType, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            var queryEvents = from events in DbContext.Events
                              //join users in DbContext.Users on events.TechnicalUserId equals users.UserId into
                              //    gTechnicalUsers
                              //from technicalUser in gTechnicalUsers.DefaultIfEmpty()
                              join postUsers in DbContext.Users on events.UserId equals postUsers.UserId into gPostUsers
                              from postUser in gPostUsers.DefaultIfEmpty()
                              join postProfiles in DbContext.Profiles on postUser.ProfileId equals
                                  postProfiles.ProfileId into gPostProfiles
                              from postProfile in gPostProfiles.DefaultIfEmpty()
                              //join technicalProfiles in DbContext.Profiles on technicalUser.ProfileId equals
                              //    technicalProfiles.ProfileId into gTechnicalProfiles
                              //from technicalProfile in gTechnicalProfiles.DefaultIfEmpty()
                              join eventsStatus in DbContext.EventStatus on events.EventStatusId equals
                                  eventsStatus.EventStatusId into gEventsStatus
                              from eventStatus in gEventsStatus.DefaultIfEmpty()
                              select new
                                         {
                                             events.EventId,
                                             events.EventType,
                                             events.Name,
                                             events.Message,
                                             events.Source,
                                             events.StackTrace,
                                             events.Path,
                                             events.RefererUrl,
                                             events.HelpLink,
                                             events.TargetSite,
                                             events.CurrentDate,
                                             events.ExceptionCode,
                                             events.ApplicationId,
                                             events.UserId,
                                           //  events.TechnicalUserId,
                                          //   events.CompanyId,
                                             EventStatusId = events.EventStatusId ?? 0,
                                           //  TechnicalUserName = technicalProfile.Name,
                                             EventStatusName = eventStatus.Name,
                                             UserName = postProfile.Name,
                                             UserNameEmail = postProfile.Email,
                                             userNamePhone = postProfile.Phone
                                         };
            if (eventType.HasValue)
                queryEvents = queryEvents.Where(e => e.EventType == eventType && e.EventStatusId != EventStatus.Closed);

            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "CurrentDate DESC";

            return queryEvents.SortAndPage(sortExpression, startRowIndex, maximumRows, "CurrentDate");
        }

        /// <summary>
        /// This method return events by event type and thecnical user
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetEventsByEventTypeAndTechnicalUser(Int32? eventStatusId, Int32? eventType,
                                                                DateTime beginDate, DateTime endDate, string sortExpression,
                                                               int startRowIndex, int maximumRows)
        {
            var queryEvents = from events in DbContext.Events
                              //join users in DbContext.Users on events.TechnicalUserId equals users.UserId into
                              //    gTechnicalUsers
                              //from technicalUser in gTechnicalUsers.DefaultIfEmpty()
                              join postUsers in DbContext.Users on events.UserId equals postUsers.UserId into gPostUsers
                              from postUser in gPostUsers.DefaultIfEmpty()
                              join postProfiles in DbContext.Profiles on postUser.ProfileId equals
                                  postProfiles.ProfileId into gPostProfiles
                              from postProfile in gPostProfiles.DefaultIfEmpty()
                              //join technicalProfiles in DbContext.Profiles on technicalUser.ProfileId equals
                              //    technicalProfiles.ProfileId into gTechnicalProfiles
                              //from technicalProfile in gTechnicalProfiles.DefaultIfEmpty()
                              join eventsStatus in DbContext.EventStatus on events.EventStatusId equals
                                  eventsStatus.EventStatusId into gEventsStatus
                              from eventStatus in gEventsStatus.DefaultIfEmpty()
                              where ((events.CurrentDate >= beginDate) && (events.CurrentDate <= endDate))
                              select new
                                         {
                                             events.EventId,
                                             events.EventType,
                                             events.Name,
                                             events.Message,
                                             events.Source,
                                             events.StackTrace,
                                             events.Path,
                                             events.RefererUrl,
                                             events.HelpLink,
                                             events.TargetSite,
                                             events.CurrentDate,
                                             events.ExceptionCode,
                                             events.ApplicationId,
                                             events.UserId,
                                          //   events.TechnicalUserId,
                                             //events.CompanyId,
                                             events.Rating,
                                             EventStatusId = events.EventStatusId ?? 0,
                                         //    TechnicalUserName = technicalProfile.Name,
                                             EventStatusName = eventStatus.Name,
                                             UserName = postProfile.Name,
                                             UserNameEmail = postProfile.Email,
                                             userNamePhone = postProfile.Phone
                                         };

            if (eventStatusId.HasValue)
                queryEvents = queryEvents.Where(e => e.EventStatusId == eventStatusId);

            if (eventType.HasValue)
                queryEvents = queryEvents.Where(e => e.EventType == eventType);

            //if (technicalUserId.HasValue)
            //    queryEvents = queryEvents.Where(e => e.TechnicalUserId == (Int32) technicalUserId);

            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "CurrentDate DESC";

            return queryEvents.SortAndPage(sortExpression, startRowIndex, maximumRows, "CurrentDate");
        }

        /// <summary>
        /// this is the countMethod of GetEventsByEventType
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetEventsByEventTypeCount(Int32? eventType, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            return
                GetEventsByEventType(eventType, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        #region CRUDMethods

#warning este método não está da maneira correta,retirar o get
        /// <summary>
        /// this method resolve(set isClosed) in event
        /// </summary>
        /// <param name="entity"></param>
        public void ResolveEvent(Event entity)
        {
            //entity = GetEvent(entity.EventId);
            //if (!entity.TechnicalUserId.HasValue)
            //    entity.TechnicalUserId = technicalUserId;
            entity.EventStatusId = EventStatus.Closed;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Event entity)
        {
            DbContext.Events.Attach(entity);
            DbContext.Events.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Event entity)
        {
            entity.EventStatusId = EventStatus.Opened;
            entity.CurrentDate = DateTime.Now;
            DbContext.Events.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Event original_entity, Event entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region eventMethodsByType

        /// <summary>
        /// this method return all events
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetEvents()
        {
            var queryEvents = from events in DbContext.Events
                              //join users in DbContext.Users on events.TechnicalUserId equals users.UserId into gUsers
                              //from users in gUsers.DefaultIfEmpty()
                              join postUsers in DbContext.Users on events.UserId equals postUsers.UserId into gPostUsers
                              from postUsers in gPostUsers.DefaultIfEmpty()
                              join postProfiles in DbContext.Profiles on postUsers.ProfileId equals
                                  postProfiles.ProfileId into gPostProfiles
                              from postProfiles in gPostProfiles.DefaultIfEmpty()
                              //join profiles in DbContext.Profiles on users.ProfileId equals profiles.ProfileId into
                              //    gProfiles
                              //from profiles in gProfiles.DefaultIfEmpty()
                              join eventsStatus in DbContext.EventStatus on events.EventStatusId equals
                                  eventsStatus.EventStatusId into gEventsStatus
                              from eventStatus in gEventsStatus.DefaultIfEmpty()
                              where eventStatus.EventStatusId != EventStatus.Closed
                              select new
                                         {
                                             events.EventId,
                                             events.EventType,
                                             events.Name,
                                             events.Message,
                                             events.Source,
                                             events.StackTrace,
                                             events.Path,
                                             events.RefererUrl,
                                             events.HelpLink,
                                             events.TargetSite,
                                             events.CurrentDate,
                                             events.ExceptionCode,
                                             events.ApplicationId,
                                             events.UserId,
                                          //   events.TechnicalUserId,
                                        //     events.CompanyId,
                                             events.EventStatusId,
                                             events.Rating,
                                            // TechnicalUserName = profiles.Name,
                                             EventStatusName = eventStatus.Name,
                                             UserName = postProfiles.Name
                                         };
            return queryEvents.AsQueryable().SortAndPage("", 0, 0, "");
        }

        /// <summary>
        /// this method return all warning events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetWarningEvents(Int32? eventStatusId, DateTime beginDate,
                                           DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetEventsByEventTypeAndTechnicalUser(eventStatusId, (Int32) EventType.Warning, beginDate, endDate,
                sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        ///  this method return quantity of warning events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetWarningEventsCount(Int32? eventStatusId, DateTime beginDate,
                                           DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetWarningEvents(eventStatusId,  beginDate, endDate, sortExpression, startRowIndex,
                                 maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all information events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetInformationEvents(Int32? eventStatusId,DateTime beginDate,
                                               DateTime endDate, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            return GetEventsByEventTypeAndTechnicalUser(eventStatusId, (Int32) EventType.Information, 
                                                        beginDate, endDate, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return quantity of information events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetInformationEventsCount(Int32? eventStatusId,  DateTime beginDate,
                                               DateTime endDate, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            return
                GetInformationEvents(eventStatusId, beginDate, endDate, sortExpression, startRowIndex,
                                     maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all sugestion events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSugestionEvents(Int32? eventStatusId,  DateTime beginDate,
                                             DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetEventsByEventTypeAndTechnicalUser(eventStatusId, (Int32) EventType.Sugestion, 
                                                        beginDate, endDate, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return quantity of sugestion events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetSugestionEventsCount(Int32? eventStatusId,  DateTime beginDate,
                                             DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetSugestionEvents(eventStatusId,  beginDate, endDate, sortExpression, startRowIndex,
                                   maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetOpenEvents(Int32? eventStatusId,  DateTime beginDate,
                                        DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetEventsByEventTypeAndTechnicalUser(eventStatusId, null, beginDate, endDate,
                                                        sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count Method of GetOpenEvents
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetOpenEventsCount(Int32? eventStatusId,  DateTime beginDate,
                                        DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetOpenEvents(eventStatusId, beginDate, endDate, sortExpression, startRowIndex,
                              maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// This method return all events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetAllEvents( string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Event> query = GetAllEvents();

            //if (technicalUserId.HasValue)
            //    query = query.Where(e => e.TechnicalUserId == technicalUserId);

            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "CurrentDate DESC";

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "CurrentDate");
        }

        /// <summary>
        /// this method return quantity de events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetAllEventsCount(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllEvents( sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all events generated by error
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetErrorEvents(Int32? eventStatusId,  DateTime beginDate,
                                         DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetEventsByEventTypeAndTechnicalUser(eventStatusId, (Int32) EventType.Error, beginDate, endDate,
                                                       sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return quantity of error events
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetErrorEventsCount(Int32? eventStatusId,  DateTime beginDate,
                                         DateTime endDate, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetErrorEvents(eventStatusId, beginDate, endDate, sortExpression, startRowIndex,
                               maximumRows).Cast<IQueryable>().Count();
        }

#warning este método possui código duplicado pois não é possivel delegar sua execução visto que o tipo de retorno não suporta as condições usadas neste método

        /// <summary>
        /// this method return all sugestions(closed,open...)
        /// </summary>
        /// <param name="technicalUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetAllSugestionEvents(Int32 eventStatusId, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            var queryEvents = from events in DbContext.Events
                              //join users in DbContext.Users on events.TechnicalUserId equals users.UserId into gUsers
                              //from users in gUsers.DefaultIfEmpty()
                              join postUsers in DbContext.Users on events.UserId equals postUsers.UserId into gPostUsers
                              from postUsers in gPostUsers.DefaultIfEmpty()
                              join postProfiles in DbContext.Profiles on postUsers.ProfileId equals
                                  postProfiles.ProfileId into gPostProfiles
                              from postProfiles in gPostProfiles.DefaultIfEmpty()
                              //join profiles in DbContext.Profiles on users.ProfileId equals profiles.ProfileId into
                              //    gProfiles
                              //from profiles in gProfiles.DefaultIfEmpty()
                              join eventsStatus in DbContext.EventStatus on events.EventStatusId equals
                                  eventsStatus.EventStatusId into gEventsStatus
                              from eventStatus in gEventsStatus.DefaultIfEmpty()
                              select new
                                         {
                                             events.EventId,
                                             events.EventType,
                                             events.Name,
                                             events.Message,
                                             events.Source,
                                             events.StackTrace,
                                             events.Path,
                                             events.RefererUrl,
                                             events.HelpLink,
                                             events.TargetSite,
                                             events.CurrentDate,
                                             events.ExceptionCode,
                                             events.ApplicationId,
                                             events.UserId,
                                          //   events.TechnicalUserId,
                                           //  events.CompanyId,
                                             events.EventStatusId,
                                           //  TechnicalUserName = profiles.Name,
                                             EventStatusName = eventStatus.Name,
                                             UserName = postProfiles.Name
                                         };
            return
                queryEvents.Where(
                    e => e.EventType == Convert.ToInt32(EventType.Sugestion) && e.EventStatusId == eventStatusId).
                    SortAndPage(sortExpression, startRowIndex, maximumRows, "CurrentDate").OrderByDescending(
                    e => e.CurrentDate);
        }

        #endregion

        #region eventStatus

        /// <summary>
        /// This method return all event status
        /// </summary>
        /// <returns></returns>
        public IQueryable<EventStatus> GetAllEventStatus()
        {
            return DbContext.EventStatus;
        }

        #endregion
    }
}