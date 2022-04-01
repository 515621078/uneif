using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Eif表情包压解工具
{
    class Program
    {
        static void Main(string[] args)
        {
            var fin = new FileStream("QQ表情包 2022-04-01-20-20-44.eif", FileMode.Open);
            var file = new EifFile(fin);
            EifUnpack.UnpackNode(0, file, new DirectoryInfo("QQ表情包 2022-04-01-20-20-44"), fin);
        }
    }
}
