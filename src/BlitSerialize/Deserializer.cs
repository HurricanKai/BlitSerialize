using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlitSerialize
{
    public static class Deserializer
    {
        public static void Deserialize<T>(TypeMap map, Span<byte> bytes, ref T target, bool bigEndian = true)
        {
            var v = (object)target;
            Deserialize(map, bytes, ref v);
            target = (T)v;
        }

        public static void Deserialize(TypeMap map, Span<byte> bytes, ref object target, bool bigEndian = true)
        {
            Reconstruct(map.Root, bytes, ref target, bigEndian);
        }

        private static void Reconstruct(NodeInfo currentRoot, Span<byte> data, ref object target, bool bigEndian)
        {
            var type = currentRoot.NodeType;
            var offset = currentRoot.Offset;
            if (type == typeof(byte))
            {
                target = (byte)data[offset];
                return;
            }
            if (type == typeof(sbyte))
            {
                target = (sbyte)data[offset];
                return;
            }
            if (type == typeof(UInt16))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(Int16))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(UInt32))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(Int32))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(UInt64))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadUInt64BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadUInt64LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(Int64))
            {
                if (bigEndian)
                    target = BinaryPrimitives.ReadInt64BigEndian(data.Slice(offset));
                else
                    target = BinaryPrimitives.ReadInt64LittleEndian(data.Slice(offset));
                return;
            }
            if (type == typeof(Single))
            {
                Int32 val;
                if (bigEndian)
                    val = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset));
                else
                    val = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset));
                target = BitConverter.Int32BitsToSingle(val);
                return;
            }
            if (type == typeof(Double))
            {
                Int64 val;
                if (bigEndian)
                    val = BinaryPrimitives.ReadInt64BigEndian(data.Slice(offset));
                else
                    val = BinaryPrimitives.ReadInt64LittleEndian(data.Slice(offset));
                target = BitConverter.Int64BitsToDouble(val);
                return;
            }

            // complex type
            foreach (var child in currentRoot.Children)
            {
                object obj;
                if (child.MemberInfo is FieldInfo field1)
                    obj = field1.GetValue(target);
                else if (child.MemberInfo is PropertyInfo property1)
                    obj = property1.GetValue(target);
                else
                    throw new Exception();

                Reconstruct(child, data, ref obj, bigEndian);

                if (child.MemberInfo is FieldInfo field2)
                    field2.SetValue(target, obj);
                else if (child.MemberInfo is PropertyInfo property2)
                    property2.SetValue(target, obj);
                else
                    throw new Exception();
            }
        }
    }
}
