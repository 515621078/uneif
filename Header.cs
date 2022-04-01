using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    public class Header
    {
        private byte[] magic = new byte[8];
        private byte[] clsid = new byte[16];

        private ushort minorVersion;
        private ushort dllVersion;
        private ushort bom;
        private ushort log2SectorSize;
        private ushort log2MiniSectorSize;
        private ushort reserved0;

        private uint reserved1;
        private uint dirSectNum;
        private uint fatSectNum;
        private uint dirBeginSect;
        private uint signature;
        private uint miniSectorCutoff;
        private uint miniFatBeginSect;
        private uint miniFatSectNum;
        private uint difBeginSect;
        private uint difSectNum;
        private uint[] fatSects = new uint[109];

        public Header(byte[] header)
        {
            var pointer = 0;

            // Get specific bytes then shift pointer to next variable index
            Func<int, byte[]> Sub = (length) =>
            {
                var rt = header.Skip(pointer).Take(length).ToArray();
                pointer += length;
                return rt;
            };

            magic = 
                Sub(8);
            clsid =
                Sub(16);
            minorVersion = 
                BitConverter.ToUInt16(Sub(2));
            dllVersion = 
                BitConverter.ToUInt16(Sub(2));
            bom = 
                BitConverter.ToUInt16(Sub(2));
            log2SectorSize = 
                BitConverter.ToUInt16(Sub(2));
            log2MiniSectorSize = 
                BitConverter.ToUInt16(Sub(2));
            reserved0 = 
                BitConverter.ToUInt16(Sub(2));

            reserved1 = 
                BitConverter.ToUInt32(Sub(4));
            dirSectNum = 
                BitConverter.ToUInt32(Sub(4));
            fatSectNum = 
                BitConverter.ToUInt32(Sub(4));
            dirBeginSect = 
                BitConverter.ToUInt32(Sub(4));
            signature = 
                BitConverter.ToUInt32(Sub(4));
            miniSectorCutoff = 
                BitConverter.ToUInt32(Sub(4));
            miniFatBeginSect = 
                BitConverter.ToUInt32(Sub(4));
            miniFatSectNum = 
                BitConverter.ToUInt32(Sub(4));
            difBeginSect = 
                BitConverter.ToUInt32(Sub(4));
            difSectNum = 
                BitConverter.ToUInt32(Sub(4));

            List<uint> fatSects_List = new List<uint>();
            for(;pointer<header.Length;) //Do not add pointer !!!! Pointer shifted by Sub(4) !!!!!!!!!!!!!!!!
            {
                fatSects_List.Add(BitConverter.ToUInt32(Sub(4)));
            }
            fatSects = fatSects_List.ToArray();
        }

        public byte[] Magic { get => magic; }
        public byte[] Clsid { get => clsid; }

        public ushort MinorVersion { get => minorVersion; }
        public ushort DllVersion { get => dllVersion; }
        public ushort Bom { get => bom; }
        public ushort Log2SectorSize { get => log2SectorSize; }
        public ushort Log2MiniSectorSize { get => log2MiniSectorSize; }
        public ushort Reserved0 { get => reserved0; }

        public uint Reserved1{ get => reserved1; }
        public uint DirSectNum{ get => dirSectNum; }
        public uint FatSectNum{ get => fatSectNum; }
        public uint DirBeginSect{ get => dirBeginSect; }
        public uint Signature{ get => signature; }
        public uint MiniSectorCutoff{ get => miniSectorCutoff; }
        public uint MiniFatBeginSect{ get => miniFatBeginSect; }
        public uint MiniFatSectNum{ get => miniFatSectNum; }
        public uint DifBeginSect{ get => difBeginSect; }
        public uint DifSectNum{ get => difSectNum; }
        public uint[] FatSects { get => fatSects; }
    }
}
