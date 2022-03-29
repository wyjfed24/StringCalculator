
using StringCalculator.Calc.Interface;
using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Interface
{
    /// <summary>
    /// 计算工厂接口
    /// </summary>
    public interface ICalcProviderFactory
    {
        /// <summary>
        /// /计算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator);

        bool CheckExpression(IExpressionParam param, IExpressionCalculator calculator);
    }
}
