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

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            if (ChooserVersions.SelectedItem is BlenderVersion selected)
            {
                BlendStarter.Launch(selected.Path);
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
                BlendStarter.Launch(selected.Path);
                Application.Current.Shutdown();
            }
        }
    }
}