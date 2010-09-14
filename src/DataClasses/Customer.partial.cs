using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace Vivina.Erp.DataClasses
{
    public partial class Customer : INotifyPropertyChanging, INotifyPropertyChanged
    {

        public string Name
        {
            get
            {
                return (Profile != null) ? Profile.Name : LegalEntityProfile.CompanyName;
            }
        }

        public string Identification
        {
            get
            {
                return (Profile != null) ? Profile.CPF : LegalEntityProfile.CNPJ;
            }
        }
        
        public string Email
        {
            get
            {
                return (Profile != null) ? Profile.Email : LegalEntityProfile.Email;
            }
        }

        public string Phone
        {
            get
            {
                return (Profile != null) ? Profile.Phone : LegalEntityProfile.Phone;
            }
        }

        public Address Address
        {
            get
            {
                return (Profile != null) ? Profile.Address : LegalEntityProfile.Address;
            }
        }

		public string AddressComp
		{
			get
			{
				return (Profile != null) ? Profile.AddressComp : LegalEntityProfile.AddressComp;
			}
		}

		public string AddressNumber
		{
			get
			{
				return (Profile != null) ? Profile.AddressNumber : LegalEntityProfile.AddressNumber;
			}
		}
    }
}
