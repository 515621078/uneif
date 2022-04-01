using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    public class DirEntry
    {
        private string name; // byte[64]
        private char nameLen;
        private Const.ObjType objType;
        private Const.ColorFlag colorFlag;
        private uint leftSiblingId;
        private uint rightSiblingId;
        private uint childId;
        private char[] clsid = new char[8];
        private uint state;
        private uint[] creationTime = new uint[2];
        private uint[] modifiedTime = new uint[2];
        private uint startSect;
        private ulong streamSize;

        public DirEntry(byte[] dirEntry)
        {
            var pointer = 0;

            // Get specific bytes then shift pointer to next variable index
            Func<int,byte[]> Sub = (length) => 
            {
                var rt = dirEntry.Skip(pointer).Take(length).ToArray();
                pointer += length;
                return rt;
            };

            // Read variables
            name =
                Encoding.Unicode.GetString(Sub(64));
            nameLen =
                BitConverter.ToChar(Sub(2));
            objType =
                (Const.ObjType)dirEntry[pointer++];
            colorFlag =
                (Const.ColorFlag)dirEntry[pointer++];
            leftSiblingId =
                BitConverter.ToUInt32(Sub(4));
            rightSiblingId =
                BitConverter.ToUInt32(Sub(4));
            childId =
                BitConverter.ToUInt32(Sub(4));
            clsid =
                new Func<char[]>(() =>
                {
                    var b = Sub(16);
                    char[] c = new char[b.Length / 2];
                    for (int i = 0; i < c.Length; i++)
                    {
                        c[i] = BitConverter.ToChar(new byte[] { b[2 * i], b[2 * i + 1] });
                    }
                    return c;
                })();
            state =
                BitConverter.ToUInt32(Sub(4));
            creationTime =
                new uint[] {
                    BitConverter.ToUInt32(dirEntry.Skip(pointer).Take(4).ToArray()),
                    BitConverter.ToUInt32(dirEntry.Skip(pointer+4).Take(4).ToArray())
                }; pointer += 8;
            modifiedTime =
                new uint[] {
                    BitConverter.ToUInt32(dirEntry.Skip(pointer).Take(4).ToArray()),
                    BitConverter.ToUInt32(dirEntry.Skip(pointer+4).Take(4).ToArray())
                }; pointer += 8;
            startSect =
                BitConverter.ToUInt32(Sub(4));
            streamSize =
                BitConverter.ToUInt64(Sub(8));
        }

        public string Name { get => name; }
        public char NameLen { get => nameLen; }
        public Const.ObjType ObjType { get => objType; }
        public Const.ColorFlag ColorFlag { get => colorFlag; }
        public uint LeftSiblingId { get => leftSiblingId; }
        public uint RightSiblingId { get => rightSiblingId; }
        public uint ChildId { get => childId; }
        public char[] Clsid { get => clsid; }
        public uint State { get => state; }
        public uint[] CreationTime { get => creationTime; }
        public uint[] ModifiedTime { get => modifiedTime; }
        public uint StartSect { get => startSect; }
        public ulong StreamSize { get => streamSize; }
    }
}
