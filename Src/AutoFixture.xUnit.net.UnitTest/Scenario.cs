﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class Scenario
    {
        [Theory, AutoData]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.NotEqual(0, primitiveValue);
        }

        [Theory, AutoData]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.True(text.StartsWith("text"));
        }

        [Theory, AutoData]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Theory, AutoData]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<OperatingSystem> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Theory, AutoData(typeof(CustomizedFixture))]
        public void AutoDataProvidesCustomizedObject(PropertyHolder<string> ph)
        {
            Assert.Equal("Ploeh", ph.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
        {
            Assert.Equal(g1, g2);
        }

        [Theory, AutoData]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen]Guid g2, Guid g3)
        {
            Assert.NotEqual(g1, g2);
            Assert.NotEqual(g1, g3);

            Assert.Equal(g2, g3);
        }

        [Theory, AutoData]
        public void IntroductoryTest(
            int expectedNumber, MyClass sut)
        {
            // Fixture setup
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.Equal(expectedNumber, result);
            // Teardown
        }

        [Theory, AutoData]
        public void ModestCreatesParameterWithModestConstructor([Modest]MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.Equal(0, p.Number);
        }

        [Theory, AutoData]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy]MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.NotEqual(0, p.Number);
        }

        [Theory, AutoData]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter([Frozen][Greedy]MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.NotEqual(0, p2.Number);
        }

        [Theory, AutoData]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen]int[] numbers, [FavorArrays]ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }
    }
}
