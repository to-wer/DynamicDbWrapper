using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToWer.DbWrapper.Converter
{
    public class DynamicConverter
    {
        public static T Convert<T>(ExpandoObject source)
    where T : class
        {
            IDictionary<string, object> dict = source;

            //var ctor = example.GetType().GetConstructors().Single(c => c.GetParameters().Count() > 0);
            var ctor = typeof(T).GetConstructors().Single(c => c.GetParameters().Count() > 0);

            var parameters = ctor.GetParameters();

            var parameterValues = parameters.Select(p => dict[p.Name]).ToArray();

            return (T)ctor.Invoke(parameterValues);
        }
    }
}
