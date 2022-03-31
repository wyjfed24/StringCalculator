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
    /// tan运算
    /// </summary>
    [Operation("tan")]
    internal class TanProvider : IOperationProvider
    {
        public int Priority { get; set; } = 4;
        public OperationType OperationType { get; set; } = OperationType.Right;


        public decimal Calc(decimal left, decimal right)
        {
            return (decimal)Math.Tan((double)right);
        }
    }
}
