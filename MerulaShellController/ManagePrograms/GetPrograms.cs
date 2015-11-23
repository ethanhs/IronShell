using System.Collections.Generic;
using MerulaShellProgramManager;
using MerulaShellProgramManager.programs;

namespace MerulaShellController.ManagePrograms
{
    public class GetPrograms
    {
        private readonly ProgramManager manager;

        public GetPrograms()
        {
            manager = new ProgramManager();
        }

        public List<IoItem> GetDesktopItems()
        {
            return manager.GetDesktopItems();
        }
    }
}
