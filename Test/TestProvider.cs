using StringCalculator.Attr;
using StringCalculator.Calc.Interface;
using StringCalculator.Interface;
using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [ExpressionProvider("测试算法1")]
    internal class TestProvider1 : ICalcProvider
    {
        public async Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator)
        {
            var newP = param as TestExpressionParam;
            await Task.Delay(0);
            return 100m * newP.Ratio;
        }
    }

    [ExpressionProvider("测试算法2")]
    internal class TestProvider2 : ICalcProvider
    {
        public async Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator)
        {
            await Task.Delay(0);
            return 200m;
        }
    }
}
