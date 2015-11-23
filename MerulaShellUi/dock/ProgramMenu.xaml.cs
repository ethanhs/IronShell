using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MerulaShellProgramManager;
using MerulaShellProgramManager.programs;

namespace MerulaShellUi.dock
{
    /// <summary>
    /// Interaction logic for ProgramMenu.xaml
    /// </summary>
    public partial class ProgramMenu : UserControl
    {
        public ProgramMenu()
        {
            InitializeComponent();
            LoadMenu();
        }

        private void LoadMenu()
        {
            var manager = new ProgramManager();
            var items = manager.GetProgramMenu();
            LoadMenuItems(items);

            var settings = SharedSettings.GetInstance();
            icon.Foreground = settings.UiForeBrushGlassL;
        }

        private void LoadMenuItems(IEnumerable<IoItem> items)
        {
            pnlPrograms.Children.Clear();
            foreach (var ioItem in items)
            {
                CreateButton(ioItem);
            }
        }

        private void CreateButton(IoItem item)
        {
            var settings = SharedSettings.GetInstance();

            var content = new WrapPanel();
            content.Children.Add(new Image { Source = item.GetSmallIcon(), Height = 16, Width = 16, Margin = new Thickness(2) });
            content.Children.Add(new TextBlock { Text = item.Name, Margin = new Thickness(2), TextTrimming = TextTrimming.CharacterEllipsis, Width = 70});
            var btnProgram = new Button
            {
                Content = content,
                Tag = item,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Foreground = settings.UiForeBrush,
                Background = settings.UiGradientBrushV,
                Margin = new Thickness(5),
                Width = 100,
                ToolTip = item.Name
            };
            btnProgram.Click+=BtnProgramClick;
            pnlPrograms.Children.Add(btnProgram);
        }

        private void BtnProgramClick(object sender, RoutedEventArgs e)
        {
            var senderIoItem = (IoItem) ((Button) sender).Tag;
            if(!senderIoItem.IsFolder())
            {
                senderIoItem.Start();
                return;
            }

            var manager = new ProgramManager();
            var items = manager.GetProgramMenu(senderIoItem.Path);
            LoadMenuItems(items);
        }
    }
}
