using System;

namespace StringCalculator.Attr
{
    /// <summary>
    /// 表达式工厂特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExpressionFactoryAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string FactoryName { get; set; }
        public ExpressionFactoryAttribute(string factoryName)
        {
            FactoryName = factoryName;
        }
    }
}
