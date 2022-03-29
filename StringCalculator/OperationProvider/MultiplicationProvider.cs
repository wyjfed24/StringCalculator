﻿using StringCalculator.Attr;
using StringCalculator.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.OperationProvider
{
    /// <summary>
    /// 乘法运算
    /// </summary>
    [Operation("*")]
    internal class MultiplicationProvider : IOperationProvider
    {
        public int Priority { get; set; } = 2;
        public decimal Calc(decimal left, decimal right) => left * right;
    }
}