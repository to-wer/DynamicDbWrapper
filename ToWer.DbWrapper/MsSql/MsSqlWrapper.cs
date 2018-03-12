using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using ToWer.DbWrapper.Converter;
using ToWer.DbWrapper.Exceptions;

namespace ToWer.DbWrapper.MsSql
{
    public class MsSqlWrapper : IDbWrapper
    {
        #region Properties

        private string _connectionString;
        private List<SqlParameter> _standardCommandParameters;

        #endregion Properties

        #region Public methods

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetStandardCommandParameters(List<SqlParameter> parameters)
        {
            if (_standardCommandParameters != null)
            {
                _standardCommandParameters.Clear();
            }
            _standardCommandParameters = parameters;
        }

        public T ReadSingle<T>(string procedureName) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadSingle<T>(_connectionString, procedureName, (List<SqlParameter>)null);
        }

        public T ReadSingle<T>(string procedureName, Dictionary<string, object> parameters) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadSingle<T>(_connectionString, procedureName, parameters);
        }

        public T ReadSingle<T>(string procedureName, List<SqlParameter> parameters) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadSingle<T>(_connectionString, procedureName, parameters);
        }

        public T ReadSingle<T>(string connectionString, string procedureName) where T : class
        {
            return ReadSingle<T>(connectionString, procedureName, (List<SqlParameter>)null);
        }

        public T ReadSingle<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class
        {
            var sqlParameters = new List<SqlParameter>();
            foreach (var param in parameters)
            {
                sqlParameters.Add(new SqlParameter(param.Key, param.Value));
            }
            return ReadSingle<T>(connectionString, procedureName, sqlParameters);
        }

        public T ReadSingle<T>(string connectionString, string procedureName, List<SqlParameter> parameters) where T : class
        {
            var result = ExecuteReader(connectionString, procedureName, parameters);
            var item = result.FirstOrDefault();
            if (item == null) return default(T);
            return DynamicConverter.Convert<T>(item);
        }

        public List<T> ReadList<T>(string procedureName) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadList<T>(_connectionString, procedureName, (List<SqlParameter>)null);
        }

        public List<T> ReadList<T>(string procedureName, Dictionary<string, object> parameters) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadList<T>(_connectionString, procedureName, parameters);
        }

        public List<T> ReadList<T>(string procedureName, List<SqlParameter> parameters) where T : class
        {
            if (string.IsNullOrEmpty(_connectionString)) { throw new ArgumentNullException("_connectionString"); }
            return ReadList<T>(_connectionString, procedureName, parameters);
        }

        public List<T> ReadList<T>(string connectionString, string procedureName) where T : class
        {
            return ReadList<T>(connectionString, procedureName, (List<SqlParameter>)null);
        }

        public List<T> ReadList<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class
        {
            var sqlParameters = new List<SqlParameter>();
            foreach(var item in parameters)
            {
                sqlParameters.Add(new SqlParameter(item.Key, item.Value));
            }
            return ReadList<T>(connectionString, procedureName, sqlParameters);
        }

        public List<T> ReadList<T>(string connectionString, string procedureName, List<SqlParameter> parameters) where T : class
        {
            var items = ExecuteReader(connectionString, procedureName, parameters);
            var result = new List<T>();
            foreach (var item in items)
            {
                result.Add(DynamicConverter.Convert<T>(item));
            }
            return result;
        }

        public void ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("_connectionString");
            ExecuteNonQuery(_connectionString, procedureName, parameters);
        }

        public void ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            if (string.IsNullOrEmpty(_connectionString)) throw new ArgumentNullException("_connectionString");
            ExecuteNonQuery(_connectionString, procedureName, parameters);
        }

        public void ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    sqlParameters.Add(new SqlParameter(param.Key, param.Value));
                }
            }
            ExecuteNonQuery(connectionString, procedureName, sqlParameters);
        }

        public void ExecuteNonQuery(string connectionString, string procedureName, List<SqlParameter> parameters)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = procedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (_standardCommandParameters != null)
                {
                    foreach (var param in _standardCommandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    if (!string.IsNullOrEmpty(cmd.Parameters["@ErrNum"].Value.ToString()) && cmd.Parameters["@ErrNum"].Value.ToString() != "0")
                    {
                        var errNum = (int)cmd.Parameters["@ErrNum"].Value;
                        var errText = cmd.Parameters["@ErrText"].Value.ToString();
                        throw new SqlCmdException()
                        {
                            ErrNum = errNum,
                            ErrText = errText
                        };
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public void ExecuteNonQuery<T>(string connectionString, string procedureName, T item) where T : class
        {
            throw new NotImplementedException();
        }

        #endregion Public methods

        #region Private methods

        private List<dynamic> ExecuteReader(string connectionString, string procedureName, List<SqlParameter> parameters)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = procedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (_standardCommandParameters != null)
                {
                    foreach (var param in _standardCommandParameters)
                    {
                        //if (cmd.Parameters.Contains(param.ParameterName)) { throw new Exception(param.ParameterName); }
                        var newParam = new SqlParameter(param.ParameterName, param.SqlDbType, param.Size)
                        {
                            Direction = param.Direction,
                            Value = param.Value
                        };
                        cmd.Parameters.Add(newParam);
                    }
                }
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (cmd.Parameters.Contains(param.ParameterName)) { throw new Exception(param.ParameterName); }
                        cmd.Parameters.Add(param);
                    }
                }
                try
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var result = new List<dynamic>();

                        while (reader.Read())
                        {
                            var newItem = new ExpandoObject() as IDictionary<string, object>;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var type = reader[i].GetType();
                                newItem.Add(reader.GetName(i), type != typeof(DBNull) ? reader[i] : null);
                            }
                            result.Add(newItem);
                        }
                        //if (result.Count == 1)
                        //{
                        //    return result.First();
                        //}
                        return result;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion Private methods
    }
}
