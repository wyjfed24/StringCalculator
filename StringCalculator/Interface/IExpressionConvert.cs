using StringCalculator.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Interface
{
    public interface IExpressionConvert
    {
        List<ExpressionItem> ToSuffixExpressionItemList(string expression);
    }
}
