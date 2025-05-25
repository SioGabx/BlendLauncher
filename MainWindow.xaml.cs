using BlendLauncherWPF;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BlendLauncher
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var versions = BlendFinder.GetInstalledVersions;
            ChooserVersions.ItemsSource = versions;

            if (versions.Any())
            {
                ChooserVersions.SelectedIndex = 0;
            }
        }

        public void Launch(string Path)
        {
            this.Close();
            BlendStarter.Launch(Path);
        }
        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            if (ChooserVersions.SelectedItem is BlenderVersion selected)
            {
                Launch(selected.Path);
            }
            else
            {
                MessageBox.Show("Sélectionnez une version.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChooserVersions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != ChooserVersions)
            {
                if (obj.GetType() == typeof(ListBoxItem))
                {
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }

            if (obj is ListBoxItem listBoxItem && listBoxItem.Content is BlenderVersion selected)
            {
                Launch(selected.Path);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Restore size but keep at screen center
            if (Properties.Settings.Default.WindowWidth > 0 && Properties.Settings.Default.WindowHeight > 0)
            {
                Left += ((ActualWidth - Properties.Settings.Default.WindowWidth) / 2);
                Top += ((ActualHeight - Properties.Settings.Default.WindowHeight) / 2);
                Width = Properties.Settings.Default.WindowWidth;
                Height = Properties.Settings.Default.WindowHeight;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WindowWidth = this.Width;
            Properties.Settings.Default.WindowHeight = this.Height;
            Properties.Settings.Default.Save(); //Settings saved at %localappdata%\BlendLauncher
        }
    }
}