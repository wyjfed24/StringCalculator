// See https://aka.ms/new-console-template for more information
using StringCalculator;
using StringCalculator.Calc.Interface;
using StringCalculator.Interface;
using StringCalculator.Param;

using Test;

Console.WriteLine("Hello, World!");
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp = "测试.测试算法1+测试.测试算法2";
var result = await calculator.CalcAsync(new TestExpressionParam
{
    Expression = exp,
    Ratio = 2,
    DynamicVariables = new List<ExpressionVariableParam> {
    new ExpressionVariableParam("a","1") }
});
Console.WriteLine($"{exp} = {result}");
//var exp2 = "sin30-((10-测试.测试算法1)addmul2(测试.测试算法2+1)^2)+sin30";
//var result2 = await calculator.CalcAsync(new TestExpressionParam { Expression = exp2, Ratio = 2 });
//Console.WriteLine($"{exp2} = {result2}");

var exp2 = "-sqrt9 - sinf 30";
var result2 = await calculator.CalcAsync(new DefaultExpressionParam { Expression = exp2 });
Console.WriteLine($"{exp2} = {result2}");