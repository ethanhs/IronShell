using System;
using System.Windows;
using MerulaShell.workspace;

namespace MerulaShellUi.workspace
{
    class WorkspaceSetter
    {
        private readonly TaskWindow taskWindow;
        private readonly MenuWindow menu;
        private readonly Desktop desktop;
        private readonly SharedSettings settings;

        public WorkspaceSetter(TaskWindow taskWindow, MenuWindow menu, Desktop desktop)
        {
            this.taskWindow = taskWindow;
            this.menu = menu;
            this.desktop = desktop;
            settings = SharedSettings.GetInstance();
            if (settings.TaskbarAlwaysVisible)
                taskWindow.SizeChanged += TaskWindowSizeChanged;
            menu.SizeChanged += TaskWindowSizeChanged;

            DrawWorkSpace();
        }
        private void TaskWindowSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            DrawWorkSpace();
        }
        private void DrawWorkSpace()
        {
            if (settings.TaskbarAlwaysVisible)
                WorkArea.MakeNewDesktopArea(0, (int)menu.ActualHeight, 0, (int)taskWindow.ActualHeight);
            else
                WorkArea.MakeNewDesktopArea(0, (int)menu.ActualHeight, 0, 0);

           desktop.SetDesktopMargin(new Thickness(0,menu.ActualHeight,0,taskWindow.ActualHeight));
        }
    }
}
