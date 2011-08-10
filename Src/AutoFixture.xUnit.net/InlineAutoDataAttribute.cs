﻿using System;
using System.Collections.Generic;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from inline
    /// values combined with auto-generated data specimens generated by AutoFixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    public sealed class InlineAutoDataAttribute : CompositeDataAttribute
    {
        private readonly IEnumerable<object> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineAutoDataAttribute"/> class.
        /// </summary>
        /// <param name="values">The data values to pass to the theory.</param>
        public InlineAutoDataAttribute(params object[] values)
            : base(new InlineDataAttribute(values), new AutoDataAttribute())
        {
            this.values = values;
        }

        /// <summary>
        /// Gets the data values to pass to the theory.
        /// </summary>
        public IEnumerable<object> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}
