using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class TestExpressionParam : IExpressionParam
    {
        /// <summary>
        /// 自定义属性
        /// </summary>
        public int Ratio { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; } = String.Empty;
        /// <summary>
        /// 动态表达式
        /// </summary>
        public List<ExpressionVariableParam>? DynamicVariables { get; set; } = new List<ExpressionVariableParam>();
        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 替换表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IExpressionParam ReplaceParam(string exp)
        {
            return new TestExpressionParam() { Expression = exp, DynamicVariables = this.DynamicVariables, Precision = this.Precision, Ratio = this.Ratio };
        }
    }
}
