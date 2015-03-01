﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// An implementation of DataAttribute that composes other DataAttribute instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeDataAttribute : DataAttribute
    {
        private readonly IEnumerable<DataAttribute> attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(IEnumerable<DataAttribute> attributes)
            : this(attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(params DataAttribute[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IEnumerable<DataAttribute> Attributes
        {
            get { return this.attributes; }
        }

        /// <summary>
        /// Returns the composition of data to be used to test the theory. Favors the data returned
        /// by DataAttributes in ascending order. Data already returned is ignored on next
        /// DataAttribute returned data.
        /// </summary>
        /// <param name="methodUnderTest">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the theory data.
        /// </returns>
        /// <remarks>
        /// The number of test cases is set from the first DataAttribute theory length.
        /// </remarks>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            if (methodUnderTest == null)
            {
                throw new ArgumentNullException("methodUnderTest");
            }

            if (parameterTypes == null)
            {
                throw new ArgumentNullException("parameterTypes");
            }

            var dataSets = this.attributes
                .Select(attr => attr.GetData(methodUnderTest, parameterTypes))
                .Zip(dataSet => dataSet.Collapse().ToArray());

            int numberOfParameters = methodUnderTest.GetParameters().Length;

            foreach (var dataSet in dataSets)
            {
                if (dataSet.Length != numberOfParameters)
                    throw new InvalidOperationException(
                              string.Format(
                                  CultureInfo.CurrentCulture,
                                  "Error invoking {0}: Expected {1} parameters, got {2} parameters",
                                  methodUnderTest.Name, numberOfParameters, dataSet.Length));

                yield return dataSet;
            }
        }
    }
}
