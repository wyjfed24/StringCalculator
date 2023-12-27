// See https://aka.ms/new-console-template for more information
using StringCalculator;
using StringCalculator.Calc.Interface;
using StringCalculator.Interface;
using StringCalculator.Param;

using System.Diagnostics;

using Test;

using Z.Expressions;

Console.WriteLine("Hello, World!");

Stopwatch sw= Stopwatch.StartNew();
IExpressionConvert expressionConvert = new ExpressionConvert();
IExpressionCalculator calculator = new ExpressionCalculator(expressionConvert);

sw.Start();
var exp = "sqrt9 - (1/sinf 30) + sinf30/3";
var result = await calculator.CalcAsync(new TestExpressionParam
{
    Expression = exp,
    Ratio = 2,
    DynamicVariables = new List<ExpressionVariableParam> {
    new ExpressionVariableParam("a","1") }
});
Console.WriteLine($"{exp} = {result}");
sw.Stop();
Console.WriteLine($"执行时间：{sw.ElapsedMilliseconds}");

//var exp2 = "sin30-((10-测试.测试算法1)addmul2(测试.测试算法2+1)^2)+sin30";
//var result2 = await calculator.CalcAsync(new TestExpressionParam { Expression = exp2, Ratio = 2 });
//Console.WriteLine($"{exp2} = {result2}");
sw.Restart();
var exp2 = "-sqrt9 - 1/sinf 30 + sinf30/3";
var result2 = await calculator.CalcAsync(new DefaultExpressionParam { Expression = exp2 });
Console.WriteLine($"{exp2} = {result2}");
sw.Stop();
Console.WriteLine($"执行时间：{sw.ElapsedMilliseconds}");
Console.WriteLine();
Console.WriteLine();

sw.Restart();
Console.WriteLine("C# Eval Expression测试 Eval.Execute");
var exp3 = "X + Y";
int result1 = Eval.Execute<int>(exp3, new { X = 1, Y = 2 });

Console.WriteLine($"{exp3} = {result1}");
sw.Stop();
Console.WriteLine($"执行时间：{sw.ElapsedMilliseconds}");


sw.Restart();
Console.WriteLine("C# Eval Expression测试  Eval.Compile");
var exp4 = "{0} + {1}";
var compiled = Eval.Compile<Func<int, int, int>>(exp4);
int result4 = compiled(1, 2);
Console.WriteLine($"{exp4} = {result4}");
sw.Stop();
Console.WriteLine($"执行时间：{sw.ElapsedMilliseconds}");