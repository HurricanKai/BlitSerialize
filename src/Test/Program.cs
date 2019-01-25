using BlitSerialize;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = Mapper.BuildMap<TestType>();
            var val = new TestType()
            {
                A = 11,
                B = 22,
                C = 33,
                Sub1 = new SubType1()
                {
                    A = 99,
                    B = 88,
                    C = 77,
                    D = 66
                },
                Sub2 = new SubType2()
                {
                    A = 250,
                    B = 200,
                    C = 150,
                    D = 100,
                }
            };
            const bool bigEndian = true;
            var bytes = map.Serialize(val, bigEndian);
            var res = new TestType();
            map.Deserialize(bytes, ref res, bigEndian);
        }
    }

    public struct TestType
    {
        public byte A;
        public byte B;
        public int C;
        public SubType1 Sub1;
        public SubType2 Sub2;
    }

    public struct SubType1
    {
        public byte A;
        public byte B;
        public byte C;
        public byte D;
    }

    public struct SubType2
    {
        public byte A;
        public byte B;
        public byte C;
        public byte D;
    }
}
