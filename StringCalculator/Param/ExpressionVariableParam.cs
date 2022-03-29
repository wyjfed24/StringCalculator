using System;
using System.Collections.Generic;
using System.Text;

namespace StringCalculator.Param
{
    /// <summary>
    /// 表达式变量
    /// </summary>
    public class ExpressionVariableParam
    {
        /// <summary>
        /// 变量名
        /// </summary>
        public string Variable { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        public ExpressionVariableParam(string name, string value)
        {
            Variable = name;
            Value = value;
        }
        public override string ToString()
        {
            return (this?.Variable ?? "") + (this?.Value ?? "");
        }
    }
}
