using MediaMetadataExtractorLib.Main.Base;


using System;

using System.IO;
using System.Linq;

namespace MediaMetadataExtractorLib.Main
{
    public class FileAbstraction : TagLib.File.IFileAbstraction
    {
        public FileAbstraction(string file)
        {
            Name = file;
        }

        public string Name { get; }

        public Stream ReadStream => new FileStream(Name, FileMode.Open);

        public Stream WriteStream => new FileStream(Name, FileMode.Open);

        public void CloseStream(Stream stream)
        {
            stream.Close();
        }
    }
    public class AudioExtractor : MediaExtractorBase
    {
        public TagLib.File TagLibfile 
        {
            get => _file;
            set => _file = value;
        }
        public TagLib.File _file { get; set; } 

        public AudioExtractor(int type, string path) : base(type, path)
        {
           
        }

        public AudioExtractor(int type, Stream stream) : base(type, stream)
        {
            
        }

        protected override void Extract2()
        {
            if (string.IsNullOrEmpty(_path))
                return;
            try
            {
                TagLib.File tfile = TagLib.File.Create(_path);
                TagLibfile = tfile;

                if (tfile != null)
                {
                    MediaInformation.Title = tfile.Tag.Title;
                    MediaInformation.Duration = tfile.Properties.Duration.ToString();
                    MediaInformation.Description = tfile.Properties.Description;
                    MediaInformation.Album = tfile.Tag.Album;
                    MediaInformation.Author = tfile.Tag.AlbumArtists.FirstOrDefault();
                    MediaInformation.Publisher = tfile.Tag.Publisher;
                    MediaInformation.CopyRight = tfile.Tag.Copyright;
                    MediaInformation.MIMEType = MimeMapping.MimeUtility.GetMimeMapping(_path);
                    MediaInformation.FileName = tfile.Name;
                    if (tfile.Tag.DateTagged != null)
                        MediaInformation.CreatedDate = ((DateTime)tfile.Tag.DateTagged).ToString();
                    //MediaInformation.CreatedDate = (DateTime)tfile.Tag.DateTagged;
                }
            }
            catch (Exception ex)
            {

            }

          
        }

        protected override void CreateDirectory()
        {
            //keep empty , cause use taglib
        }

        protected override void ExtractTag(MetadataExtractor.Tag tag)
        {
            //keep empty, cause use taglib
        }
    }
}
