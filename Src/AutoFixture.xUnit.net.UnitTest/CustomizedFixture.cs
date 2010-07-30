﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class CustomizedFixture : Fixture
    {
        public CustomizedFixture()
        {
            this.Customize<PropertyHolder<string>>(c => c.With(x => x.Property, "Ploeh"));
        }
    }
}
