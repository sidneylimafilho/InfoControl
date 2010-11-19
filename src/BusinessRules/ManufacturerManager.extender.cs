using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Security.Cryptography;
using System.Data.Linq;

namespace Vivina.Erp.BusinessRules
{
    public class ManufacturerManager : BusinessManager<InfoControlDataContext>
    {
        public ManufacturerManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method return the data used in the auto complete controls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Recognizable> SearchManufacturer(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            //if (DataManager.CacheCommands[methodName] == null)
            //{
            DataManager.CacheCommands[methodName] =
              CompiledQuery.Compile<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                  (ctx, _companyId, _name, _maximumRows) => (from comp in DbContext.Companies
                                                             join profile in DbContext.LegalEntityProfiles on comp.LegalEntityProfileId
                                                                 equals profile.LegalEntityProfileId
                                                             join manuf in DbContext.Manufacturers on profile.BranchId equals
                                                                 manuf.BranchId
                                                             where (comp.CompanyId == companyId && manuf.Name.Contains(name))
                                                             select new Recognizable(manuf.ManufacturerId.ToString(), manuf.Name)).Take(maximumRows));

            //   IQueryable<string> query = 

            //  DataManager.CacheCommands[methodName] = DbContext.GetCommand(query.Take(maximumRows));
            //}
            var method =
             (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
             DataManager.CacheCommands[methodName];

            return method(DbContext, companyId, name, maximumRows);

            //DataReader reader = DataManager.ExecuteCachedQuery(methodName, companyId, "%" + name + "%");
            //var list = new List<string>();
            //while (reader.Read())
            //{
            //    list.Add(reader.GetString(0));
            //}
            //return list.ToArray();
        }

        /// <summary>
        /// This method return a single Manufacturer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int TryManufacturer(string name)
        {
            var manuf = GetManufacturer(name);
            if (manuf == null)
                return Insert(name);

            return manuf.ManufacturerId;
        }

        /// <summary>
        /// Return all manufacturers of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Manufacturer> GetManufacturerByCompany(int companyId)
        {
            return (from comp in DbContext.Companies
                    join profile in DbContext.LegalEntityProfiles on comp.LegalEntityProfileId equals
                        profile.LegalEntityProfileId
                    join manuf in DbContext.Manufacturers on profile.BranchId equals manuf.BranchId
                    where (comp.CompanyId == companyId)
                    select manuf);
        }

        /// <summary>
        /// This method inserts a new manufacturer
        /// </summary>
        /// <param name="name">can't be null </param>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        private int Insert(string name)
        {
            var manufacturer = new Manufacturer();
            manufacturer.Name = name.Trim().ToUpper();

            //var companyManager = new CompanyManager(this);
            //var company = companyManager.GetCompany(companyId);
            //if (company.LegalEntityProfile.BranchId.HasValue)
            //    manufacturer.BranchId = company.LegalEntityProfile.BranchId.Value;

            DbContext.Manufacturers.InsertOnSubmit(manufacturer);
            DbContext.SubmitChanges();

            return manufacturer.ManufacturerId;
        }

        /// <summary>
        /// Return a single manufacturer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        public Manufacturer GetManufacturer(int manufacturerId)
        {
            return DbContext.Manufacturers.Where(x => x.ManufacturerId == manufacturerId).FirstOrDefault();
        }

        /// <summary>
        /// Return a single manufacturer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        public Manufacturer GetManufacturer(string name)
        {
            return DbContext.Manufacturers.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}