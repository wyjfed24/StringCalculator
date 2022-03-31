### 简介 


**字符串计算器，支持左右括号、加减乘除、乘方，开平方，三角函数，提供自定义运算符扩展，支持自定义中文变量扩展。** 

使用示例：

```
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp = "测试.测试算法1+a";
var result = await calculator.CalcAsync(new MyExpressionParam
{
    Expression = exp,
    Ratio = 2,
    DynamicVariables = new List<ExpressionVariableParam> {
    new ExpressionVariableParam("a","1") }
});
Console.WriteLine($"{exp} = {result}");

```
输出：  测试.测试算法1+a = 201


```
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp2 = "-((10-测试.测试算法1)/2+(测试.测试算法2+1)^2)";
var result2 = await calculator.CalcAsync(new MyExpressionParam { Expression = exp2, Ratio = 2 });
Console.WriteLine($"{exp2} = {result2}");
```

输出： -((10-测试.测试算法1)/2+(测试.测试算法2+1)^2) = -40306


### 运算符扩展


```
/// <summary>
/// 自定义常规运算 包含左右两个元素，如：1+2 1-2
/// </summary>
[Operation("addmul2")]
internal class TestOperationProvider : IOperationProvider
{
    public int Priority { get; set; } = 2;
    public OperationType OperationType { get; set; } = OperationType.Normal;//需指定运算类型为Normal

    public decimal Calc(decimal left, decimal right)
    {
        return (left + right) * 10;
    }
}
```

```
/// <summary>
/// 自定义右运算，只包含运算符右侧一个元素，如sin30 cos30 sqrt100
/// </summary>
[Operation("sin")]
internal class SinProvider : IOperationProvider
{
    public int Priority { get; set; } = 4;
    public OperationType OperationType { get; set; } = OperationType.Right;//需指定运算类型为Right


    public decimal Calc(decimal left, decimal right)
    {
        return (decimal)Math.Sin((double)right);
    }
}
```


```
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp2 = "5 addmul2 5";
var result2 = await calculator.CalcAsync(new DefaultExpressionParam{ Expression = exp2});
Console.WriteLine($"{exp2} = {result2}");
```

输出： 5 addmul2 5 = 100


### 中文变量扩展


1. 首先继承ExpressionProviderFactoryBase工厂类 


```
[ExpressionFactory("测试")]
internal class ExpressionProviderFactory : ExpressionProviderFactoryBase
{
}
```


2. 再实现具体业务算法


```
[ExpressionProvider("测试算法2")]
internal class TestProvider2 : ICalcProvider
{
    public async Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator)
    {
        //统计订单金额..获取用户年龄..等等
        await Task.Delay(0);
        return 200m;
    }
}
```

```
[ExpressionProvider("测试算法1")]
internal class TestProvider1 : ICalcProvider
{
    public async Task<decimal> CalcAsync(IExpressionParam param, IExpressionCalculator calculator)
    {
        //统计订单金额..获取用户年龄..等等
        var newP = param as MyExpressionParam;//转换自定义参数,参数中定义id等查询需要的属性作为参数，实现中根据参数查询对应数据
        await Task.Delay(0);
        return 100m * newP.Ratio;
    }
}
```

3. 扩展自定义表达式参数


```
internal class MyExpressionParam : IExpressionParam  //继承表达式参数接口
{
    /// <summary>
    /// 自定义属性
    /// </summary>
    public int Ratio { get; set; }
    /// <summary>
    /// 表达式
    /// </summary>
    public string Expression { get; set; } = String.Empty;
    /// <summary>
    /// 动态表达式
    /// </summary>
    public List<ExpressionVariableParam>? DynamicVariables { get; set; } = new List<ExpressionVariableParam>();
    /// <summary>
    /// 精度
    /// </summary>
    public int Precision { get; set; }

    /// <summary>
    /// 替换表达式
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public IExpressionParam ReplaceParam(string exp)
    {
        return new MyExpressionParam() { Expression = exp, DynamicVariables = this.DynamicVariables, Precision = this.Precision, Ratio =         
               this.Ratio };
    }
}
```


4. 使用方式


```
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp2 = "-((10-测试.测试算法1)/2+(测试.测试算法2+1)^2)";
var result2 = await calculator.CalcAsync(new MyExpressionParam { Expression = exp2, Ratio = 2 });
Console.WriteLine($"{exp2} = {result2}");
```

输出： -((10-测试.测试算法1)/2+(测试.测试算法2+1)^2) = -40306