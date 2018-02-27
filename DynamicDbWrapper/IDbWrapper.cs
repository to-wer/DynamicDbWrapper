using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDbWrapper
{
    public interface IDbWrapper
    {
        T ExecuteReader<T>(string connectionString, string procedureName)
            where T : class;
        
    }
}
