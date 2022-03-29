using System;

namespace StringCalculator.Attr
{
    /// <summary>
    /// 表达式处理类特性
    /// </summary>
    public class ExpressionProviderAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public ExpressionProviderAttribute(string name)
        {
            Name = name;
        }
    }
}
