using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.QueryModels {
    public sealed partial class QueryModel {
        private class TagWriter : IDisposable {

            private readonly string _tag;
            private readonly StringBuilder _sb;

            public TagWriter(string tag, StringBuilder sb) {
                _tag = tag;
                _sb = sb;

                _sb.Append("<" + _tag + ">");
            }

            public static void Write(string tag, StringBuilder sb, object innerValue) {
                if (innerValue == null) {
                    return;
                }

                sb.AppendFormat("<{0}>{1}</{0}>", tag, innerValue);
            }

            public static void Write(string tag, StringBuilder sb, IEnumerable<object> innerValues) {
                if (innerValues == null) {
                    return;
                }

                var list = innerValues.ToList();

                if (!list.Any()) {
                    return;
                }

                sb.AppendFormat("<{0}>{1}</{0}>", tag, list.JoinToString(""));
            }

            public void Dispose() {
                _sb.Append("</" + _tag + ">");
            }

        }

    }

}