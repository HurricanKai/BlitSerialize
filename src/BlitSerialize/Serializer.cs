using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlitSerialize
{
    public static class Serializer
    {
        public static Span<byte> Serialize(TypeMap map, object obj, bool bigEndian = true)
        {
            var result = new Span<byte>(new byte[map.Length]);

            foreach (NodeInfo node in map)
            {
                WriteType(ref result, node.NodeType, bigEndian, node.Offset, GetValue(node, obj));
            }
            return result;
        }

        private static object GetValue(NodeInfo node, object obj)
        {
            var current = node;
            var stack = new Stack<NodeInfo>();
            do
            {
                stack.Push(current);
                current = current.Parent;
            }
            while (current != null && current.MemberInfo != null);

            var currentObj = obj;
            while (stack.Count > 0)
                currentObj = GetObj(stack.Pop().MemberInfo, currentObj);

            return currentObj;
        }

        private static Object GetObj(MemberInfo memberInfo, Object currentObj)
        {
            if (memberInfo is FieldInfo field)
                return field.GetValue(currentObj);
            if (memberInfo is PropertyInfo property)
                return property.GetValue(currentObj);
            return currentObj; // TODO: Exception? 
        }

        private static void WriteType(ref Span<byte> result, Type type, bool bigEndian, Int32 offset, object value)
        {
            if (type == typeof(byte))
            {
                result[offset] = (byte)value;
                return;
            }
            if (type == typeof(sbyte))
            {
                result[offset] = (byte)(sbyte)value;
                return;
            }
            if (type == typeof(UInt16))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteUInt16BigEndian(result.Slice(offset), (UInt16)value);
                else
                    BinaryPrimitives.WriteUInt16LittleEndian(result.Slice(offset), (UInt16)value);
                return;
            }
            if (type == typeof(Int16))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteInt16BigEndian(result.Slice(offset), (Int16)value);
                else
                    BinaryPrimitives.WriteInt16LittleEndian(result.Slice(offset), (Int16)value);
                return;
            }
            if (type == typeof(UInt32))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteUInt32BigEndian(result.Slice(offset), (UInt32)value);
                else
                    BinaryPrimitives.WriteUInt32LittleEndian(result.Slice(offset), (UInt32)value);
                return;
            }
            if (type == typeof(Int32))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteInt32BigEndian(result.Slice(offset), (Int32)value);
                else
                    BinaryPrimitives.WriteInt32LittleEndian(result.Slice(offset), (Int32)value);
                return;
            }
            if (type == typeof(UInt64))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteUInt64BigEndian(result.Slice(offset), (UInt64)value);
                else
                    BinaryPrimitives.WriteUInt64LittleEndian(result.Slice(offset), (UInt64)value);
                return;
            }
            if (type == typeof(Int64))
            {
                if (bigEndian)
                    BinaryPrimitives.WriteInt64BigEndian(result.Slice(offset), (Int64)value);
                else
                    BinaryPrimitives.WriteInt64LittleEndian(result.Slice(offset), (Int64)value);
                return;
            }
            if (type == typeof(Single))
            {
                var val = BitConverter.SingleToInt32Bits((Single)value);
                if (bigEndian)
                    BinaryPrimitives.WriteInt32BigEndian(result.Slice(offset), (Int32)value);
                else
                    BinaryPrimitives.WriteInt32LittleEndian(result.Slice(offset), (Int32)value);
                return;
            }
            if (type == typeof(Double))
            {
                var val = BitConverter.DoubleToInt64Bits((Double)value);
                if (bigEndian)
                    BinaryPrimitives.WriteInt64BigEndian(result.Slice(offset), (Int64)value);
                else
                    BinaryPrimitives.WriteInt64LittleEndian(result.Slice(offset), (Int64)value);
                return;
            }
        }
    }
}
