using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRULE_Wrapper
{
    class Program
    {
        public static string WORKING_DIRECTORY = "";

        static void Main(string[] args)
        {
            WORKING_DIRECTORY = Directory.GetCurrentDirectory();

            string dataFile;
            string apkFile;

            // Define paths
            if (args.Length > 0)
            {
                dataFile = args[0];
            }
            else
            {
                dataFile = WORKING_DIRECTORY + "/data.win";

                if (!File.Exists(dataFile))
                {
                    dataFile = WORKING_DIRECTORY + "/game.droid";

                    if (!File.Exists(dataFile))
                    {
                        Console.WriteLine("Unable to find any data file (data.win nor game.droid)");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            // Define apk base file
            apkFile = WORKING_DIRECTORY + "/base.apk";

            if (!File.Exists(apkFile))
            {
                Console.WriteLine("Unable to find base.apk");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Extracting APK...");

            RunJar("wrappertools/apktool.jar d base.apk");

            Console.WriteLine("Copying files...");
            PushFiles("base/assets/");

            Console.WriteLine("Building apk file");

            RunJar("wrappertools/apktool.jar b base");

            FileInfo f = new FileInfo("base/dist/base.apk");
            string resultfile = WORKING_DIRECTORY + "/" + f.Name;
            f.CopyTo(resultfile, true);

            Directory.Delete("base", true);

            Console.WriteLine("Signing...");
            RunJar("wrappertools/signer.jar -w wrappertools/testkey.x509.pem wrappertools/testkey.pk8 base.apk base_signed.apk");

            //Process.Start("explorer.exe", string.Format("/select,\"{0}\"", resultfile));

            Console.WriteLine("Wrapped up nice for you! Press any key to exit");

            Console.ReadKey();
        }

        static Process RunJar(string args)
        {
            Process p = new Process();

            p.StartInfo.FileName = "java.exe";
            p.StartInfo.Arguments = "-jar " + args;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            return p;
        }

        static void PushFiles(string destpath, string sourcepath = "")
        {
            if (sourcepath == "")
            {
                sourcepath = WORKING_DIRECTORY;
            }

            Console.WriteLine(sourcepath);
            Console.ReadKey();

            string[] files = Directory.GetFiles(sourcepath);
            string[] directories = Directory.GetDirectories(sourcepath);
            string[] datafiles = files.Concat(directories).ToArray();

            foreach (string dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    FileInfo f = new FileInfo(dir);
                    Directory.CreateDirectory(destpath + f.Name);
                }
            }

            foreach (string file in datafiles)
            {
                if (file.EndsWith(".exe") || file.EndsWith(".apk"))
                    continue;

                FileInfo f = new FileInfo(file);
                string name = f.Name;

                if (name == "apktool.jar" || name == "base" || name == "wrappertools")
                    continue;

                if (name == "data.win")
                    name = "game.droid";

                Console.WriteLine(file);

                if (File.Exists(file))
                {
                    f.CopyTo(destpath + name, true);
                }
                else if (Directory.Exists(file))
                {
                    Console.WriteLine(destpath + "/" + name);
                    Console.WriteLine(sourcepath);
                    Console.ReadKey();
                    Directory.CreateDirectory(destpath + "/" + name);
                    PushFiles(destpath + "/" + name + "/", sourcepath + "/" + name);
                }
            }
        }
    }
}