using StringCalculator.Attr;
using StringCalculator.Enum;
using StringCalculator.Interface;
using StringCalculator.Model;
using StringCalculator.Utility;

namespace StringCalculator
{
    /// <summary>
    /// 表达式转换器
    /// </summary>
    public class ExpressionConvert : IExpressionConvert
    {
        /// <summary>
        /// 运算提供器缓存
        /// </summary>
        private static Dictionary<string, IOperationProvider> OperationProviderDic;
        private static Dictionary<string, List<ExpressionItem>> OperationExpressionItemListDic;
        /// <summary>
        /// 运算符标识集合
        /// </summary>
        private static List<string> OperationSigns;
        static ExpressionConvert()
        {
            //预加载所有运算提供器
            OperationProviderDic = ClassFinder.GetInterfaceInstances<OperationAttribute, IOperationProvider>(x => x.Name);
            OperationSigns = OperationProviderDic.Select(x => x.Key).ToList();
            //创建拼接运算符字典
            BuildOperationCombinRefItem();
        }

        /// <summary>
        /// 构建双运算符拼接字典，处理 “1+sin30” “10addmulsin30” “1+sqrt9” 情况
        /// </summary>
        private static void BuildOperationCombinRefItem()
        {
            OperationExpressionItemListDic = new Dictionary<string, List<ExpressionItem>>();
            foreach (var item in OperationProviderDic)
            {
                var expItem = CreateOperationExpItem(item.Key);
                OperationExpressionItemListDic.Add(item.Key, new List<ExpressionItem> { expItem });
                foreach (var subItem in OperationProviderDic)
                {
                    var subExpItem = CreateOperationExpItem(subItem.Key);
                    OperationExpressionItemListDic.Add(item.Key + subItem.Key, new List<ExpressionItem> { expItem, subExpItem });
                }
            }
        }

        /// <summary>
        /// 创建运算元素
        /// </summary>
        /// <param name="operationName"></param>
        /// <returns></returns>
        private static ExpressionItem CreateOperationExpItem(string operationName)
        {
            var provider = OperationProviderDic[operationName];
            var operationElem = new ExpressionItem
            {
                Element = operationName,
                OperationProvider = provider,
                Priority = provider.Priority,
                Type = ExpressionItemType.Operation
            };
            return operationElem;
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
            //-((-22+变量.费用)×4)+123-变量.费用
            //负号情况
            //①-()
            //②-1+2
            //③-变量.费用
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
            //根据运算符分割表达式，获取所有元素（带括号）
            var elems = formatExp.Split(OperationSigns.ToArray(), StringSplitOptions.None);
            //反向根据元素分割表达式，获取所有运算符（含自定义运算符）
            var operationNames = formatExp.Split(elems, StringSplitOptions.RemoveEmptyEntries);
            var i = -1;
            foreach (var elem in elems)//
            {
                if (i >= 0 && i < operationNames.Length)//插入运算符
                {
                    if (string.IsNullOrWhiteSpace(elem))//第二次循环后的空元素不处理，当使用右运算如：sin30作为开头，首个分割元素才会是空元素
                        continue;
                    var operationName = operationNames[i];
                    var operationExpItems = OperationExpressionItemListDic[operationName];
                    list.AddRange(operationExpItems);
                }
                //创建数字变量括号元素
                CreateElemExpItems(elem, expressionStr, list);
                i++;
            }
            //括号匹配校验
            if (list.Count(x => x.Type == ExpressionItemType.LeftParenthesis) != list.Count(x => x.Type == ExpressionItemType.RightParenthesis))
                throw new Exception($"语法错误：{expressionStr}中“括号不匹配”");
            list.ForEach(x => Console.Write(x.Element));
            Console.WriteLine();
            return list;
        }

        /// <summary>
        /// 创建数字变量括号元素
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="expressionStr"></param>
        /// <param name="list"></param>
        /// <exception cref="Exception"></exception>
        private void CreateElemExpItems(string elem, string expressionStr, List<ExpressionItem> list)
        {
            foreach (var charItem in elem)
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
                //跟上一次元素类型相同则相加，否则就添加到结果数组中
                if (lastElem != null && ((lastElem.Type == ExpressionItemType.Number && lastElem.Type == curElem.Type) || (lastElem.Type == ExpressionItemType.Variable && (int)curElem.Type >= 20)))
                    lastElem += curElem;//直接更新上一次元素值
                else
                    list.Add(curElem);//添加到集合
            }
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
            formulaList.ForEach(x => Console.Write($"{x.Element} "));
            Console.WriteLine();
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
                    return curType == ExpressionItemType.RightParenthesis || curType == ExpressionItemType.Operation || curType == ExpressionItemType.Number;
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