
using MediaMetadataExtractorLib.Main.Base;
using MediaMetadataExtractorLib.Util;

using MetadataExtractor.Formats.Exif;


using System;
using System.Drawing;
using System.IO;
using System.Linq;


using TagLib;

namespace MediaMetadataExtractorLib.Main
{
    public class ImageExtractor : MediaExtractorBase
    {
        private ExifDirectoryBase exifDir;
        private ExifSubIfdDirectory exifSubIfdDir;

        public ImageExtractor(int type, string path) : base(type, path)
        {
            if (MediaInformation.Height == null || MediaInformation.Width == null)
                this.ExtractImageSize();
        }

        public ImageExtractor(int type, Stream stream) : base(type, stream)
        {
            if (MediaInformation.Height == null || MediaInformation.Width == null)
                this.ExtractImageSize();
        }

        private void ExtractImageSize()
        {
            if (!string.IsNullOrEmpty(_path))
            {
                FileInfo file = new FileInfo(_path);
                var sizeInBytes = file.Length;
                Bitmap img = new Bitmap(_path);
                if (img != null)
                {
                    MediaInformation.Height = img.Height.ToString();
                    MediaInformation.Width = img.Width.ToString();
                }
            }
            else if (Stream != null)
            {
                Image image = Image.FromStream(new MemoryStream(Utility.ReadFully(Stream)));
                MediaInformation.Height = image.Height.ToString();
                MediaInformation.Width = image.Width.ToString();
            }
        }

        protected override void CreateDirectory()
        {
            if (Directories == null)
                return;

            if (exifDir == null)
                exifDir = Directories.OfType<ExifDirectoryBase>().FirstOrDefault();

            if(exifSubIfdDir == null)
                exifSubIfdDir = Directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        }

        protected override void ExtractTag(MetadataExtractor.Tag tag)
        {
            try
            {
                if (exifDir != null && tag.DirectoryName.Equals(exifDir.Name))
                {
                    if (tag.Type.Equals(ExifDirectoryBase.TagWinTitle))
                        MediaInformation.Title = tag.Description;
                    if (tag.Type.Equals(ExifDirectoryBase.TagImageDescription))
                        MediaInformation.Description = tag.Description;
                    if (tag.Type.Equals(ExifDirectoryBase.TagDateTime))  //'2008:11:01 21:15:07
                    {
                        //                    MediaInformation.CreatedDate = Utility.ParseDate(tag.Description, "yyyy:MM:dd HH:mm:ss");
                        MediaInformation.CreatedDate = tag.Description;
                    }
                    if (tag.Type.Equals(ExifDirectoryBase.TagCopyright))
                        MediaInformation.CopyRight = tag.Description;
                }

                if (exifSubIfdDir != null && tag.DirectoryName.Equals(exifSubIfdDir.Name))
                {
                    if (tag.Type.Equals(ExifDirectoryBase.TagExifImageHeight))
                        MediaInformation.Height = tag.Description;
                    if (tag.Type.Equals(ExifDirectoryBase.TagExifImageWidth))
                        MediaInformation.Width = tag.Description;
                }
            }
            catch (Exception ex)
            {

            }
            
        }

    }
}
