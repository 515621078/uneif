using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    class EifReader
    {
        public static byte[] ReadBytes(FileStream fs, long length) 
        { 
            var b = new byte[length]; fs.Read(b, 0, b.Length); return b; 
        }
        public static uint[] BufferToU32(byte[] buffer)
        {
            var intArray = new uint[buffer.Length / sizeof(uint)];
            Buffer.BlockCopy(buffer, 0, intArray, 0, buffer.Length);
            return intArray;
        }
        public static Header GetHeader(FileStream fs)
        {
            return new Header(ReadBytes(fs,512));
        }
        public static List<uint> GetDiFat(FileStream fs, Header header)
        {
            List<uint> dif = new List<uint>();

            dif.AddRange(header.FatSects);

            if (header.DifSectNum > 0)
            {
                while (true)
                {
                    //数组池
                    dif.AddRange(BufferToU32(ReadBytes(fs, Const.FatEntriesPerSect * sizeof(uint))));
                    //区块
                    uint next = BitConverter.ToUInt32(ReadBytes(fs, sizeof(uint)));
                    //结束条件
                    if (next == (uint)Const.SectId.EndOfChain) { break; }
                    //区块偏移
                    fs.Seek((next + 1) * Const.SectSize, SeekOrigin.Begin);
                }
            }
            return dif;
        }
        public static List<uint> GetFat(FileStream fs, Header header, List<uint> dif)
        {
            List<uint> fat = new List<uint>();
            for (int i = 0; i < header.FatSectNum; i++)
            {
                fs.Seek((dif[i] + 1) * Const.SectSize, SeekOrigin.Begin);
                fat.InsertRange((int)(i * Const.SectSize / sizeof(uint)), BufferToU32(ReadBytes(fs, Const.SectSize)));
            }
            return fat;
        }
        public static List<uint> GetMiniFat(FileStream fs, Header header, List<uint> fat)
        {
            List<uint> minifat = new List<uint>();
            for (uint i = header.MiniFatBeginSect; i != (uint)Const.SectId.EndOfChain; i = fat[(int)i])
            {
                fs.Seek((i + 1) * Const.SectSize, SeekOrigin.Begin);
                minifat.InsertRange(minifat.Count, BufferToU32(ReadBytes(fs, Const.SectSize)));
            }
            return minifat;
        }
        public static List<DirEntry> GetDirEntries(FileStream fs, Header header, List<uint> fat)
        {
            List<DirEntry> dirs = new List<DirEntry>();
            for (uint i = header.DirBeginSect; i != (uint)Const.SectId.EndOfChain; i = fat[(int)i])
            {
                fs.Seek((i + 1) * Const.SectSize, SeekOrigin.Begin);

                var dir = ByteArrayToDir(ReadBytes(fs, Const.SectSize),128);
                dirs.AddRange(dir);
            }
            return dirs;
        }
        public static List<byte> GetMiniStream(FileStream fs, List<uint> fat, List<DirEntry> dirs)
        {
            List<byte> miniStream = new List<byte>();
            var eoc = (uint)Const.SectId.EndOfChain;
            for (uint i = dirs[0].StartSect; i != eoc; i = fat[(int)i])
            {
                fs.Seek((i + 1) * Const.SectSize, SeekOrigin.Begin);
                miniStream.AddRange(ReadBytes(fs, Const.SectSize));
            }
            return miniStream;
        }

        public static List<DirEntry> ByteArrayToDir(byte[] bytes, int batchSize)
        {
            List<DirEntry> list = new List<DirEntry>();
            for (int i = 0; i < bytes.Length; i += batchSize)
            {
                var sub = new byte[batchSize];
                Array.Copy(bytes, i, sub, 0, batchSize);
                list.Add(new DirEntry(sub));
            }
            return list;
        }
    }
}
