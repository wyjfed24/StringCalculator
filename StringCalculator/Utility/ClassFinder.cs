using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StringCalculator.Utility
{
    /// <summary>
    /// 类查找器
    /// </summary>
    public static class ClassFinder
    {
        /// <summary>
        /// 获取类实例
        /// </summary>
        /// <typeparam name="Attr"></typeparam>
        /// <typeparam name="IInterface"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IInterface GetInstance<Attr, IInterface>(Func<Attr, bool> func, params object[] paramList) where Attr : Attribute
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();
            var importProviders = types.Where(x => x.CustomAttributes.Any(c => c.AttributeType == typeof(Attr))).ToList();
            //通过特性标识找到对应处理类
            var curProviderType = importProviders.FirstOrDefault(x =>
            {
                var attr = (Attr)x.GetCustomAttribute(typeof(Attr));
                return func(attr);
            });
            if (curProviderType == null)
                return default(IInterface);
            var curProvider = (IInterface)Activator.CreateInstance(curProviderType, paramList);
            return curProvider;
        }

        /// <summary>
        /// 获取接口实现类字典
        /// </summary>
        /// <typeparam name="Attr"></typeparam>
        /// <typeparam name="IInterface"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Dictionary<string, IInterface> GetInterfaceInstances<Attr, IInterface>(Func<Attr, string> func) where Attr : Attribute
        {
            var dic = new Dictionary<string, IInterface>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IInterface))))
                      .Where(x => !x.IsAbstract && !x.IsInterface)
                      .ToList();
            foreach (var t in types)
            {
                var ins = (IInterface)Activator.CreateInstance(t);
                var attr = (Attr)t.GetCustomAttribute(typeof(Attr));
                var key = func(attr);
                dic.Add(key, ins);
            }
            return dic;
        }
    }
}
