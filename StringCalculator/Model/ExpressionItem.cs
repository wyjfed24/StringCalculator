using StringCalculator.Enum;
using StringCalculator.Interface;

namespace StringCalculator.Model
{
    /// <summary>
    /// 表达式项
    /// </summary>
    public class ExpressionItem
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; } = 0;
        /// <summary>
        /// 元素
        /// </summary>
        public string Element { get; set; } = string.Empty;
        /// <summary>
        /// 类型
        /// </summary>
        public ExpressionItemType Type { get; set; }
        /// <summary>
        /// 运算器
        /// </summary>
        public IOperationProvider? OperationProvider { get; set; }

        public static ExpressionItem operator +(ExpressionItem left, ExpressionItem right)
        {
            left.Element += right.Element;
            left.Type = right.Type > left.Type ? right.Type : left.Type;
            return left;
        }
    }
}
