﻿using System;
using System.Collections.Generic;

namespace DocHelper.Domain.Helpers
{
    public static class TypeHelper
    {
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type StringType = typeof(string);
        public static readonly Type CharType = typeof(char);
        public static readonly Type NullableCharType = typeof(char?);
        public static readonly Type DateTimeType = typeof(DateTime);
        public static readonly Type NullableDateTimeType = typeof(DateTime?);
        public static readonly Type BoolType = typeof(bool);
        public static readonly Type NullableBoolType = typeof(bool?);
        public static readonly Type ByteArrayType = typeof(byte[]);
        public static readonly Type ByteType = typeof(byte);
        public static readonly Type SByteType = typeof(sbyte);
        public static readonly Type SingleType = typeof(float);
        public static readonly Type DecimalType = typeof(decimal);
        public static readonly Type Int16Type = typeof(short);
        public static readonly Type UInt16Type = typeof(ushort);
        public static readonly Type Int32Type = typeof(int);
        public static readonly Type UInt32Type = typeof(uint);
        public static readonly Type Int64Type = typeof(long);
        public static readonly Type UInt64Type = typeof(ulong);
        public static readonly Type DoubleType = typeof(double);

        public static Type ResolveType(string fullTypeName, Type expectedBase = null)
        {
            if (String.IsNullOrEmpty(fullTypeName))
                return null;

            var type = Type.GetType(fullTypeName);
            if (type == null)
            {
                return null;
            }

            if (expectedBase != null && !expectedBase.IsAssignableFrom(type))
            {
                return null;
            }

            return type;
        }

        private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string> {
            { StringType, "string" },
            { BoolType, "bool" },
            { ByteType, "byte" },
            { SByteType, "sbyte" },
            { CharType, "char" },
            { DecimalType, "decimal" },
            { DoubleType, "double" },
            { SingleType, "float" },
            { Int16Type, "short" },
            { Int32Type, "int" },
            { Int64Type, "long" },
            { ObjectType, "object" },
            { UInt16Type, "ushort" },
            { UInt32Type, "uint" },
            { UInt64Type, "ulong" }
        };
    }
}