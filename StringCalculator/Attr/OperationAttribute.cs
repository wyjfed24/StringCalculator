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
    internal class OperationAttribute : Attribute
    {
        public string Name { get; set; }

        public OperationAttribute(string name)
        {
            Name = name;
        }
    }
}
