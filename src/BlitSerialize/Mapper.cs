using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlitSerialize
{
    public static class Mapper
    {
        public static TypeMap BuildMap<T>() where T : struct
        {
            return BuildMap(typeof(T));
        }

        public static TypeMap BuildMap(Type rootType)
        {
            if (rootType is null)
                throw new ArgumentNullException(nameof(rootType));

            if (!rootType.IsValueType)
                throw new ArgumentException("Type needs to be struct", nameof(rootType));

            var finalOffset = 0;
            var typeInfo = BuildNode(rootType, ref finalOffset);

            var map = new TypeMap();
            var stack = new Stack<NodeInfo>();
            stack.Push(typeInfo);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current.Children == null || current.Children.Length == 0)
                    map.Add(current.Offset, current);
                else if (current.Children != null && current.Children.Length > 0)
                    foreach (var child in current.Children)
                    {
                        child.Parent = current; // just setting this now shrug
                        stack.Push(child);
                    }
            }
            map.Length = finalOffset;
            return map;
        }

        private static NodeInfo[] BuildNodesFromType(Type baseType, ref int offset)
        {
            const BindingFlags bindingFlags =
                BindingFlags.GetField | BindingFlags.SetField |
                BindingFlags.GetProperty | BindingFlags.SetProperty |
                BindingFlags.Instance | BindingFlags.Public;

            var members = baseType.GetMembers(bindingFlags)
                .Where(x => x is FieldInfo || x is PropertyInfo)
                .Select(x =>
                {
                    if (x is FieldInfo field) return (field.FieldType, x);
                    if (x is PropertyInfo property) return (property.PropertyType, x);
                    throw new Exception(); // wont happen
                }).ToArray();
            var res = new NodeInfo[members.Length];
            for (var i = 0; i < members.Length; i++)
            {
                res[i] = BuildNode(members[i].Item1, ref offset);
                res[i].MemberInfo = members[i].x;
            }
            return res;
        }

        private static Dictionary<Type, int> lengthMappings = new Dictionary<Type, int>()
        {
            { typeof(byte), sizeof(byte) },
            { typeof(sbyte), sizeof(sbyte) },
            { typeof(UInt16), sizeof(UInt16) },
            { typeof(Int16), sizeof(Int16) },
            { typeof(UInt32), sizeof(UInt32) },
            { typeof(Int32), sizeof(Int32) },
            { typeof(UInt64), sizeof(UInt64) },
            { typeof(Int64), sizeof(Int64) },
            { typeof(Single), sizeof(Single) },
            { typeof(Double), sizeof(Double) }
        };

        private static NodeInfo BuildNode(Type type, ref int offset)
        {
            if (lengthMappings.TryGetValue(type, out int defaultOffset))
            {
                var v = new NodeInfo()
                {
                    NodeType = type,
                    Offset = offset,
                };
                offset += defaultOffset;
                return v;
            }


            return new NodeInfo()
            {
                NodeType = type,
                Offset = offset,
                Children = BuildNodesFromType(type, ref offset)
            };
        }
    }
}
