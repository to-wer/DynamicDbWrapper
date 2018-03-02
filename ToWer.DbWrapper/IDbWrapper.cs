using System.Collections.Generic;
using System.Data.SqlClient;

namespace ToWer.DbWrapper
{
    public interface IDbWrapper
    {
        void SetConnectionString(string connectionString);
        void SetStandardCommandParameters(List<SqlParameter> parameters);

        T ReadSingle<T>(string procedureName) where T : class;
        T ReadSingle<T>(string procedureName, Dictionary<string, object> parameters) where T : class;
        T ReadSingle<T>(string procedureName, List<SqlParameter> parameters) where T : class;
        T ReadSingle<T>(string connectionString, string procedureName) where T : class;
        T ReadSingle<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class;
        T ReadSingle<T>(string connectionString, string procedureName, List<SqlParameter> parameters) where T : class;

        List<T> ReadList<T>(string procedureName) where T : class;
        List<T> ReadList<T>(string procedureName, Dictionary<string, object> parameters) where T : class;
        List<T> ReadList<T>(string procedureName, List<SqlParameter> parameters) where T : class;
        List<T> ReadList<T>(string connectionString, string procedureName) where T : class;
        List<T> ReadList<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class;
        List<T> ReadList<T>(string connectionString, string procedureName, List<SqlParameter> parameters) where T : class;

        void ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters);
        void ExecuteNonQuery(string procedureName, List<SqlParameter> parameters);
        void ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters);
        void ExecuteNonQuery(string connectionString, string procedureName, List<SqlParameter> parameters);
        void ExecuteNonQuery<T>(string connectionString, string procedureName, T item) where T : class;
    }
}
