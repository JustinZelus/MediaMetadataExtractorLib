using MediaMetadataExtractorLib.Main.Base;

using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;


using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using TagLib;

namespace MediaMetadataExtractorLib.Main
{
    public class VideoExtractor : MediaExtractorBase
    {
        private QuickTimeMovieHeaderDirectory quickTimeMovieHeaderDir;
        private QuickTimeTrackHeaderDirectory quickTimeTrackHeaderDir;
        private QuickTimeMetadataHeaderDirectory quickTimeMetadataHeaderDir;
        public VideoExtractor(int type, string path) : base(type, path)
        {

        }
        public VideoExtractor(int type, Stream stream) : base(type, stream)
        {

        }

        protected override void CreateDirectory()
        {
            if (Directories == null)
                return;
            if (quickTimeMovieHeaderDir == null)
                quickTimeMovieHeaderDir = Directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
            if (quickTimeTrackHeaderDir == null)
                quickTimeTrackHeaderDir = Directories.OfType<QuickTimeTrackHeaderDirectory>().FirstOrDefault();
            if (quickTimeMetadataHeaderDir == null)
                quickTimeMetadataHeaderDir = Directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
        }

        protected override void ExtractTag(MetadataExtractor.Tag tag)
        {
            try
            {
                if (quickTimeMovieHeaderDir != null && tag.DirectoryName.Equals(quickTimeMovieHeaderDir.Name))
                {
                    if (tag.Type.Equals(QuickTimeMovieHeaderDirectory.TagDuration))
                        MediaInformation.Duration = tag.Description;
                }

                if (quickTimeTrackHeaderDir != null && tag.DirectoryName.Equals(quickTimeTrackHeaderDir.Name))
                {
                    if (tag.Type.Equals(QuickTimeTrackHeaderDirectory.TagCreated))
                        MediaInformation.CreatedDate = tag.Description;
                    //                    MediaInformation.CreatedDate = Utility.ParseDate(tag.Description, "ddd MMM dd HH:mm:ss yyyy");


                    if (tag.Type.Equals(QuickTimeTrackHeaderDirectory.TagHeight))
                    {

                        if (int.TryParse(tag.Description, out int H) && H > 0)
                            MediaInformation.Height = tag.Description;
                    }

                    if (tag.Type.Equals(QuickTimeTrackHeaderDirectory.TagWidth))
                    {

                        if (int.TryParse(tag.Description, out int W) && W > 0)
                            MediaInformation.Width = tag.Description;
                    }
                }

                if (quickTimeMetadataHeaderDir != null && tag.DirectoryName.Equals(quickTimeMetadataHeaderDir.Name))
                {
                    if (tag.Type.Equals(QuickTimeMetadataHeaderDirectory.TagCopyright))
                        MediaInformation.CopyRight = tag.Description;
                }
            }
            catch (Exception ex)
            {

            }

            
        }

    }
}
