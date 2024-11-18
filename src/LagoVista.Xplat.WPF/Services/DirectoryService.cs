using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class DirectoryService : IDirectoryServices
    {
        public List<File> GetFiles(string directory)
        {
            var fileList = new List<File>();
            var files = System.IO.Directory.GetFiles(directory).ToList();


            foreach (var file in files)
            {
                fileList.Add(new File()
                {
                    Directory = directory,
                    FileName = file
                });
            }

            return fileList;
        }

        public List<Folder> GetFolders(string parent)
        {
            var fldrs = new List<Folder>();
            var dirs = System.IO.Directory.GetDirectories(parent);
            foreach(var dir in dirs)
            {
                var parts = dir.Split('\\');
                fldrs.Add(new Folder()
                {
                    FullPath = dir,
                });
            }

            return fldrs;
        }
    }
}
