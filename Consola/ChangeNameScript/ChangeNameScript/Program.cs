using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChangeNameScript
{
    class Program
    {

        static void Main(string[] args)
        {
            String dir = @"C:\Users\Administrator\Downloads";
            String strWith ="";
            String strReplace = "[smallpdf.com]";
            DirectoryInfo DirInfo = new DirectoryInfo(dir);
            FileInfo[] names = DirInfo.GetFiles();
            foreach (FileInfo f in names)
            {
                if (f.Name.Contains(strReplace))
                {
                    string newName = f.Name.Replace(strReplace, strWith);
                    File.Move(dir + "\\" + f.Name, dir + "\\" + newName);
                }

            }
        }


    }
}
