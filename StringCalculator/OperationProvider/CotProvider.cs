using StringCalculator.Attr;
using StringCalculator.Enum;
using StringCalculator.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// cot运算
    /// </summary>
    [Operation("cot")]
    internal class CotProvider : IOperationProvider
    {
        public int Priority { get; set; } = 4;
        public OperationType OperationType { get; set; } = OperationType.Right;


        public decimal Calc(decimal left, decimal right)
        {
            return (decimal)(1d / Math.Tan((double)right));
        }
    }
}
