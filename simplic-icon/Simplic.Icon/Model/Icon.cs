using System;
using System.Windows.Media.Imaging;

namespace Simplic.Icon
{
    /// <summary>
    /// Represents an icon saved in the database
    /// </summary>
    public class Icon
    {
        #region Private Members
        private BitmapImage iconBlobAsImage;
        #endregion

        #region Public Properties
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public byte[] IconBlob { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }        
        public BitmapImage IconBlobAsImage
        {
            get
            {
                if (iconBlobAsImage == null)
                {
                    if (IconBlob == null || IconBlob.Length <= 0) return null;

                    var img = new BitmapImage();
                    using (var ms = new System.IO.MemoryStream(IconBlob))
                    {
                        ms.Position = 0;
                        img.BeginInit();
                        img.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = null;
                        img.StreamSource = ms;
                        img.EndInit();
                    }
                    
                    img.Freeze();

                    iconBlobAsImage = img;
                }

                return iconBlobAsImage;
            }
        }
        #endregion
    }
}