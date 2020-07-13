
using MediaMetadataExtractorLib.Main;


namespace MediaMetadataExtractorLib
{
    public class MMetadataResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IMediaExtractor MediaExtractor { get; set; }
}
}
