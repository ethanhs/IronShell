using System;
using System.Collections.Generic;
using System.IO;
using MerulaShellProgramManager.programs;
using System.Linq;

namespace MerulaShellProgramManager
{
    class ProgramList
    {
        private readonly string[] directories;

        public ProgramList(params string[] directories)
        {
            IoItems = new List<IoItem>();
            this.directories = directories;
            LoadIoItems();
        }
        /// <summary>
        /// Loads all of the io items
        /// </summary>
        private void LoadIoItems()
        {
            foreach (var directory in directories)
            {
                LoadDirectory(directory);
            }
            IoItems = IoItems.OrderBy(i => i.IsFolder()).ThenBy(i=>i.Name).ToList();
        }
        /// <summary>
        /// Loads a directory
        /// </summary>
        /// <param name="directory"></param>
        private void LoadDirectory(string directory)
        {
            var i = new DirectoryInfo(directory);
            foreach (var directoryInfo in i.GetDirectories())
            {

                if (HasItem(directoryInfo.Name) || directoryInfo.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                IoItems.Add(new Folder(directoryInfo));
            }
            foreach (var fileInfo in i.GetFiles())
            {
                if (HasItem(fileInfo.Name) || fileInfo.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                IoItems.Add(new Program(fileInfo));
            }
        }
        /// <summary>
        /// If the name already exists in the list 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool HasItem(string name)
        {
            return IoItems.Any(io => io.Name == name);
        }

        public List<IoItem> IoItems { get; private set; }

        public void AddRootFolder()
        {
            IoItems.Insert(0, new Folder(new DirectoryInfo(directories.First()).Parent));
        }
    }

   
}
