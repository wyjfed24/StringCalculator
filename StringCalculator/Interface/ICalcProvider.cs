using StringCalculator.Calc.Interface;
using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Interface
{
    /// <summary>
    /// 基础变量计算接口
    /// </summary>
    public interface ICalcProvider
    {
        /// <summary>
        /// /计算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator);
    }
}
