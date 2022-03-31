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
    /// sin运算
    /// </summary>
    [Operation("sin")]
    internal class SinProvider : IOperationProvider
    {
        public int Priority { get; set; } = 4;
        public OperationType OperationType { get; set; } = OperationType.Right;


        public decimal Calc(decimal left, decimal right)
        {
            return (decimal)Math.Sin((double)right);
        }
    }
}
