﻿using System;

namespace AutoFixture.Xunit2.UnitTest
{
    internal class DelegatingCustomization : ICustomization
    {
        internal DelegatingCustomization()
        {
            this.OnCustomize = f => { };
        }

        public void Customize(IFixture fixture)
        {
            this.OnCustomize(fixture);
        }

        internal Action<IFixture> OnCustomize { get; set; }
    }
}
