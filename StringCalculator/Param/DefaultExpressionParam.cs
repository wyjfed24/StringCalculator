using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Param
{
    /// <summary>
    /// 默认表达式参数
    /// </summary>
    public class DefaultExpressionParam : IExpressionParam
    {
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; } = string.Empty;
        /// <summary>
        /// 动态变量
        /// </summary>
        public List<ExpressionVariableParam>? DynamicVariables { get; set; } = new List<ExpressionVariableParam>() { };
        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 替换表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IExpressionParam ReplaceParam(string exp)
        {
            return new DefaultExpressionParam() { Expression = exp, DynamicVariables = this.DynamicVariables, Precision = this.Precision };
        }
    }
}
