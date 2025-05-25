using BlendLauncherWPF;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace BlendLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length == 0)
            {
                MessageBox.Show("Aucun argument fourni.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            if (BlendFinder.GetInstalledVersions.Count == 0)
            {
                MessageBox.Show($"Aucune version de blender détéctée.\nChemin de recherche : {BlendFinder.BlenderFoundationSearchPath}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (BlendFinder.GetInstalledVersions.Count == 1)
            {
                BlendStarter.Launch(BlendFinder.GetInstalledVersions.FirstOrDefault().Path);
                Shutdown();
                return;
            }
            else
            {
                MainWindow window = new();
                window.Show();
            }
        }
    }
}