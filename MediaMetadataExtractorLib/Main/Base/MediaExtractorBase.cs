using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.FileType;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MediaMetadataExtractorLib.Main.Base
{
  

    public abstract class MediaExtractorBase : IMediaExtractor
    {


        public MediaInformation MediaInformation { get; set; }

        protected IEnumerable<MetadataExtractor.Directory> Directories { get; set; }

        protected string _path { get; set; }
        public Stream Stream { get; set; }
        public int MediaType { get; set; }
   
        public MediaExtractorBase(int type, string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("path can't be null");

            _path = path;
            MediaType = type;
            MediaInformation = new MediaInformation();

            try
            {
                //DOC = 0, AUDIO = 2, IMAGE = 1, VIDEO = 3,
                switch (MediaType)
                {
                    case 2:
                        Extract2();
                        break;
                    case 1:
                        Extract();
                        break;
                    case 3:
                        Extract();
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public MediaExtractorBase(int type, Stream stream)
        {
            if (stream == null)
                throw new Exception("stream can't be null");

            this.Stream = stream;
            MediaType = type;
            MediaInformation = new MediaInformation();

            try
            {
                switch (MediaType)
                {
                    case 2:
                        Extract2();
                        break;
                    case 1:
                        Extract();
                        break;
                    case 3:
                        Extract();
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            { 
                
            }

        }

        //for sub class to extract additional tags
        protected abstract void ExtractTag(MetadataExtractor.Tag tag);

        protected abstract void CreateDirectory();

        protected virtual void Extract2()
        {
           
        }

        protected void Extract()
        {
            if (Stream != null)
                Directories = ImageMetadataReader.ReadMetadata(Stream);
            else if (!string.IsNullOrEmpty(_path))
                Directories = ImageMetadataReader.ReadMetadata(_path);

            if (Directories == null)
                return;

            var gpsDir = Directories.OfType<GpsDirectory>().FirstOrDefault();
            if (gpsDir != null)
            {
                MediaInformation.GeoLocation = gpsDir.GetGeoLocation();
                MediaInformation.Latitude = gpsDir.GetGeoLocation().Latitude.ToString("0.00000");
                MediaInformation.Longitude = gpsDir.GetGeoLocation().Longitude.ToString("0.00000");
            }
            

            var fileTypeDir = Directories.OfType<FileTypeDirectory>().FirstOrDefault();
            var fileMeatadataDir = Directories.OfType<FileMetadataDirectory>().FirstOrDefault();

            if (Stream != null)
            {
                MediaInformation.FileSize = this.Stream.Length.ToString();
            }


            CreateDirectory();

            foreach (var directory in Directories)
            {
                if (directory.Tags != null)
                {
                    foreach (var tag in directory.Tags)
                    {

                        if (fileTypeDir != null && tag.DirectoryName.Equals(fileTypeDir.Name))
                        {
                            if (tag.Type.Equals(FileTypeDirectory.TagDetectedFileTypeLongName))
                                MediaInformation.Detected_File_Type_Long_Name = tag.Description;

                            if (tag.Type.Equals(FileTypeDirectory.TagDetectedFileMimeType))
                            {
                                MediaInformation.MIMEType = tag.Description;
                                MediaInformation.Detected_MIME_Type = tag.Description;
                            }
                            
                            if (tag.Type.Equals(FileTypeDirectory.TagDetectedFileTypeName))
                            {
                                MediaInformation.Detected_File_Type_Name = tag.Description;
                                MediaInformation.FileType = tag.Description;
                            }

                            if (tag.Type.Equals(FileTypeDirectory.TagExpectedFileNameExtension))
                            {
                                MediaInformation.FileNameExtension = tag.Description;
                                MediaInformation.Expected_File_Name = tag.Description;
                            }
                            
                        }
                        if (fileMeatadataDir != null && tag.DirectoryName.Equals(fileMeatadataDir.Name))
                        {
                            if (tag.Type.Equals(FileMetadataDirectory.TagFileName))
                                MediaInformation.FileName = tag.Description;
                            if (tag.Type.Equals(FileMetadataDirectory.TagFileSize))
                                MediaInformation.FileSize = tag.Description;
                            if (tag.Type.Equals(FileMetadataDirectory.TagFileModifiedDate))
                                MediaInformation.ModifiedDate = tag.Description;
                            //MediaInformation.ModifiedDate = Utility.ParseDate(tag.Description, "ddd MMM dd HH:mm:ss zz:00 yyyy");
                            
                        }

                        //for sub class extract
                        ExtractTag(tag);
                    }
                }
            }
        }
    }
}
