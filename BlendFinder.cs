using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace BlendLauncher
{
    public class BlenderVersion
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ImageSource Icon => IconHelper.GetIcon(Path, IconHelper.ItemType.File, IconHelper.IconSize.Large, IconHelper.ItemState.Undefined);


    }

    public static class BlendFinder
    {
        private static List<BlenderVersion> _getBlenderVersions;

        public static List<BlenderVersion> GetInstalledVersions
        {
            get
            {
                _getBlenderVersions ??= SearchAllInstalledVersions();
                return _getBlenderVersions;
            }
        }

        public static string BlenderFoundationSearchPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Blender Foundation");

        private static List<BlenderVersion> SearchAllInstalledVersions()
        {
            if (!Directory.Exists(BlenderFoundationSearchPath))
            {
                return new List<BlenderVersion>();
            }

            return Directory.GetDirectories(BlenderFoundationSearchPath)
                .Select(dir => new BlenderVersion
                {
                    Name = Path.GetFileName(dir),
                    Path = Path.Combine(dir, "blender.exe")
                })
                .Where(v => File.Exists(v.Path))
                .Reverse()
                .ToList();
        }
    }
}