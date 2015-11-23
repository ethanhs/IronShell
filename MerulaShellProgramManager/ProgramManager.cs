using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MerulaShellProgramManager.programs;
using MerulaShellProgramManager.shell;

namespace MerulaShellProgramManager
{
    public class ProgramManager
    {
        public List<IoItem> GetDesktopItems()
        {
            var list = new ProgramList(ShellPath.GetAllUsersDesktopFolderPath(),Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            return list.IoItems;
        }

        public List<IoItem> GetProgramMenu()
        {
            var list = new ProgramList(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                );
            return list.IoItems;
        }

        public List<IoItem> GetSpecialLocations()
        {
            var locations = (from drive in DriveInfo.GetDrives()
                                where drive.IsReady
                                select drive.RootDirectory.FullName).ToList();
            locations.AddRange(new[]
                                   {
                                       Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                                       Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                       Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                                       Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                                       Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)
                                   });
            
            return (from location in locations
                    where location != string.Empty
                    select new Folder(new DirectoryInfo(location))).Cast<IoItem>().ToList();
        }

        public List<IoItem> GetProgramMenu(string path)
        {
            if (path.EndsWith("Start Menu\\Programs"))
                return GetProgramMenu();
            var list = new ProgramList(path);
            list.AddRootFolder();
            return list.IoItems;
        }
    }
}
