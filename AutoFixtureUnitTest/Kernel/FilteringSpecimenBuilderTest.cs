﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FilteringSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummySpecification = new DelegatingRequestSpecification();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new FilteringSpecimenBuilder(dummyBuilder, dummySpecification);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderWillThrows()
        {
            // Fixture setup
            var dummySpecification = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(null, dummySpecification));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new FilteringSpecimenBuilder(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenSpecificationIsNotSatisfied()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, spec);
            var request = new object();
            // Exercise system            
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void SpecificationReceivesCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };

            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new FilteringSpecimenBuilder(dummyBuilder, specMock);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(expectedRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenFilterAllowsRequestThrough()
        {
            // Fixture setup
            var expectedResult = new object();
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new FilteringSpecimenBuilder(builder, spec);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreatePassesCorrectParametersToDecoratedBuilder()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContainer();
            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => true };
            var sut = new FilteringSpecimenBuilder(builderMock, spec);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
