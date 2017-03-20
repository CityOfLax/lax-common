﻿// The MIT License (MIT)
// 
// Copyright (c) 2015-2017 Rasmus Mikkelsen
// Copyright (c) 2015-2017 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace Lax.EventFlow.Core {

    public class RetryDelay {

        private static readonly Random Random = new Random();

        public static RetryDelay Between(TimeSpan min, TimeSpan max) {
            return new RetryDelay(min, max);
        }

        private RetryDelay(
            TimeSpan min,
            TimeSpan max) {
            if (min.Ticks < 0) throw new ArgumentOutOfRangeException(nameof(min), "Minimum cannot be negative");
            if (max.Ticks < 0) throw new ArgumentOutOfRangeException(nameof(max), "Maximum cannot be negative");

            Min = min;
            Max = max;
        }

        public TimeSpan Max { get; }
        public TimeSpan Min { get; }

        public TimeSpan PickDelay() {
            return
                Min.Add(TimeSpan.FromMilliseconds((Max.TotalMilliseconds - Min.TotalMilliseconds) * Random.NextDouble()));
        }

    }

}