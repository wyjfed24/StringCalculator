using StringCalculator.Calc.Interface;
using StringCalculator.Enum;
using StringCalculator.Interface;
using StringCalculator.Model;
using StringCalculator.Param;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StringCalculator
{
    /// <summary>
    /// 表达式计算类
    /// </summary>
    public class ExpressionCalculator : IExpressionCalculator
    {
        /// <summary>
        /// //计算结果缓存
        /// </summary>
        private ConcurrentDictionary<int, decimal> _calcResultCache = new ConcurrentDictionary<int, decimal>();
        /// <summary>
        /// 表达式转换器
        /// </summary>
        public IExpressionConvert _expressionConvert { get; set; }

        public ExpressionCalculator(IExpressionConvert expressionConvert)
        {
            _expressionConvert = expressionConvert;
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<decimal> CalcAsync(IExpressionParam param)
        {
            //转换后缀表达式元素数组
            var expItems = _expressionConvert.ToSuffixExpressionItemList(param.Expression);
            //替换动态变量，并返回变量类型元素集合（排除动态变量部分）
            var variableExpItems = ReplaceDynamicVariables(expItems, param.DynamicVariables);
            //计算每个变量的值，转换为数字，在进行四则运算，内部已转换数字型元素
            await CalcExpressionItemValueToNumberAsync(variableExpItems, param);
            //计算数字类型表达式
            var result = CalcNumberExpression(expItems);
            //计算结果四舍五入
            result = Math.Round(result, param.Precision, MidpointRounding.AwayFromZero);
            return result;
        }

        /// <summary>
        /// 替换动态变量
        /// </summary>
        /// <param name="variableExpItems"></param>
        /// <param name="dynamicVariables"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private List<ExpressionItem> ReplaceDynamicVariables(List<ExpressionItem> expItems, List<ExpressionVariableParam>? dynamicVariables)
        {
            //筛选变量类型元素
            var variableExpItems = expItems.Where(x => x.Type == ExpressionItemType.Variable).ToList();
            if (dynamicVariables != null && dynamicVariables.Count > 0)
            {
                variableExpItems.ForEach(expItem =>
                {
                    var dynamicVar = dynamicVariables.FirstOrDefault(c => c.Variable == expItem.Element);
                    if (dynamicVar != null)
                    {
                        if (!decimal.TryParse(dynamicVar.Value, out _))
                            throw new Exception($"参数错误：“{expItem.Element}”变量的值“{dynamicVar.Value}”不是数字");
                        expItem.Element = dynamicVar.Value;
                        expItem.Type = ExpressionItemType.Number;
                    }
                });
                //再筛选一次变量类型元素
                variableExpItems = variableExpItems.Where(x => x.Type == ExpressionItemType.Variable).ToList();
            }
            return variableExpItems;
        }

        /// <summary>
        /// 校验计算式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool CheckExpressionAsync(IExpressionParam param)
        {
            //转换后缀表达式元素数组
            var expItems = _expressionConvert.ToSuffixExpressionItemList(param.Expression);
            //校验计算式
            var varExpList = expItems.Where(x => x.Type == ExpressionItemType.Variable).ToList();
            var isPassed = true;
            foreach (var newExp in varExpList)
            {
                //用当前元素替换表达式
                var itemParam = param.ReplaceParam(newExp.Element);
                isPassed = GetCalcFactoryProvider(newExp.Element).CheckExpression(itemParam, this);
                if (!isPassed)
                    break;
            }
            return isPassed;
        }

        /// <summary>
        /// 计算每个元素项的值并转换为数字型元素
        /// </summary>
        /// <param name="expItems"></param>
        /// <returns></returns>
        private async Task CalcExpressionItemValueToNumberAsync(List<ExpressionItem> variableExpItems, IExpressionParam param)
        {
            foreach (var item in variableExpItems)
            {
                //用当前元素替换表达式
                var itemParam = param.ReplaceParam(item.Element);
                //查找计算缓存字典
                //序列化为json再获取哈希码作为字典的key
                var key = JsonSerializer.Serialize(itemParam).GetHashCode();
                var readCache = _calcResultCache.TryGetValue(key, out var result);
                if (!readCache)
                {
                    //获取算法工厂实例
                    var curProvider = GetCalcFactoryProvider(item.Element);
                    //计算
                    result = await curProvider.CalcAsync(itemParam, this);
                    result = Math.Round(result, param.Precision, MidpointRounding.AwayFromZero);
                    //计算结果缓存到计算字典中
                    _calcResultCache.TryAdd(key, result);
                }
                //计算结果替换表达式
                item.Element = result.ToString();
                //转换为数字类型
                item.Type = ExpressionItemType.Number;
            };
        }

        /// <summary>
        /// 根据表达式获取算法工厂实例
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ICalcProviderFactory GetCalcFactoryProvider(string expression)
        {
            var factoryProviders = ExpressionCalcProviderCache.CalcFactoryProviders;
            //获取算法工厂名称
            var factoryName = expression.Split('.')[0];
            if (!factoryProviders.ContainsKey(factoryName))
                throw new Exception($"检测到未定义模块变量：{expression}");
            var curProvider = factoryProviders[factoryName];
            return curProvider;
        }


        /// <summary>
        /// 计算数字类型表达式
        /// </summary>
        /// <param name="expNumerItems"></param>
        /// <returns></returns>
        private decimal CalcNumberExpression(List<ExpressionItem> expNumerItems)
        {
            var stack = new Stack<decimal>();
            foreach (var item in expNumerItems)
            {
                if (item.Type == ExpressionItemType.Number)
                {
                    if (!decimal.TryParse(item.Element, out var number))//超过decimal最大值情况未处理-todo
                        throw new Exception($"“{item.Element}”不是有效数字");
                    stack.Push(number);
                }
                else if (item.Type == ExpressionItemType.Operation)
                {
                    //运算符类型OperationProvider始终有值
                    var rightNumber = stack.Pop();
                    var leftNumber = 0m;
                    if (item.OperationProvider.OperationType == OperationType.Normal)
                        leftNumber = stack.Pop();
                    var value = item.OperationProvider.Calc(leftNumber, rightNumber);
                    stack.Push(value);
                }
            }
            var result = stack.Count == 0 ? 0m : stack.Pop();
            return result;
        }
    }
}
