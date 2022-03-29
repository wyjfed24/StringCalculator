using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StringCalculator.Enum
{
    /// <summary>
    /// 表达式项类型
    /// </summary>
    public enum ExpressionItemType
    {
        /// <summary>
        /// 运算符
        /// </summary>
        [Description("运算符")]
        Operation = 1,
        /// <summary>
        /// 左小括号
        /// </summary>
        [Description("左小括号")]
        LeftParenthesis =2,
        /// <summary>
        /// 右小括号
        /// </summary>
        [Description("右小括号")]
        RightParenthesis =3,
        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字")]
        Number = 20,
        /// <summary>
        /// 变量
        /// </summary>
        [Description("变量")]
        Variable = 21
    }
}
