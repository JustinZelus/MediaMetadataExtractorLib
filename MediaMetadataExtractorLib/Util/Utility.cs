﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaMetadataExtractorLib.Util
{
    public static class Utility
    {
        public static Dictionary<string, object> ParseObj(object obj)
        {
            if (obj == null) return null;

            var result = obj.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));
            return result;
        }
        public static DateTime ParseDate(string s, string format)
        {
            if (DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime CreatedDate))
            {
                return CreatedDate;
            }
            return DateTime.Now;
        }

        public static byte[] ReadFully(Stream s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.CanRead)
                throw new ArgumentException("Stream cannot be read");

            MemoryStream ms = s as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            long pos = s.CanSeek ? s.Position : 0L;
            if (pos != 0L)
                s.Seek(0, SeekOrigin.Begin);

            byte[] result = new byte[s.Length];
            s.Read(result, 0, result.Length);
            if (s.CanSeek)
                s.Seek(pos, SeekOrigin.Begin);
            return result;
        }


    }
}
