using StringCalculator.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Interface
{
    /// <summary>
    /// 表达式转换器接口
    /// </summary>
    public interface IExpressionConvert
    {
        /// <summary>
        /// 表达式转换后缀表达式元素数组
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<ExpressionItem> ToSuffixExpressionItemList(string expression);
    }
}
