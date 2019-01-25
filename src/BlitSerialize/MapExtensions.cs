using System;
using System.Collections.Generic;
using System.Text;

namespace BlitSerialize
{
    public static class MapExtensions
    {
        public static Span<byte> Serialize(this TypeMap map, object obj, bool bigEndian = true)
            => Serializer.Serialize(map, obj, bigEndian);

        public static void Deserialize<T>(this TypeMap map, Span<byte> bytes, ref T target, bool bigEndian = true)
            => Deserializer.Deserialize(map, bytes, ref target);

        public static void Deserialize(this TypeMap map, Span<byte> bytes, ref object target, bool bigEndian = true)
            => Deserializer.Deserialize(map, bytes, ref target, bigEndian);
    }
}
