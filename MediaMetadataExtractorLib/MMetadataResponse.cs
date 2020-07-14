
using MediaMetadataExtractorLib.Main;

using System.Collections.Generic;

namespace MediaMetadataExtractorLib
{
    public class MMetadataResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IMediaExtractor MediaExtractor { get; set; }
        public Dictionary<string, object> DicMediaInformation { get; set; }
    }
}
