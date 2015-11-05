﻿using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from inline
    /// values combined with auto-generated data specimens generated by AutoFixture.
    /// </summary>
    [DataDiscoverer(
        typeName: "Ploeh.AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer",
        assemblyName: "Ploeh.AutoFixture.Xunit2")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class InlineAutoDataAttribute : CompositeDataAttribute
    {
        private readonly AutoDataAttribute autoDataAttribute;
        private readonly IEnumerable<object> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineAutoDataAttribute"/> class.
        /// </summary>
        /// <param name="values">The data values to pass to the theory.</param>
        public InlineAutoDataAttribute(params object[] values)
            : this(new AutoDataAttribute(), values)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineAutoDataAttribute"/> class.
        /// </summary>
        /// <param name="autoDataAttribute">An <see cref="AutoDataAttribute"/>.</param>
        /// <param name="values">The data values to pass to the theory.</param>
        protected InlineAutoDataAttribute(AutoDataAttribute autoDataAttribute, params object[] values)
            : base(new DataAttribute[] { new InlineDataAttribute(values), autoDataAttribute })
        {
            this.autoDataAttribute = autoDataAttribute;
            this.values = values;
        }

        /// <summary>
        /// Gets the data values to pass to the theory.
        /// </summary>
        public IEnumerable<object> Values
        {
            get { return this.values; }
        }

        /// <summary>
        /// Gets the <see cref="AutoDataAttribute"/> encapsulated by this instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the constructor overload wich takes an explicit instance of
        /// <see cref="AutoDataAttribute" /> is used, this property exposes that instance.
        /// </para>
        /// </remarks>
        /// <seealso cref="InlineAutoDataAttribute(AutoDataAttribute, object[])"/>
        public AutoDataAttribute AutoDataAttribute
        {
            get { return this.autoDataAttribute; }
        }
    }
}