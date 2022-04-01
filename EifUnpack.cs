using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eif表情包压解工具
{
    class EifUnpack
    {
        public static void UnpackNode(int sid, EifFile file, DirectoryInfo outDir, FileStream fin)
        {
            if (sid == -1) { return; }
            DirEntry node = file.Dirs[sid];

            var dirpath = outDir.FullName + "\\" + node.Name.Substring(0, node.Name.IndexOf('\0'));
            DirectoryInfo curPath = (node.ObjType == Const.ObjType.RootStorage ? outDir : new DirectoryInfo(dirpath));

            switch (node.ObjType)
            {
                case Const.ObjType.Unknown:
                    break;
                case Const.ObjType.RootStorage:
                case Const.ObjType.Storage:
                    if (!curPath.Exists) { curPath.Create(); }
                    UnpackNode((int)node.ChildId, file, curPath, fin);
                    break;
                case Const.ObjType.Stream:
                    UnpackFile(node, file, curPath, fin);
                    break;
                default:
                    break;
            }
            UnpackNode((int)node.LeftSiblingId, file, outDir, fin);
            UnpackNode((int)node.RightSiblingId, file, outDir, fin);
        }
        public static void UnpackFile(DirEntry node, EifFile file, DirectoryInfo outpath, FileStream fin)
        {
            FileStream fs = new FileStream(outpath.FullName, FileMode.Create);

            byte[] buffer = new byte[Const.SectSize];

            ulong remaining = node.StreamSize;
            try
            {
                if (node.StreamSize < file.Header.MiniSectorCutoff)
                {
                    for (uint i = node.StartSect; remaining > 0; i = file.MiniFat[(int)i])
                    {
                        if (i == (uint)Const.SectId.EndOfChain) { Console.WriteLine("Eif Formate Error -> End of chain before sector"); break; }

                        ulong readSize = remaining < Const.MiniSectSize ? remaining : Const.MiniSectSize;

                        if (i * Const.MiniSectSize + readSize > (uint)file.MiniStream.Count) { Console.WriteLine("Eif Formate Error -> Mini_Sect + readSize > MiniStream"); break; }

                        if (node.StreamSize < file.Header.MiniSectorCutoff)
                        {
                            List<byte> mini = new List<byte>(file.MiniStream);
                            fs.Write(mini.ToArray(), (int)(i * Const.MiniSectSize), (int)readSize);
                        }
                        else
                        {
                            fin.Seek((i + 1) * Const.SectSize, SeekOrigin.Begin);
                            fin.Read(buffer, 0, (int)readSize);
                            if (!fin.CanRead){Console.WriteLine("Eif Formate Error -> FileStream cannot read");break;}
                            fs.Write(buffer, 0, (int)readSize);
                        }
                        fs.Flush();
                        remaining -= readSize;
                    }
                    fs.Close();
                }
            }
            finally { Console.WriteLine("Eif Formate Error - Formate exception"); }
        }
    }
}
