using StringCalculator.Attr;
using StringCalculator.Calc.Interface;
using StringCalculator.Factory;
using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [ExpressionFactory("测试")]
    internal class ExpressionProviderFactory : ExpressionProviderFactoryBase
    {
    }
}
