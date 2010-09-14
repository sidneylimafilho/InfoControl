using System;
using System.Data;

namespace InfoControl.Data
{
	/// <summary>
	/// Cria um Parameter genérico
	/// </summary>
	public class DataParameter: IDbDataParameter, ICloneable
	{
		#region Variables
		private IDbDataParameter _param;
		#endregion
		
		#region Properties

		#endregion

		#region Constructor
		public DataParameter(IDbDataParameter param)
		{
			_param = param;
		}
		#endregion

		#region Methods

		#endregion

		#region IDbDataParameter Members
		public byte Precision
		{
			get
			{
				return _param.Precision;
			}
			set
			{
				_param.Precision = value;
			}
		}

		public byte Scale
		{
			get
			{
				return _param.Scale;
			}
			set
			{
				_param.Scale = value;
			}
		}

		public int Size
		{
			get
			{
				return _param.Size;
			}
			set
			{
				_param.Size = value;
			}
		}

		#endregion

		#region IDataParameter Members
		public System.Data.ParameterDirection Direction
		{
			get
			{
				return _param.Direction;
			}
			set
			{
				_param.Direction = value;
			}
		}

		public System.Data.DbType DbType
		{
			get
			{
				return _param.DbType;
			}
			set
			{
				_param.DbType = value;
			}
		}

		public object Value
		{
			get
			{
				return _param.Value;
			}
			set
			{
				_param.Value = value;
			}
		}

		public bool IsNullable
		{
			get
			{
				return _param.IsNullable;
			}
		}

		public System.Data.DataRowVersion SourceVersion
		{
			get
			{
				return _param.SourceVersion;
			}
			set
			{
				_param.SourceVersion = value;
			}
		}

		public string ParameterName
		{
			get
			{
				return _param.ParameterName;
			}
			set
			{
				_param.ParameterName = value;
			}
		}

		public string SourceColumn
		{
			get
			{
				return _param.SourceColumn;
			}
			set
			{
				_param.SourceColumn = value;
			}
		}

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Cria uma instancia do parameter indicado e copia seus valores
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			IDbDataParameter param = Activator.CreateInstance(_param.GetType()) as IDbDataParameter;
			param.Precision = _param.Precision;
			param.Scale = _param.Scale;
			param.Size = _param.Size;
			param.Direction = _param.Direction;
			param.Value = _param.Value;
			param.SourceVersion = _param.SourceVersion;
			param.DbType = _param.DbType;
			param.ParameterName = _param.ParameterName;
			param.SourceColumn = _param.SourceColumn;

			return param;
		}

		#endregion
	}
}
