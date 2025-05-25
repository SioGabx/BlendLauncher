using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlendLauncher
{
    public static class IconHelper
    {
        private static class Interop
        {
            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFileInfo psfi, uint cbFileInfo, uint uFlags);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DestroyIcon(IntPtr hIcon);

            public const uint SHGFI_ICON = 0x000000100;
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            public const uint SHGFI_OPENICON = 0x000000002;
            public const uint SHGFI_SMALLICON = 0x000000001;
            public const uint SHGFI_LARGEICON = 0x000000000;
            public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            public const uint FILE_ATTRIBUTE_FILE = 0x00000100;

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct SHFileInfo
            {
                public IntPtr HIcon;
                public int IIcon;
                public uint DwAttributes;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string SzDisplayName;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string SzTypeName;
            }
        }

        public enum IconSize { Small, Large }
        public enum ItemState { Undefined, Open, Close }
        public enum ItemType { Folder, File }

        public static ImageSource GetIcon(string path, ItemType type, IconSize size, ItemState state)
        {
            uint flags = Interop.SHGFI_ICON | Interop.SHGFI_USEFILEATTRIBUTES;
            uint attributes = type == ItemType.Folder ? Interop.FILE_ATTRIBUTE_DIRECTORY : Interop.FILE_ATTRIBUTE_FILE;

            if (type == ItemType.Folder && state == ItemState.Open)
            {
                flags |= Interop.SHGFI_OPENICON;
            }

            flags |= size == IconSize.Small ? Interop.SHGFI_SMALLICON : Interop.SHGFI_LARGEICON;

            IntPtr result = Interop.SHGetFileInfo(path, attributes, out Interop.SHFileInfo shfi, (uint)Marshal.SizeOf(typeof(Interop.SHFileInfo)), flags);

            if (result == IntPtr.Zero)
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            try
            {
                const int IconPixelSize = 32;
                return Imaging.CreateBitmapSourceFromHIcon(shfi.HIcon, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(IconPixelSize, IconPixelSize));
            }
            finally
            {
                Interop.DestroyIcon(shfi.HIcon);
            }
        }
    }
}