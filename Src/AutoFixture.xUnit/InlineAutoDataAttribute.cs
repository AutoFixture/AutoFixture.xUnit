﻿using System;
using System.Collections.Generic;
using Xunit.Extensions;

namespace AutoFixture.Xunit
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from inline
    /// values combined with auto-generated data specimens generated by AutoFixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class InlineAutoDataAttribute : CompositeDataAttribute
    {
        /// <summary>
        /// Gets the attribute used to automatically generate the remaining theory parameters, which are not fixed.
        /// </summary>
        public DataAttribute AutoDataAttribute { get; }

        /// <summary>
        /// Gets the data values to pass to the theory.
        /// </summary>
        public IEnumerable<object> Values { get; }

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
        /// <param name="autoDataAttribute">An <see cref="DataAttribute"/>.</param>
        /// <param name="values">The data values to pass to the theory.</param>
        /// <remarks>
        /// <para>
        /// This constructor overload exists to enable a derived attribute to
        /// supply a custom <see cref="DataAttribute" /> that again may
        /// contain custom behavior.
        /// </para>
        /// </remarks>
        /// <example>
        /// In this example, TheAnswer is a Customization that changes all
        /// 32-bit integer values to 42. This behavior is encapsulated in
        /// MyCustomAutoDataAttribute, and transitively in
        /// MyCustomInlineAutoDataAttribute. A parameterized test demonstrates
        /// how it can be used.
        /// <code>
        /// [Theory]
        /// [MyCustomInlineAutoData(1337)]
        /// [MyCustomInlineAutoData(1337, 7)]
        /// [MyCustomInlineAutoData(1337, 7, 42)]
        /// public void CustomInlineDataSuppliesExtraValues(int x, int y, int z)
        /// {
        ///     Assert.Equal(1337, x);
        ///     // y can vary, so we can't express any meaningful assertion for it.
        ///     Assert.Equal(42, z);
        /// }
        ///
        /// private class MyCustomInlineAutoDataAttribute : InlineAutoDataAttribute
        /// {
        ///     public MyCustomInlineAutoDataAttribute(params object[] values) :
        ///         base(new MyCustomAutoDataAttribute(), values)
        ///     {
        ///     }
        /// }
        ///
        /// private class MyCustomAutoDataAttribute : AutoDataAttribute
        /// {
        ///     public MyCustomAutoDataAttribute() :
        ///         base(() => new Fixture().Customize(new TheAnswer()))
        ///     {
        ///     }
        ///
        ///     private class TheAnswer : ICustomization
        ///     {
        ///         public void Customize(IFixture fixture)
        ///         {
        ///             fixture.Inject(42);
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        protected InlineAutoDataAttribute(DataAttribute autoDataAttribute, params object[] values)
            : base(new InlineDataAttribute(values), autoDataAttribute)
        {
            this.AutoDataAttribute = autoDataAttribute;
            this.Values = values;
        }
    }
}
