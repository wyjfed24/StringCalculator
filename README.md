 **字符串计算器，支持左右括号、加减乘除、乘方，提供自定义运算符扩展，支持中文变量，并提供自定义扩展。** 

代码示例：

```
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp = "测试.测试算法1+a";
var result = await calculator.CalcAsync(new TestExpressionParam
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
var result2 = await calculator.CalcAsync(new TestExpressionParam { Expression = exp2, Ratio = 2 });
Console.WriteLine($"{exp2} = {result2}");
```
输出： -((10-测试.测试算法1)/2+(测试.测试算法2+1)^2) = -40306



