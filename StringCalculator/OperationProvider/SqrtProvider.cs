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
    /// 开方运算
    /// </summary>
    [Operation("sqrt")]
    internal class SqrtProvider : IOperationProvider
    {
        public int Priority { get; set; } = 3;
        public OperationType OperationType { get; set; } = OperationType.Right;


        public decimal Calc(decimal left, decimal right)
        {
            return (decimal)Math.Sqrt((double)right);
        }
    }
}
