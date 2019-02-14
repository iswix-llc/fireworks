///////////////////////////////////////////////
// Copyright (C) 2010-2019 ISWIX, LLC
// Web: http://www.iswix.com
// All Rights Reserved
///////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using FireworksFramework.Views;

namespace FireworksFramework.Managers
{
    public class FireworksManager
    {
        private static readonly Lazy<FireworksManager> lazy = new Lazy<FireworksManager>(() => new FireworksManager());
        public static FireworksManager FireworksManagerInstance { get { return lazy.Value; } }

        public string FilePath
        {
            get
            {
                // Empty if no parms passed, otherwise the File Path of the default document to be opened.
                string filePath;
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    filePath = args[1];
                }
                else
                {
                    filePath = string.Empty;
                }
                return filePath;
            }
        }
        public BitmapImage BrandingBitMap { get; set; }
        public string ProductName { get; set; }
        private FireworksManager()
        {
            try
            {
                Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            }
            catch (Exception)
            {
            }
        }
        public void Start()
        {
            FireworksView fireworksView = new FireworksView();
            fireworksView.ShowDialog();
        }
    }
}
