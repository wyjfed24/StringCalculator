using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StringCalculator.Enum
{
    /// <summary>
    /// 运算类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 常规
        /// </summary>
        [Description("常规")]
        Normal = 1,
        /// <summary>
        /// 右运算
        /// </summary>
        [Description("右运算")]
        Right = 2,
       
    }
}
