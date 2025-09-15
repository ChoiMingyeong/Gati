using Microsoft.Extensions.Logging;
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
        private static bool IsFrameworkAssembly(Assembly assembly)
        {
            var name = assembly.GetName().Name ?? string.Empty;
            return name.StartsWith("System.", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("Windows.", StringComparison.OrdinalIgnoreCase)
                || name.Equals("Microsoft.AspNetCore.Server.IIS", StringComparison.OrdinalIgnoreCase)
                || name.Equals("Microsoft.AspNetCore.Server.IISIntegration", StringComparison.OrdinalIgnoreCase);
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // 문제타입만 빼고 진행
                return ex.Types.Where(t => t != null)!;
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }

        public static void ExecuteStaticMethod<T>(params object[] param)
        {
            var result = new List<MethodInfo>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.IsDynamic == false)
                .Where(a => string.IsNullOrEmpty(a.Location) == false)      // 위치없는 동적/리소스 어셈블리 제외
                .Where(a => IsFrameworkAssembly(a) == false);               // 프레임워크/IIS 어셈블리 제외

            foreach (var assembly in assemblies)
            {
                foreach (var type in GetLoadableTypes(assembly))
                {
                    IEnumerable<MethodInfo> methods;
                    try
                    {
                        methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    }
                    catch
                    {
                        continue;   // 타입 리플렉션 실패시 스킵
                    }

                    foreach (var m in methods)
                    {
                        if (m.GetCustomAttributes(typeof(T), inherit: false).Any())
                            result.Add(m);
                    }
                }
            }

            foreach (var method in result)
            {
                try
                {
                    method.Invoke(null, param);
                }
                catch (TargetInvocationException ex)
                {
                    Logger.Default.LogError(ex.InnerException ?? ex, "[AttrInvoke] {0}", method.Name);
                }
                catch (Exception ex)
                {
                    Logger.Default.LogError(ex, "[AttrInvoke] {0}", method.Name);
                }
            }
        }
    }
}
