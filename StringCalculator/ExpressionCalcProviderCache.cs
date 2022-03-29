using StringCalculator.Attr;
using StringCalculator.Interface;
using StringCalculator.Utility;

using System;
using System.Collections.Generic;
using System.Text;

namespace StringCalculator
{
    /// <summary>
    /// 算法实例缓存
    /// </summary>
    public static class ExpressionCalcProviderCache
    {
        /// <summary>
        /// 算法工厂提供类实例集合
        /// </summary>
        public static Dictionary<string, ICalcProviderFactory> CalcFactoryProviders;
        /// <summary>
        /// 预设算法提供类实例集合
        /// </summary>
        public static Dictionary<string, ICalcProvider> BasicCalcProviders;

        static ExpressionCalcProviderCache()
        {
            CalcFactoryProviders = ClassFinder.GetInterfaceInstances<ExpressionFactoryAttribute, ICalcProviderFactory>(x => x.FactoryName);
            BasicCalcProviders = ClassFinder.GetInterfaceInstances<ExpressionProviderAttribute, ICalcProvider>(x => x.Name);
        }
    }
}
