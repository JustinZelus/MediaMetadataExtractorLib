
using MediaMetadataExtractorLib.Main;
using MediaMetadataExtractorLib.Util;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MediaMetadataExtractorLib
{


    public class MetadataExtractorFactory
    {

        public MetadataExtractorFactory()
        {
        }

        public MMetadataResponse CreateExtractor(int mediaType, Stream stream)
        {
            var result = new MMetadataResponse();
            if (stream == null)
            {
                result.IsSuccess = false;
                result.Message = "Stream不得為空";
                return result;
            }

            if (mediaType < 0)
            {
                result.IsSuccess = false;
                result.Message = "MediaSourceType錯誤";
                return result;
            }


            IMediaExtractor mediaExtractor = null;
            try
            {
                switch (mediaType)
                {
                    case 0:
                        break;
                    case 1:
                        mediaExtractor = new ImageExtractor(mediaType, stream);
                        break;

                    case 2:
                        mediaExtractor = new AudioExtractor(mediaType, stream);
                        break;
                    case 3:
                        mediaExtractor = new VideoExtractor(mediaType, stream);
                        break;
                    default:
                        return null;
                }



                if (mediaExtractor == null || mediaExtractor.MediaInformation == null)
                {
                    result.IsSuccess = false;
                    result.Message = "IMediaExtractor不得為空或解析失敗";
                    return result;
                }


                var dicMediaInformation = Utility.ParseObj(mediaExtractor.MediaInformation);
                result.MediaExtractor = mediaExtractor;
                result.DicMediaInformation = dicMediaInformation;
                result.IsSuccess = true;
                result.Message = "MediaMetadataExtractor套件解析成功";

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "MediaMetadataExtractor套件解析錯誤" + "\r\n" + ex.Message;
            }

            return result;
        }
        public MMetadataResponse CreateExtractor(int mediaType, string path)
        {
            var result = new MMetadataResponse();

            if (string.IsNullOrEmpty(path))
            {
                result.IsSuccess = false;
                result.Message = "檔案路徑不得為空";
                return result;
            }

            if (mediaType < 0)
            {
                result.IsSuccess = false;
                result.Message = "MediaSourceType錯誤";
                return result;
            }

            IMediaExtractor mediaExtractor = null;
            try
            {
                switch (mediaType)
                {
                    case 0:
                        break;
                    case 1:
                        mediaExtractor = new ImageExtractor(mediaType, path);
                        break;

                    case 2:
                        mediaExtractor = new AudioExtractor(mediaType, path);
                        break;
                    case 3:
                        mediaExtractor = new VideoExtractor(mediaType, path);
                        break;
                    default:
                        return null;
                }

                if (mediaExtractor == null || mediaExtractor.MediaInformation == null)
                {
                    result.IsSuccess = false;
                    result.Message = "IMediaExtractor不得為空或解析失敗";
                    return result;
                }

                var dicMediaInformation = Utility.ParseObj(mediaExtractor.MediaInformation);
                result.MediaExtractor = mediaExtractor;
                result.DicMediaInformation = dicMediaInformation;
                result.IsSuccess = true;
                result.Message = "MediaMetadataExtractor套件解析成功";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "MediaMetadataExtractor套件解析錯誤" + "\r\n" + ex.Message;
            }

            return result;
        }
    }
}
