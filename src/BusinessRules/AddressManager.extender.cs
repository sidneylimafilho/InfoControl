using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class AddressManager : BusinessManager<InfoControlDataContext>
    {
        public AddressManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// Returns the complete adress by the postal code
        /// </summary>
        /// <param name="postalCode"></param>
        /// <returns></returns>
        public Address GetAddress(string postalCode)
        {
            IQueryable<Address> query = from address in DbContext.Addresses
                                        join ng in DbContext.Neighborhoods on address.NeighborhoodId equals
                                            ng.NeighborhoodId
                                        join ct in DbContext.Cities on ng.CityId equals ct.CityId
                                        join st in DbContext.States on ct.StateId equals st.StateId
                                        where address.PostalCode == postalCode
                                        select address;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get Neighborhood by City
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Address SaveAddress(string postalCode, string name, int neighborhoodId)
        {
            Address address = GetAddress(postalCode);
            if (address == null)
            {
                address = new Address
                              {
                                  Name = name,
                                  PostalCode = postalCode,
                                  NeighborhoodId = neighborhoodId
                              };

                DbContext.Addresses.InsertOnSubmit(address);
            }
            else
            {
                address.Name = name;
                address.NeighborhoodId = neighborhoodId;
            }
            DbContext.SubmitChanges();

            return address;
        }

        /// <summary>
        /// Get Neighborhood by City
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Neighborhood SaveNeighborhood(string name, int cityId)
        {
            Neighborhood neighborhood = GetNeighborhood(name, cityId);
            if (neighborhood == null)
            {
                neighborhood = new Neighborhood
                                   {
                                       Name = name,
                                       CityId = cityId
                                   };

                DbContext.Neighborhoods.InsertOnSubmit(neighborhood);
                DbContext.SubmitChanges();
            }

            return neighborhood;
        }

        /// <summary>
        /// This method returns a specified neighborhood
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public Neighborhood GetNeighborhood(string name, int cityId)
        {
            return DbContext.Neighborhoods.Where(ct => ct.Name == name && ct.CityId == cityId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a neighborhood by name from db 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Neighborhood GetNeighborhood(string name)
        {
            return DbContext.Neighborhoods.Where(city => city.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// This method returns the acronym of a state name
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public string GetAcronymState(string stateName)
        {
            var acronymState = DbContext.States.Where(state => state.Name.Equals(stateName)).FirstOrDefault().StateId;
            return acronymState;
        }

        /// <summary>
        /// This method returns the state by name
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public string GetState(string stateName)
        {
            var retrievedState = DbContext.States.Where(state => state.StateId == stateName.ToUpper() || state.Name == stateName.ToUpper()).FirstOrDefault();

            if (retrievedState != null)
                return retrievedState.StateId;
            else
                return string.Empty;
        }

        /// <summary>
        /// Save City by State
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public City SaveCity(string name, string stateId)
        {
            City city = GetCity(name, stateId);
            if (city == null)
            {
                city = new City
                           {
                               Name = name,
                               StateId = stateId
                           };
                DbContext.Cities.InsertOnSubmit(city);
                DbContext.SubmitChanges();
            }

            return city;
        }

        /// <summary>
        /// This method retrieves a specified city 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public City GetCity(string name, string stateId)
        {
            return DbContext.Cities.Where(ct => ct.Name == name && ct.StateId == stateId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a city by name from db
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public City GetCity(string name)
        {
            return DbContext.Cities.Where(city => city.Name == name.ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// This method inserts a new city in db
        /// </summary>
        /// <param name="city">can't be null</param>
        public void InsertCity(City city)
        {
            DbContext.Cities.InsertOnSubmit(city);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new neighborhood in db
        /// </summary>
        /// <param name="neighborhood">can't be null</param>
        public void InsertNeighborhood(Neighborhood neighborhood)
        {
            DbContext.Neighborhoods.InsertOnSubmit(neighborhood);
            DbContext.SubmitChanges();
        }
    }
}