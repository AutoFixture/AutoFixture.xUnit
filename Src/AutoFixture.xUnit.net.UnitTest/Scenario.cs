﻿using System;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

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

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
        }

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotEqual("foo", s3);
            Assert.NotEqual("bar", s3);
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

        [Theory, AutoData]
        public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
#pragma warning disable 0618
            [Frozen(As = typeof(AbstractType))]ConcreteType p1,
#pragma warning restore 0618
            AbstractType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
#pragma warning disable 0618
            [Frozen(As = null)]ConcreteType p1,
#pragma warning restore 0618
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterShouldAssignSameInstanceToSecondParameter(
            [Frozen]string p1,
            string p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ExactType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            AbstractType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            IInterface p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByParameterNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName, "parameter")]string p1,
            SingleParameterType<object> p2)
        {
            Assert.Equal(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByPropertyNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName, "Property")]string p1,
            PropertyHolder<object> p2)
        {
            Assert.Equal(p1, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByFieldNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName, "Field")]string p1,
            FieldHolder<object> p2)
        {
            Assert.Equal(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)]string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName, "parameter")]string p1,
            SingleParameterType<object> p2)
        {
            Assert.Equal(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName, "Property")]string p1,
            PropertyHolder<object> p2)
        {
            Assert.Equal(p1, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName, "Field")]string p1,
            FieldHolder<object> p2)
        {
            Assert.Equal(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName)]string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }
    }
}
