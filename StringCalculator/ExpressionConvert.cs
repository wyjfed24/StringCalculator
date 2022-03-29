using StringCalculator.Attr;
using StringCalculator.Enum;
using StringCalculator.Interface;
using StringCalculator.Model;
using StringCalculator.Utility;

namespace StringCalculator
{
    public class ExpressionConvert : IExpressionConvert
    {
        /// <summary>
        /// 运算提供器缓存
        /// </summary>
        private static Dictionary<string, IOperationProvider> OperationProviderDic;
        private static List<string> OperationSigns;
        static ExpressionConvert()
        {
            //预加载所有运算提供器
            OperationProviderDic = ClassFinder.GetInterfaceInstances<OperationAttribute, IOperationProvider>(x => x.Name);
            OperationSigns = OperationProviderDic.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// 表达式转换后缀表达式元素数组
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<ExpressionItem> ToSuffixExpressionItemList(string expression)
        {
            var infixExpItemList = ToInfixExpressionItemList(expression);
            var suffixExpItemList = ToSuffixExpressionItemList(infixExpItemList);
            return suffixExpItemList;
        }

        /// <summary>
        /// 转换中缀表达式元素数组
        /// </summary>
        /// <param name="expression">中缀表达式字符串</param>
        /// <returns></returns>
        internal List<ExpressionItem> ToInfixExpressionItemList(string expressionStr)
        {
            //-((-22+人工费)×4)+123-人工费
            //负号情况
            //①-()
            //②-1+2
            //③-@取费表.人工费
            //④1-(-1)
            //只会出现开始带“-”和“（-”结构的负号
            if (expressionStr.Trim() == String.Empty)
                throw new Exception("不能计算空表达式");
            var list = new List<ExpressionItem>();
            //清除空格,把表达式中负号替换为“0-X”形式，
            var formatExp = expressionStr.Replace(" ", "").Replace("(-", "(0-");
            if (formatExp.FirstOrDefault() == '-')
                formatExp = $"0{formatExp}";
            //百分号替换为*0.01
            formatExp = formatExp.Replace("%", "*0.01");
            //从左到右扫描中缀表达式，需要合并多位数或单词/中文变量
            foreach (var charItem in formatExp)
            {
                var lastElem = list.LastOrDefault();//上一次的元素
                //当前元素
                var curElem = new ExpressionItem
                {
                    Element = charItem.ToString(),
                    Type = GetExpressionType(charItem)
                };
                //校验是否是正确的类型
                if (!CheckIsCorrectType(lastElem, curElem))
                    throw new Exception($"语法错误：{expressionStr}中“{lastElem?.Element}{curElem.Element}”错误");
                if (curElem.Type == ExpressionItemType.Operation)
                {
                    curElem.OperationProvider = OperationProviderDic[curElem.Element];
                    curElem.Priority = curElem.OperationProvider.Priority;
                }
                //跟上一次元素类型相同则相加，否则就添加到结果数组中
                if (lastElem != null && (int)lastElem.Type >= 20 && (int)curElem.Type >= 20)
                    lastElem += curElem;//直接更新上一次元素值
                else
                    list.Add(curElem);//添加到集合
            }
            //括号匹配校验
            if (list.Count(x => x.Type == ExpressionItemType.LeftParenthesis) != list.Count(x => x.Type == ExpressionItemType.RightParenthesis))
                throw new Exception($"语法错误：{expressionStr}中“括号不匹配”");
            return list;
        }
        /// <summary>
        /// 表达式元素数组转后缀表达式元素数组
        /// </summary>
        /// <param name="expressionList"></param>
        /// <returns></returns>
        internal List<ExpressionItem> ToSuffixExpressionItemList(List<ExpressionItem> expressionList)
        {
            //11+((22+3)×4)-5    =》 11 22 3 + 4 × + 5 -
            var formulaList = new List<ExpressionItem>();
            var operationStack = new Stack<ExpressionItem>();
            foreach (var item in expressionList)
            {
                switch (item.Type)
                {
                    case ExpressionItemType.Number:
                    case ExpressionItemType.Variable:
                        formulaList.Add(item);//数字和变量直接加入结果集
                        break;
                    case ExpressionItemType.LeftParenthesis:
                        operationStack.Push(item);//左括号入栈
                        break;
                    case ExpressionItemType.RightParenthesis:
                        //出栈栈顶元素到结果集中，直到遇到左括号
                        while (operationStack.Peek().Type != ExpressionItemType.LeftParenthesis)
                            formulaList.Add(operationStack.Pop());//出栈运算符加入结果集
                        operationStack.Pop();//丢弃栈顶左括号
                        break;
                    case ExpressionItemType.Operation:
                        //出栈优先级大于或等于当前的运算符元素到结果集中
                        while (operationStack.Count > 0 && operationStack.Peek().Priority >= item.Priority)
                            formulaList.Add(operationStack.Pop());//出栈运算符加入结果集
                        operationStack.Push(item);//当前元素入栈
                        break;
                    default:
                        break;
                }
            }
            //将剩余运算符写入结果集
            while (operationStack.Count > 0)
                formulaList.Add(operationStack.Pop());
            return formulaList;
        }

        /// <summary>
        /// 校验是否是正确的类型
        /// </summary>
        /// <param name="lastItem"></param>
        /// <param name="curItem"></param>
        /// <returns></returns>
        private bool CheckIsCorrectType(ExpressionItem? lastItem, ExpressionItem curItem)
        {
            var curType = curItem.Type;
            if (lastItem == null)//上一级元素为空时=》负号 左括号 数字 变量
                return curItem.Element == "-" ||
                       curType == ExpressionItemType.LeftParenthesis ||
                       (curType == ExpressionItemType.Number && curItem.Element != ".") ||
                       curType == ExpressionItemType.Variable;
            switch (lastItem.Type)
            {
                case ExpressionItemType.Operation:
                    return curType == ExpressionItemType.LeftParenthesis || curType == ExpressionItemType.Number || curType == ExpressionItemType.Variable;
                case ExpressionItemType.LeftParenthesis:
                    return curType == ExpressionItemType.LeftParenthesis || curType == ExpressionItemType.Operation || curType == ExpressionItemType.Number || curType == ExpressionItemType.Variable;
                case ExpressionItemType.RightParenthesis:
                    return curType == ExpressionItemType.RightParenthesis || curType == ExpressionItemType.Operation;
                case ExpressionItemType.Number:
                case ExpressionItemType.Variable:
                    return curType == ExpressionItemType.RightParenthesis || curType == ExpressionItemType.Operation || curType == ExpressionItemType.Number || curType == ExpressionItemType.Variable;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取字符对应表达式类型
        /// </summary>
        /// <param name="charItem"></param>
        /// <returns></returns>
        private ExpressionItemType GetExpressionType(char charItem)
        {
            //判断数字，运算符，左括号，右括号，变量
            if (".0123456789".Contains(charItem)) return ExpressionItemType.Number;
            else if (OperationProviderDic.ContainsKey(charItem.ToString())) return ExpressionItemType.Operation;
            else if (charItem == '(') return ExpressionItemType.LeftParenthesis;
            else if (charItem == ')') return ExpressionItemType.RightParenthesis;
            else return ExpressionItemType.Variable;
        }
    }
}