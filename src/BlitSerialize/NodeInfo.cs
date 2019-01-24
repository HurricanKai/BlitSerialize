using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlitSerialize
{
    public class NodeInfo
    {
        public Type NodeType;
        public MemberInfo MemberInfo;
        public int Offset;
        public NodeInfo Parent;
        public NodeInfo[] Children;
    }
}
