using StringCalculator.Attr;
using StringCalculator.Enum;
using StringCalculator.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.OperationProvider
{
    /// <summary>
    /// 乘方运算
    /// </summary>
    [Operation("^")]
    internal class PowerProvider : IOperationProvider
    {
        public int Priority { get; set; } = 3;
        public OperationType OperationType { get; set; } = OperationType.Normal;
        /// <summary>
        /// 执行运算
        /// </summary>
        /// <param name="number">底数</param>
        /// <param name="pow">幂</param>
        /// <returns></returns>
        public decimal Calc(decimal number, decimal pow)
        {
            if (pow == 0)
                return 1m;
            if (number == 0)
                return 0m;
            var result = number;
            var count = Math.Abs(pow);
            for (int i = 1; i < count; i++)
            {
                result = result * number;
            }
            if (pow > 0)
                return result;
            else
                return 1m / result;
        }
    }
}
