using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Interface
{
    /// <summary>
    /// 运算符接口
    /// </summary>
    public interface IOperationProvider
    {
        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get; set; }
        /// <summary>
        /// 运算
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        decimal Calc(decimal left, decimal right);

    }
}
