using DynamicDbWrapper.Converter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDbWrapper.MsSql
{
    public class MsSqlWrapper : IDbWrapper
    {
        public List<dynamic> ExecuteReader(string connectionString, string procedureName)
        {
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = procedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

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
            return default(dynamic);
        }

        public T ExecuteReader<T>(string connectionString, string procedureName) where T : class
        {
            var result = ExecuteReader(connectionString, procedureName);

            return DynamicConverter.Convert<T>(result.First());
        }
    }
}
