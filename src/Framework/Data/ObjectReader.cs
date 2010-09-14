using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace InfoControl.Data
{
    /// <summary>
    /// Classe criada para monitorar um IDataReader, permitindo assim o uso prático de multiplos DataReaders.
    /// <para>Baseado no Proxy Pattern</para>
    /// </summary>
    public class ObjectReader<T> : IDataReader, IEnumerable
    {
        internal IDataReader _dataReader;
        private int _recordCount = -1;

                

        /// <summary>
        /// Classe criada para monitorar um IDataReader, permitindo assim o uso prático de multiplos DataReaders.
        /// <para>Baseado no Proxy Pattern</para>
        /// </summary>
        /// <param name="dr"></param>
        public ObjectReader(IDataReader dr)
        {
            _dataReader = dr;

        }

        
        /// <summary>
        /// Fecha o DataReader caso chegue no ultimo registro.
        /// </summary>
        /// <param name="hasNext">Indicador de próximo registro</param>
        /// <returns>Indicador de próximo registro</returns>
        internal bool CloseIfEnded(bool hasNext)
        {
            if (!hasNext && !NextResult())
                Close();

            return (hasNext);
        }



        #region IDataReader Members

        /// <summary>
        /// Retorna o numero de registros foram inseridos, atualizados ou deletados na execução da instrução SQL
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return (_dataReader.RecordsAffected);
            }
        }

        /// <summary>
        /// Indica se o DataReader está fechado
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return (_dataReader.IsClosed);
            }
        }

        /// <summary>
        /// Avança o cursor para o próximo bloco de resultados, quando lendo resultados de instruções SQL em lote
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            return (_dataReader.NextResult());
        }

        /// <summary>
        /// Fecha o objeto DataReader
        /// </summary>
        public void Close()
        {
            _dataReader.Close();

        }

        /// <summary>
        /// Avança para o próximo registro, deve ser usado para iniciar a leitura do DataReader
        /// <code>
        /// while(DataReader.Read())
        /// {
        ///		...
        /// }
        /// </code>
        /// </summary>
        /// <returns>Retorna um booleano indicando se tem o próximo registro</returns>
        public bool Read()
        {
            return (CloseIfEnded(_dataReader.Read()));
        }

        /// <summary>
        /// Retorna um valor indicando o aninhamento do registro
        /// </summary>
        public int Depth
        {
            get
            {
                return (_dataReader.Depth);
            }
        }

        /// <summary>
        /// Retorna um DataTable que descreve o metadata das colunas do DataReader
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable()
        {
            return (_dataReader.GetSchemaTable());
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Realiza todas as tarefas associadas com liberar, desalocar, resetar recursos não gerenciados
        /// </summary>
        public void Dispose()
        {
            if (!_dataReader.IsClosed)
                _dataReader.Close();

            _dataReader.Dispose();
        }
        #endregion

        #region IDataRecord Members

        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public int GetInt32(int i)
        {
            return (_dataReader.GetInt32(i));
        }

        /// <summary>
        /// Retorna o conteudo da coluna
        /// </summary>
        public object this[string name]
        {
            get
            {
                return (_dataReader[name]);
            }
        }
        /// <summary>
        /// Retorna o conteudo da coluna
        /// </summary>
        public object this[int i]
        {
            get
            {
                return (_dataReader[i]);
            }
        }
        /*// <summary>
        /// Retorna o conteudo da coluna
        /// </summary>
        object System.Data.IDataRecord.this[int i]
        {
            get
            {
                return(_dataReader[i]);
            }
        }
        */

        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public object GetValue(int i)
        {
            return (_dataReader.GetValue(i));
        }

        /// <summary>
        /// Retorna true ou false indicando se o conteúdo do campo é NULL
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader </param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            return (_dataReader.IsDBNull(i));
        }

        /// <summary>
        /// Retorna um stream de bytes da coluna especificada
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return (_dataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            return (_dataReader.GetByte(i));
        }

        /// <summary>
        /// Retorna o tipo da coluna especificada
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            return (_dataReader.GetFieldType(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            return (_dataReader.GetDecimal(i));
        }

        /// <summary>
        /// Retorna todos os atributos na coleção do registro
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            return (_dataReader.GetValues(values));
        }

        /// <summary>
        /// Retorna o titulo da coluna especificada
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public string GetName(int i)
        {
            return (_dataReader.GetName(i));
        }

        /// <summary>
        /// Retorna o numero de colunas no registro
        /// </summary>
        public int FieldCount
        {
            get
            {
                return (_dataReader.FieldCount);
            }
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public long GetInt64(int i)
        {
            return (_dataReader.GetInt64(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            return (_dataReader.GetDouble(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            return (_dataReader.GetBoolean(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public Guid GetGuid(int i)
        {
            return (_dataReader.GetGuid(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            return (_dataReader.GetDateTime(i));
        }

        /// <summary>
        /// Retorna o indice da coluna pelo seu nome
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetOrdinal(string name)
        {
            return (_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Retorna o tipo da coluna especificada
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public string GetDataTypeName(int i)
        {
            return (_dataReader.GetDataTypeName(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            return (_dataReader.GetFloat(i));
        }

        /// <summary>
        /// Retorna um DataReader para ser usado em dados estruturados remotos 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IDataReader GetData(int i)
        {
            return (_dataReader.GetData(i));
        }

        /// <summary>
        /// Retorna um stream de chars da coluna especificada
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return (_dataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public string GetString(int i)
        {
            return (_dataReader.GetString(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            return (_dataReader.GetChar(i));
        }
        /// <summary>
        /// Retorna o valor do campo especificado
        /// </summary>
        /// <param name="i">Indice da coluna no DataReader</param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            return (_dataReader.GetInt16(i));
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Retorna um enumerator para iterar a coleção
        /// <para>Foi desenvolvido um Enumerator especial para monitorar o DataReader em seu uso</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new ObjectReaderEnumerator<T>(this);
        }        

        #endregion



    }
}
