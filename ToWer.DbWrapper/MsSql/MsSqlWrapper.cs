using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using ToWer.DbWrapper.Converter;

namespace ToWer.DbWrapper.MsSql
{
    public class MsSqlWrapper : IDbWrapper
    {
        #region Public methods
        
        public T ReadSingle<T>(string connectionString, string procedureName) where T : class
        {
            return ReadSingle<T>(connectionString, procedureName, null);
        }

        public T ReadSingle<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class
        {
            var result = ExecuteReader(connectionString, procedureName, parameters);
            var item = result.FirstOrDefault();
            if (item == null) return default(T);
            return DynamicConverter.Convert<T>(item);
        }

        public List<T> ReadList<T>(string connectionString, string procedureName) where T : class
        {
            return ReadList<T>(connectionString, procedureName, null);
        }

        public List<T> ReadList<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class
        {
            var items = ExecuteReader(connectionString, procedureName, parameters);
            var result = new List<T>();
            foreach (var item in items)
            {
                result.Add(DynamicConverter.Convert<T>(item));
            }
            return result;
        }

        public void ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = procedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                    }
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
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

        private List<dynamic> ExecuteReader(string connectionString, string procedureName, Dictionary<string, object> parameters)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = procedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
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
