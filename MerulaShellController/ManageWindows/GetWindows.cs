using System.Collections.Generic;
using MerulaShell.windows;

namespace MerulaShellController.ManageWindows
{
    class GetWindows
    {
        private MerulaShell.MerulaShell shell;

        public GetWindows()
        {
            shell = MerulaShell.MerulaShell.GetInstance();
        }

        /// <summary>
        /// Gets a list of active windows 
        /// </summary>
        /// <returns>A list of window objects</returns>
        public List<Window> GetActiveWindows()
        {
            return shell.GetActiveWindows();
        }
    }
}
