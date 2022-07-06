﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Clinic.Core.Helper
{
    public static class Extensions
    {
        private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = 
            new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T FromJson<T>(this string json) =>
            System.Text.Json.JsonSerializer.Deserialize<T>(json, _jsonOptions);

        public static string ToJson(this object obj) =>
            System.Text.Json.JsonSerializer.Serialize(obj, _jsonOptions);

        public static IEnumerable<T1> Map<T1, T2>(this IEnumerable<T2> t2s) where T1 : new()
        {
            if (t2s == null)
                yield break;

            var t1Type = typeof(T1);
            var t2Type = typeof(T2);
            var t1Props = t1Type.GetProperties();
            var t2Props = t2Type.GetProperties();
            foreach (var t2 in t2s)
            {
                var t1 = new T1();
                foreach (var tProp in t1Props)
                {
                    var objProp = t2Props.FirstOrDefault(x => x.Name == tProp.Name && x.PropertyType.FullName == tProp.PropertyType.FullName);
                    if (objProp == null)
                        continue;

                    var value = objProp.GetValue(t2);
                    tProp.SetValue(t1, value);
                }
                yield return t1;
            }
        }

        public static bool IsBetween(this DateTimeOffset date, DateTimeOffset start, DateTimeOffset end)
            => date > start && date < end;

        public static bool IsBetweenIncludEdges(this DateTimeOffset date, DateTimeOffset start, DateTimeOffset end)
            => date >= start && date <= end;

        public static DateTimeOffset ParseDate(this string date, string format = "yyyy-MM-dd HH:mm:ss")
            => DateTimeOffset.ParseExact(date, format, CultureInfo.CurrentCulture);

        public static long GetDay(this DateTimeOffset date)
        {
            return (long)Math.Floor(TimeSpan.FromTicks(date.Ticks).TotalDays);
        }

        public static T1 As<T1, T2>(this T2 t2, Func<T2, T1> func)
            => func(t2);

        public static string UrlEncode(this string value)
            => HttpUtility.UrlEncode(value);

        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);

        public static bool IsNotNullOrEmpty(this string str)
            => !string.IsNullOrEmpty(str);

        public static DateTimeOffset RemoveTimeData(this DateTimeOffset date)
        {
            date = date.AddHours(-date.Hour);
            date = date.AddMinutes(-date.Minute);
            date = date.AddSeconds(-date.Second);
            date = date.AddMilliseconds(-date.Millisecond);
            return date;
        }

        public static int CountInSeries<T>(this IEnumerable<T> ts, Func<T, bool> func)
        {
            var count = 0;
            var started = false;
            foreach (var item in ts)
            {
                if (func(item))
                {
                    count++;
                    started = true;
                }
                else if (started)
                    break;
            }
            return count;
        }

        public enum DateCompareResult { Bigger, Smaller, DatesAreEqual }
        public static DateCompareResult CompareDate(this DateTimeOffset first, DateTimeOffset second)
        {
            if (first > second)
                return DateCompareResult.Bigger;
            if (first < second)
                return DateCompareResult.Smaller;
            return DateCompareResult.DatesAreEqual;
        }

        public static T[] ToArrayOrEmpty<T>(this IEnumerable<T> ts)
            => ts?.ToArray() ?? new T[0];


        public static void Foreach<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (var t in ts)
                action(t);
        }

        //public static string GenerateQR<T>(this T t) => Utilities.GenerateQR(t.Serialize());

        public static void Log(this Exception ex, bool type = true)
        {
            String LogText = "";
            LogText += $"Exception: {ex.GetType().ToString()}\n";
            LogText += $"Message: {ex.Message}\n---\n";
            LogText += $"Exception String: {ex.ToString()}\n";
            LogText.Log(type);
        }

        static ReaderWriterLock logLock = new ReaderWriterLock();
        static ReaderWriterLock uLogLock = new ReaderWriterLock();
        public static void Log(this String err, bool type = true)
        {
            String LogText = "______________________________________________________________\n\n";
            LogText += $"{DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt")} \n";
            LogText += $"{err} \n";

            if (type)
            {
                try
                {
                    logLock.AcquireWriterLock(int.MaxValue);
                    System.IO.File.AppendAllText("Log.log", LogText);
                }
                finally
                {
                    logLock.ReleaseWriterLock();
                }
            }
            else
            {
                try
                {
                    uLogLock.AcquireWriterLock(int.MaxValue);
                    System.IO.File.AppendAllText("UnLog.log", LogText);
                }
                finally
                {
                    uLogLock.ReleaseWriterLock();
                }
            }
        }
    }
}
