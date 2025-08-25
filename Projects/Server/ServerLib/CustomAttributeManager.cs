using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class CustomAttributeManager
    {
        private struct MethodReflect
        {
            public MethodInfo MethodInfo;

            public MethodReflect(MethodInfo methodInfo)
            {
                MethodInfo = methodInfo;
            }
        }
        public static void ExecuteStaticMethod<T>(params object[] param)
        {
            var reflectMethod = new List<MethodReflect>();
            var scriptAssemblies =
                AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in scriptAssemblies)
            {
                var methodInfos = assembly.GetTypes().SelectMany(m =>
                        m.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    .Where(x => x.GetCustomAttributes(typeof(T), false).Length > 0);
                reflectMethod.AddRange(methodInfos.Select(method => new MethodReflect(method)));
            }

            foreach (var reflect in reflectMethod)
            {
                var method = reflect.MethodInfo;
                if (method != null)
                    method.Invoke(null, param);
            }
        }
    }
}
