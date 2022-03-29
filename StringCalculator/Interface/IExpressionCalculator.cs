

using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Calc.Interface
{
    /// <summary>
    /// 表达式计算器
    /// </summary>
    public interface IExpressionCalculator 
    {
        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<decimal> CalcAsync(IExpressionParam param);
        /// <summary>
        /// 校验表达式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool CheckExpressionAsync(IExpressionParam param);
    }
}
