﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class TriplePropertyHolder<T1, T2, T3>
    {
        public T1 Property1 { get; set; }

        public T2 Property2 { get; set; }

        public T3 Property3 { get; set; }
    }
}
