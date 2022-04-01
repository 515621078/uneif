using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    public class Const
    {
        public static uint SectSize = 512;
        public static uint DirEntrySize = 128;
        public static uint MiniSectSize = 64;
        public static uint FatEntriesInHeader = 109;
        public static uint FatEntriesPerSect = 127;

        public enum SectId : uint
        {
            Dif = 0xfffffffc,
            Fat = 0xfffffffd,
            EndOfChain = 0xfffffffe,
            Free = 0xffffffff,
        };
        public enum ObjType : byte
        {
            Unknown = 0x00,
            Storage = 0x01,
            Stream = 0x02,
            RootStorage = 0x05,
        };
        public enum ColorFlag : byte
        {
            Red = 0x00,
            Black = 0x01,
        };

        public enum StreamID : uint
        {
            MaxRegSid = 0xfffffffa,
            NoStream = 0xffffffff,
        };
    }
}
