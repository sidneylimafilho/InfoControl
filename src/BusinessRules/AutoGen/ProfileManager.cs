using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class ProfileManager : BusinessManager<InfoControlDataContext>
    {
        public ProfileManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Profiles.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Profile> GetAllProfiles()
        {
            //
            return DbContext.Profiles;
        }

        /// <summary>
        /// This method gets record counts of all Profiles.
        /// Do not change this method.
        /// </summary>
        public int GetAllProfilesCount()
        {
            return GetAllProfiles().Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Profile entity)
        {
            //
            DbContext.Profiles.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Profile entity)
        {
            entity.ModifiedDate = DateTime.Now;
            DbContext.Profiles.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name="entity"></param>
        public void InsertLegalEntityProfile(LegalEntityProfile entity)
        {
            entity.ModifiedDate = DateTime.Now;
            DbContext.LegalEntityProfiles.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Profile original_entity, Profile entity)
        {
            //
            original_entity.CopyPropertiesFrom(entity);
            entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method saves the profile
        /// </summary>
        /// <param name="profile"> can't be null</param>
        public void SaveProfile(Profile profile)
        {
            //
            // Insert
            //
            if (profile.ProfileId == 0)
            {
                Insert(profile);
                return;
            }

            //
            // Update
            //
            var originalProfile = GetProfile(profile.ProfileId);
            Update(originalProfile, profile);
        }


        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void SaveLegalEntityProfile(LegalEntityProfile entity)
        {
            LegalEntityProfile originalEntity = GetLegalEntityProfile(entity.LegalEntityProfileId);
            originalEntity.CopyPropertiesFrom(entity);
            originalEntity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns a profile of the a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Profile GetProfileByUser(int userId)
        {
            Profile profile = (from users in DbContext.Users
                               join prof in DbContext.Profiles on users.ProfileId equals prof.ProfileId
                               where users.UserId == userId
                               select prof).FirstOrDefault();
            return profile;
        }
    }
}