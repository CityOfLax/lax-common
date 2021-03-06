﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lax.Helpers.Reflection.Enumerations {

    public static class EnumerationExtensions {

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Any()) ? (T)attributes.First() : null;
        }

        public static IEnumerable<T> GetAttributesOfType<T>(this Enum enumVal) where T : Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.AsEnumerable().Cast<T>();
        }

    }

}