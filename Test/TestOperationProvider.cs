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
    /// 自定义运算
    /// </summary>
    [Operation("addmul2")]
    internal class TestOperationProvider : IOperationProvider
    {
        public int Priority { get; set; } = 2;
        public OperationType OperationType { get; set; } = OperationType.Normal;

        public decimal Calc(decimal left, decimal right)
        {
            return (left + right) * 10;
        }
    }

    /// <summary>
    /// 自定义运算
    /// </summary>
    [Operation("sinf")]
    internal class TestOperationProvider2 : IOperationProvider
    {
        public int Priority { get; set; } = 4;
        public OperationType OperationType { get; set; } = OperationType.Right;


        public decimal Calc(decimal left, decimal right)
        {
            return right;
        }
    }
}
