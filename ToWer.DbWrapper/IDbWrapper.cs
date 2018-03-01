using System.Collections.Generic;

namespace ToWer.DbWrapper
{
    public interface IDbWrapper
    {
        T ReadSingle<T>(string connectionString, string procedureName) where T : class;
        T ReadSingle<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class;

        List<T> ReadList<T>(string connectionString, string procedureName) where T : class;
        List<T> ReadList<T>(string connectionString, string procedureName, Dictionary<string, object> parameters) where T : class;

        void ExecuteNonQuery(string connectionString, string procedureName, Dictionary<string, object> parameters);

        void ExecuteNonQuery<T>(string connectionString, string procedureName, T item) where T : class;
    }
}
