
using System.Collections.Generic;
using System.IO;

namespace MediaMetadataExtractorLib.Main
{
    public interface IMediaExtractor
    {
         Stream Stream { get; set; }

         MediaInformation MediaInformation { get; set; }

         int MediaType { get; set; }

         
    }

}
