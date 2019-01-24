using System;
using System.Collections.Generic;
using System.Text;

namespace BlitSerialize
{
    public static class MapExtensions
    {
        public static Span<byte> Serialize(this TypeMap map, object obj)
            => Serializer.Serialize(map, obj);
    }
}
