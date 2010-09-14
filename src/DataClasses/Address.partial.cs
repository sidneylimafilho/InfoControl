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
    public partial class Address : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public string City { 
            get { return NeighborhoodEntity.City.Name; }
            set { NeighborhoodEntity.City.Name = value; }
        }

        public int CityId { 
            get { return NeighborhoodEntity.City.CityId; } 
        }

        public string StateId { 
            get { return NeighborhoodEntity.City.StateId; }
            set { NeighborhoodEntity.City.StateId = value; }
        }

        public string State { 
            get { return NeighborhoodEntity.City.State.Name; } 
        }

        public string Neighborhood { 
            get { return NeighborhoodEntity.Name; }
            set {
                if (NeighborhoodEntity == null)
                    NeighborhoodEntity = new Neighborhood() { City = new City() };

                NeighborhoodEntity.Name = value;
            }
        }

        private EntityRef<Neighborhood> _neighborhoodEntity;

        [Association(Storage = "_neighborhoodEntity", ThisKey = "NeighborhoodId", IsForeignKey = true)]
        public Neighborhood NeighborhoodEntity
        {
            get {
                if (_neighborhoodEntity.Entity == null)
                    _neighborhoodEntity.Entity = new Neighborhood();
                return _neighborhoodEntity.Entity; }
            set
            {
                if ((this._neighborhoodEntity.Entity != value))
                {
                    this._neighborhoodEntity.Entity = value;                   
                }
            }
        }

        
    }
}
