

using MetadataExtractor;

namespace MediaMetadataExtractorLib
{
    public class MediaInformation
    {
        public string Detected_File_Type_Name { get; set; }
        public string Detected_File_Type_Long_Name { get; set; }
        public string Detected_MIME_Type { get; set; }
        public string Expected_File_Name { get; set; }

        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public string FileNameExtension { get; set; }
        public string MIMEType { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Duration { get; set; }
        public string CopyRight { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }

        public string Album { get; set; }
    }
}
