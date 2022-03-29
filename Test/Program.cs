// See https://aka.ms/new-console-template for more information
using StringCalculator;
using StringCalculator.Param;

using Test;

Console.WriteLine("Hello, World!");
ExpressionConvert expressionConvert = new ExpressionConvert();
ExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);
var exp = "测试.测试算法1+a";
var result = await calculator.CalcAsync(new TestExpressionParam
{
    Expression = exp,
    Ratio = 2,
    DynamicVariables = new List<ExpressionVariableParam> {
    new ExpressionVariableParam("a","1") }
});
Console.WriteLine($"{exp} = {result}");
var exp2 = "测试+1";
var result2 = await calculator.CalcAsync(new TestExpressionParam { Expression = exp2, Ratio = 2 ,DynamicVariables = new List<ExpressionVariableParam> {
    new ExpressionVariableParam("a","1") }
});
Console.WriteLine($"{exp2} = {result2}");