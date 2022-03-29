using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringCalculator.Param
{
    /// <summary>
    /// 表达式参数接口
    /// </summary>
    public interface IExpressionParam
    {
        /// <summary>
        /// 表达式
        /// </summary>
        string Expression { get; set; }
        /// <summary>
        /// 动态变量集合 1+2+a+b
        /// <para>计算公式带已知值的变量时必填</para>
        /// </summary>
        List<ExpressionVariableParam>? DynamicVariables { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        int Precision { get; set; }


        /// <summary>
        /// 替换表达式
        /// </summary>
        IExpressionParam ReplaceParam(string exp);

    }
}
