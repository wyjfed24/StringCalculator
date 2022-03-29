using StringCalculator.Attr;
using StringCalculator.Calc.Interface;
using StringCalculator.Interface;
using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Factory
{
    /// <summary>
    /// 表达式提供器工厂基类
    /// </summary>
    public abstract class ExpressionProviderFactoryBase : ICalcProviderFactory
    {
        /// <summary>
        /// 获取算法提供器名称，默认截取第一个“.”后元素作为算法提供器名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual string GetProviderName(string expression)
        {
            var arr = expression.Split('.');
            if (arr.Length != 2)
                throw new Exception($"语法错误：“{expression}”中必须包含“.”,且需要在“.”后填写算法名称");
            return expression.Split('.')[1];
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="param"></param>
        /// <param name="calculator"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator)
        {
            //获取算法提供器名称
            var providerName = GetProviderName(param.Expression);
            //根据变量名称匹配算法
            if (!ExpressionCalcProviderCache.BasicCalcProviders.TryGetValue(providerName, out var curProvider))
                throw new Exception($"检测到未定义变量：{param.Expression}");
            var result = await curProvider.CalcAsync(param, calculator);
            return result;
        }

        /// <summary>
        /// 校验表达式
        /// </summary>
        /// <param name="param"></param>
        /// <param name="calculator"></param>
        /// <returns></returns>
        public virtual bool CheckExpression(IExpressionParam param, IExpressionCalculator calculator)
        {
            return true;
        }
    }
}
