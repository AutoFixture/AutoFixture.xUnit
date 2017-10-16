﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2
{
    /// <summary>
    /// Provides auto-generated data specimens generated by AutoFixture as an extension to
    /// xUnit.net's Theory attribute.
    /// </summary>
    [DataDiscoverer(
        typeName: "Ploeh.AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer",
        assemblyName: "Ploeh.AutoFixture.Xunit2")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class AutoDataAttribute : DataAttribute
    {
        private readonly IFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor overload initializes the <see cref="Fixture"/> to an instance of
        /// <see cref="Fixture"/>.
        /// </para>
        /// </remarks>
        public AutoDataAttribute()
            : this(new Fixture())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class with an
        /// <see cref="IFixture"/> of the supplied type.
        /// </summary>
        /// <param name="fixtureType">The type of the composer.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="fixtureType"/> does not implement <see cref="IFixture"/>
        /// or does not have a default constructor.
        /// </exception>
        [Obsolete("This constructor overload is deprecated, and will be removed in a future version of AutoFixture. If you need to change the behaviour of the [AutoData] attribute, please create a derived attribute class and pass in a customized Fixture via the constructor that takes an IFixture instance.", true)]
        public AutoDataAttribute(Type fixtureType)
            : this(AutoDataAttribute.CreateFixture(fixtureType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class with the
        /// supplied <see cref="IFixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        protected AutoDataAttribute(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            this.fixture = fixture;
        }

        /// <summary>
        /// Gets the fixture used by <see cref="GetData"/> to create specimens.
        /// </summary>
        public IFixture Fixture
        {
            get { return this.fixture; }
        }

        /// <summary>
        /// Gets the type of <see cref="Fixture"/>.
        /// </summary>
        [Obsolete("This property is deprecated and will be removed in a future version of AutoFixture. Please use Fixture.GetType() instead.")]
        public Type FixtureType
        {
            get { return this.Fixture.GetType(); }
        }

        /// <summary>
        /// Returns the data to be used to test the theory.
        /// </summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>The theory data generated by <see cref="Fixture"/>.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException("testMethod");
            }

            var specimens = new List<object>();
            foreach (var p in testMethod.GetParameters())
            {
                this.CustomizeFixture(p);

                var specimen = this.Resolve(p);
                specimens.Add(specimen);
            }

            return new[] { specimens.ToArray() };
        }

        private void CustomizeFixture(ParameterInfo p)
        {
            var customizeAttributes = p.GetCustomAttributes()
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var c = ca.GetCustomization(p);
                this.Fixture.Customize(c);
            }
        }

        private object Resolve(ParameterInfo p)
        {
            var context = new SpecimenContext(this.Fixture);
            return context.Resolve(p);
        }

        private static IFixture CreateFixture(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!typeof(IFixture).GetTypeInfo().IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} is not compatible with IFixture. Please supply a Type which implements IFixture.",
                        type),
                    "type");
            }

            var ctor = type.GetTypeInfo().GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} has no default constructor. Please supply a a Type that implements IFixture and has a default constructor. Alternatively you can supply an IFixture instance through one of the AutoDataAttribute constructor overloads. If used as an attribute, this can be done from a derived class.",
                        type),
                    "type");
            }

            return (IFixture)ctor.Invoke(null);
        }
    }
}