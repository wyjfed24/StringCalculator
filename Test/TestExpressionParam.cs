using StringCalculator.Param;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class TestExpressionParam : IExpressionParam
    {

        public int Ratio { get; set; }
        public string Expression { get; set; } = String.Empty;
        public List<ExpressionVariableParam>? DynamicVariables { get; set; } = new List<ExpressionVariableParam>();
        public int Precision { get; set; }

        public IExpressionParam ReplaceParam(string exp)
        {
            return new TestExpressionParam() { Expression = exp, DynamicVariables = this.DynamicVariables, Precision = this.Precision, Ratio = this.Ratio };
        }
    }
}
