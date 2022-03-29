using StringCalculator.Attr;
using StringCalculator.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.OperationProvider
{
    /// <summary>
    /// 加法运算
    /// </summary>
    [Operation("+")]
    internal class AdditionProvider : IOperationProvider
    {
        public int Priority { get; set; } = 1;

        public decimal Calc(decimal left, decimal right) => left + right;
    }
}
