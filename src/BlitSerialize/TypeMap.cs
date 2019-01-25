using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlitSerialize
{
    [DebuggerDisplay("TypeMap ({Length})")]
    public class TypeMap : IEnumerable<NodeInfo>
    {
        public int Length { get; internal set; }
        public NodeInfo Root { get; internal set; }
        public IEnumerable<NodeInfo> Nodes => _forward.Values;


        private readonly Dictionary<int, NodeInfo> _forward = new Dictionary<int, NodeInfo>();
        private readonly Dictionary<NodeInfo, int> _reverse = new Dictionary<NodeInfo, int>();
        private readonly Dictionary<int, MemberInfo> _memberForward = new Dictionary<int, MemberInfo>();
        private readonly Dictionary<MemberInfo, int> _memberReverse = new Dictionary<MemberInfo, int>();

        public NodeInfo this[int key]
            => _forward[key];

        public int this[NodeInfo key]
            => _reverse[key];

        public MemberInfo GetMember(int key)
            => _memberForward[key];

        public int GetOffset(MemberInfo key)
            => _memberReverse[key];

        public void Add(int t1, NodeInfo t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
            _memberForward.Add(t1, t2.MemberInfo);
            _memberReverse.Add(t2.MemberInfo, t1);
        }

        public IEnumerator<NodeInfo> GetEnumerator()
        {
            return this._forward.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._forward.Values.GetEnumerator();
        }
    }
}
