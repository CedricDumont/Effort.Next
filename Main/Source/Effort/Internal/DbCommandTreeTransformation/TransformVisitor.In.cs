﻿// --------------------------------------------------------------------------------------------
// <copyright file="TransformVisitor.In.cs" company="Effort Team">
//     Copyright (C) Effort Team
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------

namespace Effort.Internal.DbCommandTreeTransformation
{
    using System;
#if !EFOLD
    using System.Data.Entity.Core.Common.CommandTrees;
#else
    using System.Data.Common.CommandTrees;
#endif
    using System.Linq.Expressions;
    using Effort.Exceptions;

    internal partial class TransformVisitor
    {
#if !EFOLD
        public override Expression Visit(DbInExpression expression)
        {
            throw new NotImplementedException(
                string.Format(
                    ExceptionMessages.DbExpressionTransformationNotImplemented,
                    typeof(DbInExpression).Name));
        }
#endif
    }
}
