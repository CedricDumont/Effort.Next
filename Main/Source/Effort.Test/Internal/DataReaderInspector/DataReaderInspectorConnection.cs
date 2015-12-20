﻿// --------------------------------------------------------------------------------------------
// <copyright file="DataReaderInspectorConnection.cs" company="Effort Team">
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

namespace Effort.Test.Internal.DataReaderInspector
{
    using System.Data.Common;
    using Effort.Provider;
    using Effort.Test.Internal.ResultSets;
    using Effort.Test.Internal.WrapperProviders;

    internal class DataReaderInspectorConnection : DbConnectionWrapper
    {
        private IResultSetComposer composer;

        public DataReaderInspectorConnection(IResultSetComposer composer)
        {
            this.composer = composer;
        }

        public IResultSetComposer Composer
        {
            get
            {
                return this.composer;
            }
        }

        protected override string DefaultWrappedProviderName
        {
            get 
            { 
                return EffortProviderConfiguration.ProviderInvariantName; 
            }
        }

        //protected override DbProviderFactory DbProviderFactory
        //{
        //    get
        //    {
        //        return DataReaderInspectorProviderFactory.Instance;
        //    }
        //}
    }
}
