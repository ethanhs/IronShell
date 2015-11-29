using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Explore10;
using MahApps.Metro;
using MahApps.Metro.Controls;
using System.IO;
using MerulaShellProgramManager.programs;
using MerulaShellProgramManager.shell;

namespace MerulaShellUi.start
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private string _searchText = string.Empty;
        public StartWindow()
        {
            InitializeComponent();
            Height = SystemParameters.FullPrimaryScreenHeight - 60;
            FillStart();
        }

        private void FillStart()
        {
            var settings = SharedSettings.GetInstance();
            AllApps.Foreground = settings.UiForeBrush;
            Explore10.Foreground = settings.UiForeBrush;
            Background = settings.UiBrushGlass;
            ProgramList.Visibility = Visibility.Hidden;
            ProgramList.Height = SystemParameters.FullPrimaryScreenHeight - 210;
            ProgramList.VerticalAlignment = VerticalAlignment.Top;
            ProgramParent.Visibility = Visibility.Hidden;
            ProgramParent.Height = SystemParameters.FullPrimaryScreenHeight - 210;
        }

        private void Explore10_OnClick(object sender, RoutedEventArgs e)
        {
            var explore = new Explore10.MainWindow();
            explore.Show();
        }

        private void StartWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            TilesView.Visibility=Visibility.Hidden;
            _searchText += e.Key;
            var ret= start.Search.GetSearchResults(_searchText);
            Debug.WriteLine(ret);


            
        }

        private void Explorer_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer");
        }

        private void AllApps_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProgramList.Visibility == Visibility.Hidden)
            {
                ProgramList.Visibility = Visibility.Visible;
                ProgramParent.Visibility = Visibility.Visible;
                var StartPrograms = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs";
                AllPrograms.Margin = new Thickness { Left = 5, Top = SystemParameters.FullPrimaryScreenHeight - 210 };
                //fill with the contents for start menu
                var info = new DirectoryInfo(StartPrograms);
                foreach (var file in info.GetFiles())
                {
                    var imgsrc = SmartThumnailProvider.GetThumbInt(file.FullName, 24, 24, ThumbOptions.BiggerOk);
                    var tile= new Tile
                    {
                        Margin = new Thickness {Left=10, Top = 1, Right = 10},
                        Width = 150,
                        Height = 30,
                    };
                    
                    var panel = new StackPanel
                    {
                        Width = 150,
                        Orientation = Orientation.Horizontal,
                    };
                    var bord = new Border
                    {
                        Width = 25,
                        Height = 25,
                        Background = new SolidColorBrush(Color.FromRgb(0x56,0x56,0x56)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };
                    var img = new Image
                    {
                        Width = 25,
                        Height = 25,
                        Source = imgsrc,
                        Stretch = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        
                    };
                    var text =  new TextBlock
                    {
                        Text = file.Name,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 120,
                        FontSize = 14,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness { Left=4},
                    };
                    bord.Child = img;
                    panel.Children.Add(bord);
                    panel.Children.Add(text);
                    tile.Content = panel;
                    ProgramList.Children.Add(tile);

                }
                foreach (var folder in info.GetDirectories())
                {
                    var imgsrc = SmartThumnailProvider.GetThumbInt(folder.FullName, 24, 24, ThumbOptions.BiggerOk);
                    var tile = new Tile
                    {
                        Margin = new Thickness { Left = 10, Top = 1, Right = 10 },
                        Width = 150,
                        Height = 30,
                    };

                    var panel = new StackPanel
                    {
                        Width = 150,
                        Orientation = Orientation.Horizontal,
                    };
                    var bord = new Border
                    {
                        Width = 25,
                        Height = 25,
                        Background = new SolidColorBrush(Color.FromRgb(0x56, 0x56, 0x56)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };
                    var img = new Image
                    {
                        Width = 25,
                        Height = 25,
                        Source = imgsrc,
                        Stretch = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Left,

                    };
                    var text = new TextBlock
                    {
                        Text = folder.Name,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 120,
                        FontSize = 14,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness { Left = 4 },
                    };
                    bord.Child = img;
                    panel.Children.Add(bord);
                    panel.Children.Add(text);
                    tile.Content = panel;
                    ProgramList.Children.Add(tile);

                }

            }
            else
            {
                ProgramList.Visibility = Visibility.Hidden;
                ProgramParent.Visibility = Visibility.Hidden;
                AllPrograms.Margin = new Thickness{Left = 5};

            }
            

        }


        private void StartWindow_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
