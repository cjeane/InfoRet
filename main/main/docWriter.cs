using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    static class docWriter
    {
        static public void writeDoc(string location, Doc doc)
        {
            string temp = "";
            foreach (var terms in doc.dict())
            {

                for (int i = 0; i < terms.Value; i++)
                {
                    temp += terms.Key + " ";
                }
                temp += "\n";
            }
            ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressLabel.Content = "Writing " + doc.getName();
            File.WriteAllText(doc.getFullName(), temp);
        }

        static public void createDoc(string path, string name, string text)
        {
            File.WriteAllText(path + name + ".txt", text);
            ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressLabel.Content = name + " created.";
        }

        static public bool deleteAllFiles(string path)
        {

            if (Directory.Exists(path))
            {
                DirectoryInfo direc = new DirectoryInfo(path);
                foreach (FileInfo file in direc.GetFiles())
                    file.Delete();
                foreach (DirectoryInfo dir in direc.GetDirectories())
                    dir.Delete(true);

                return true;
            }
            return false;

        }

    }


}
