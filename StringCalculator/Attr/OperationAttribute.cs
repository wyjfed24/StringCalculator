using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Attr
{
    /// <summary>
    /// 运算符特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationAttribute : Attribute
    {
        /// <summary>
        /// 运算符标识[字符或字符+数字]
        /// </summary>
        public string Name { get; set; }

        public OperationAttribute(string name)
        {
            Name = name;
        }
    }
}
