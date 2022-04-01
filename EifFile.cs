using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    public class EifFile
    {
        private Header header;
        private List<uint> dif;
        private List<uint> fat;
        private List<uint> miniFat;
        private List<DirEntry> dirs;
        private List<byte> miniStream;

        public Header Header { get => header; }
        public List<uint> Dif { get => dif; }
        public List<uint> Fat { get => fat; }
        public List<uint> MiniFat { get => miniFat; }
        public List<DirEntry> Dirs { get => dirs; }
        public List<byte> MiniStream { get => miniStream; }

        public EifFile (FileStream fin)
        {
            // Read Header
            header = EifReader.GetHeader(fin);

            // Read DIFAT
            dif = EifReader.GetDiFat(fin, header);

            // Read FAT
            fat = EifReader.GetFat(fin, header, dif);

            // Read Mini FAT
            miniFat = EifReader.GetMiniFat(fin, header, fat);

            // Read Dir Entires
            dirs = EifReader.GetDirEntries(fin, header, fat);

            // Read Mini Stream
            miniStream = EifReader.GetMiniStream(fin, fat, dirs);
        }
    }
}