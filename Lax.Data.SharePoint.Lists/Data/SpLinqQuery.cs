﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data {

    internal class SpLinqQuery<T> : IOrderedQueryable<T> {

        public SpLinqQuery(Expression expression) {
            Guard.CheckNotNull(nameof(expression), expression);
            Guard.CheckIsTypeAssignableTo<IQueryable<T>>(nameof(expression), expression.Type);

            Expression = expression;
        }

        public IEnumerator<T> GetEnumerator() {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Expression Expression { get; }
        public Type ElementType => typeof(T);
        public IQueryProvider Provider => SpLinqQueryProvider.Instance;

    }

}