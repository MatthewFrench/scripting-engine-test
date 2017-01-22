using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LeagueSandboxLua2CS
{
    class ClassWriter
    {
        private List<DirectoryInfo> Champions = new List<DirectoryInfo>();
        private DirectoryInfo RootDirectory;
        private string championLocation = "";
        public ClassWriter(string contentLocation, string classType)
        {
            championLocation = contentLocation + "\\Champions\\";
            RootDirectory = new DirectoryInfo(championLocation);
            foreach (DirectoryInfo dirinfo in RootDirectory.GetDirectories())
                Champions.Add(dirinfo);
            RemoveFiles();
            Write(classType);
        }
        private void RemoveFiles()
        {
            foreach(DirectoryInfo dirinfo in Champions)
            {
                foreach(FileInfo fileinfo in dirinfo.GetFiles())
                {
                    if (File.Exists(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs"))
                        File.Delete(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs");
                }
            }
        }
        private void Write(string classType)
        {
            StreamWriter sw;
            foreach (DirectoryInfo dirinfo in Champions)
            {
                foreach (FileInfo fileinfo in dirinfo.GetFiles())
                {
                    if (!File.Exists(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs"))
                        File.Create(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs").Close();
                    using (sw = new StreamWriter(File.Open(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs", FileMode.Append)))
                    {
                        sw.Write("using System;" + Environment.NewLine);
                        sw.Write("using System.Collections.Generic;" + Environment.NewLine);
                        sw.Write("using System.Linq;" + Environment.NewLine);
                        sw.Write("using System.Text;" + Environment.NewLine);
                        sw.Write("using System.Threading.Tasks;" + Environment.NewLine);
                        sw.Write("using System.Numerics;" + Environment.NewLine);
                        sw.Write("using LeagueSandbox.GameServer.Logic.GameObjects;" + Environment.NewLine);
                        sw.Write("using LeagueSandbox.GameServer.Logic.API;" + Environment.NewLine);
                    }
                    using (sw = new StreamWriter(File.Open(fileinfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + ".cs", FileMode.Append)))
                    {
                        sw.Write("namespace " + dirinfo.Name  + Environment.NewLine);
                        sw.Write("{" + Environment.NewLine);
                        sw.Write("    class " + Path.GetFileNameWithoutExtension(fileinfo.Name) + Environment.NewLine);
                        sw.Write("    {" + Environment.NewLine);
                        sw.Write("        " + classType + " void onStartCasting()" + Environment.NewLine);
                        sw.Write("        {" + Environment.NewLine);
                        sw.Write("        " + Environment.NewLine);
                        sw.Write("        }" + Environment.NewLine);
                        sw.Write("        " + classType + " void onFinishCasting()" + Environment.NewLine);
                        sw.Write("        {" + Environment.NewLine);
                        sw.Write("        " + Environment.NewLine);
                        sw.Write("        }" + Environment.NewLine);
                        sw.Write("        " + classType + " void applyEffects()" + Environment.NewLine);
                        sw.Write("        {" + Environment.NewLine);
                        sw.Write("        " + Environment.NewLine);
                        sw.Write("        }" + Environment.NewLine);
                        sw.Write("        " + classType + " void onUpdate(double diff)" + Environment.NewLine);
                        sw.Write("        {" + Environment.NewLine);
                        sw.Write("        " + Environment.NewLine);
                        sw.Write("        }" + Environment.NewLine);
                        sw.Write("    }" + Environment.NewLine);
                        sw.Write("}" + Environment.NewLine);
                    }
                }
            }
        }
    }
}
