using System;
using System.Data;
using System.Collections;
using System.Reflection;

namespace InfoControl.Data
{
	/// <summary>
	/// DataParameterCollection contains DataCommand's IDbDataParameters
	/// </summary>
	public class DataParameterCollection: IDataParameterCollection, ICloneable
	{
		private IDbCommand _command;
		private IDataParameterCollection _parameters;	
		private Type _parameterDbType;
	

		internal DataParameterCollection(IDbCommand command)
		{
			_command = command;
			_parameters = command.Parameters;	
			_parameters.Clear();
			_parameterDbType = command.CreateParameter().GetType();	
			
		}


		
		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection,  byte precision, byte scale, string sourceColumn, object paramValue) 
		{
			
			IDbDataParameter param = _command.CreateParameter();

			param.ParameterName = paramName;	

			if(paramType is DbType) 
			{
				param.DbType = (DbType)paramType;
			}
			
			param.Direction = paramDirection;

			if(sourceColumn != null && sourceColumn != string.Empty)
				param.SourceColumn = sourceColumn;


			if(paramSize > 0)
				param.Size = paramSize;
			if(precision > 0)
				param.Precision = precision;
			if(scale > 0)
				param.Scale = scale;

			
			if(paramValue != null && param.Value != DBNull.Value) 					
				param.Value	= paramValue;

			_parameters.Add(param);			
			

			return param;
		}

		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection, string sourceColumn, object paramValue) 
		{
			return this.Add( paramName, paramType, paramSize, paramDirection, 0, 0, sourceColumn, paramValue);
		}

		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection,  byte precision, byte scale, object paramValue)
		{
			return this.Add( paramName, paramType, paramSize, paramDirection, precision, scale, null, paramValue);
		}
				
		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection,  byte precision, object paramValue) 
		{
			return this.Add( paramName, paramType, paramSize, paramDirection, precision, 0, null, paramValue);
		}
		
		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize, ParameterDirection paramDirection, object paramValue) 
		{
			return this.Add( paramName, paramType, paramSize, paramDirection, 0, 0, null, paramValue);
		}
		
		public IDbDataParameter Add(string paramName, Enum paramType, int paramSize,  object paramValue) 
		{
			return this.Add( paramName, paramType, paramSize, ParameterDirection.Input, 0, 0, null, paramValue);
		}
		
		public IDbDataParameter Add(string paramName, Enum paramType, ParameterDirection paramDirection, object paramValue)
		{
			return this.Add( paramName, paramType, 0, paramDirection, 0, 0, null, paramValue);
		}
	
		public IDbDataParameter Add(string paramName, Enum paramType, object paramValue)
		{
			return this.Add( paramName, paramType, 0, ParameterDirection.Input, 0, 0, null, paramValue);
		}

		public IDbDataParameter Add(string paramName, object paramValue, object equalsIsNull)
		{	
			if(paramValue.Equals(equalsIsNull))
				paramValue = DBNull.Value;
			
			return this.Add( paramName, null, 0, ParameterDirection.Input, 0, 0, null, paramValue);
		}

		public IDbDataParameter Add(string paramName, object paramValue)
		{	
			return this.Add( paramName, null, 0, ParameterDirection.Input, 0, 0, null, paramValue);
		}
	
		

		public IDbDataParameter this[int parameterIndex]
		{
			get
			{
				return (IDbDataParameter)_parameters[parameterIndex];
			}
			set
			{
				_parameters[parameterIndex] = value;
			}
		}

        public IDbDataParameter this[string parameterName]
        {
            get
            {
                return (IDbDataParameter)_parameters[_parameters.IndexOf(parameterName)];
            }
            set
            {
                _parameters[_parameters.IndexOf(parameterName)] = value;
            }
        }

		
		#region IDataParameterCollection Members

		object IDataParameterCollection.this[string parameterName]
		{
			get
			{
				return _parameters[parameterName];
			}
			set
			{
				_parameters[parameterName] = value; 
			}
		}

		public void RemoveAt(string parameterName)
		{
			_parameters.RemoveAt(parameterName);
		}

		public bool Contains(string parameterName)
		{
			return _parameters.Contains(parameterName);
		}

		public int IndexOf(string parameterName)
		{
			return _parameters.IndexOf(parameterName);
		}

		#endregion

		#region IList Members

		public bool IsReadOnly
		{
			get
			{
				return _parameters.IsReadOnly;
			}
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return _parameters[index];
			}
			set
			{
				_parameters[index] = value; 
			}
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			_parameters.RemoveAt(index);
		}

		public void Insert(int index, object value)
		{
			_parameters.Insert(index, value);
		}

		public void Remove(object value)
		{
			_parameters.Remove(value);
		}

		bool System.Collections.IList.Contains(object value)
		{
			return _parameters.Contains(value);
		}

		public void Clear()
		{
			_parameters.Clear();
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return _parameters.IndexOf(value);
		}

		int System.Collections.IList.Add(object value)
		{
			return _parameters.Add(value);
		}

		public bool IsFixedSize
		{
			get
			{
				return _parameters.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return _parameters.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return _parameters.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_parameters.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _parameters.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _parameters.GetEnumerator();
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return new DataParameterCollection(_command);
		}

		#endregion
		

	}
}
