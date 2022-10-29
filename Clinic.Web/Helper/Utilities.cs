using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using Clinic.Core.Helper.ModelsHelper;
using Clinic.Core.Constant;

namespace Clinic.Web.Helper
{
    public static class Utilities
    {
        public static string GetDate(DateTime date)
        {
            TimeSpan span = (DateTime.Now - date);
            var days = span.Days;
            var hours = span.Hours;
            var minutes = span.Minutes;
            var seconds = span.Seconds;
            if (minutes == 0 && days == 0 && hours == 0)
            {
                return String.Format("{0} seconds", seconds);
            }
            if (days == 0 && hours == 0)
            {
                return String.Format("{0} minutes", minutes);
            }
            if (days == 0 && hours != 0)
            {
                return String.Format("{0} hours, {1} minutes", hours, minutes);
            }
            if (days != 0)
            {
                return String.Format("{0} days, {1} hours", days, hours);
            }
            return String.Format("{0} days, {1} hours, {2} minutes", days, hours, minutes);
        }

        public static string AutoIncrement(string lastSerialNum)
        {
            int id = Convert.ToInt32(lastSerialNum);
            id = id + 1;
            string autoId = String.Format("{0:00000000}", id);
            return autoId;
        }

        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;
            return attribute?.Description ?? e.ToString();
        }

        public static double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double radiusE = 6378135;
            double radiusP = 6356750;

            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            distance = radius * c;
            return distance;
        }

        public static double ToRadians(double degrees) => degrees * Math.PI / 180.0;

        public static double DistanceInMiles(double lon1d, double lat1d, double lon2d, double lat2d)
        {
            var lon1 = ToRadians(lon1d);
            var lat1 = ToRadians(lat1d);
            var lon2 = ToRadians(lon2d);
            var lat2 = ToRadians(lat2d);

            var deltaLon = lon2 - lon1;
            var c = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
            var earthRadius = 3958.76;
            var distInMiles = earthRadius * c;

            return distInMiles;
        }


        public static List<DataPage> Pagenate(int totalItems, out string showing, out bool nextEnabled, out bool prevEnabled, out int totalPages, int currentPage = 1, int pageSize = 10, int maxPages = 5)
        {
            var pager = new Pager(totalItems, currentPage, pageSize, maxPages);
            totalPages = pager.TotalPages;
            var pages = new List<DataPage>();
            foreach (var item in pager.Pages)
            {
                DataPage page = new DataPage
                {
                    PageNumber = item,
                    IsSelected = item == currentPage ? true : false,
                };
                pages.Add(page);
            }

            var start = (pageSize * (currentPage - 1)) + 1;
            if (totalPages == 1)
            {
                showing = $"Showing {start} to {totalItems} of {totalItems}) entries";
            }
            else
            {
                var to = totalItems == 0 ? 0 : currentPage == totalPages ? (totalItems % (totalPages - 1) + start - 1) : start + pageSize - 1;
                showing = $"Showing {start} to {to} of {totalItems}) entries";
            }
            if (currentPage == 1)
                prevEnabled = false;
            else
                prevEnabled = true;

            if (currentPage >= totalPages)
                nextEnabled = false;
            else
                nextEnabled = true;


            return pages;
        }

        //public static string GenerateQR(string data)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    var stream = new MemoryStream();
        //    qrCode.GetGraphic(20).Save(stream, ImageFormat.Png);
        //    var bytes = stream.ToArray();
        //    return Convert.ToBase64String(bytes);
        //}

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path.Substring(0, 8);
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static Dictionary<int, string> GetEnumAsDictionary<T>()
        {
            var resultDictionary = new Dictionary<int, string>();
            foreach (var name in Enum.GetNames(typeof(T)))
            {
                resultDictionary.Add((int)Enum.Parse(typeof(T), name), name);
            }
            return resultDictionary;
        }

        public static List<string> GetEnumAsList<T>()
        {
            List<string> listTypes = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Where(fi => fi.FieldType == typeof(string))
                .Select(fi => fi.Name)
                .ToList();
            return listTypes;
        }

        public static List<T> GetEnumAsListValues<T>()
        {
            List<T> listTypes = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return listTypes;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string ByteToString(this byte[] arr)
        {
            string fileString = Convert.ToBase64String(arr);
            return fileString;
        }

        public static string ToSize(this int value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.00");
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool IsNull(this string value) => string.IsNullOrEmpty(value);

        public static Type GetBaseType(this PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }
            return propertyType;
        }

        public static bool CheckIDNum(string ID)
        {
            if (ID.IsNull())
                return false;
            if (!ID.All(char.IsDigit))
                return false;
            if (ID.Length != 9)
                return false;
            int isValid = 0;
            double sum = 0;
            for (int i = 0; i < ID.Length - 1; i++)
            {
                int x = Convert.ToInt32(ID.Substring(i, 1));
                if ((i % 2) != 0)
                {
                    x = x * 2;
                    if (x > 9)
                    {
                        x = x - 9;
                    }
                }
                sum = sum + x;
            }
            sum = sum / 10.0;
            int checkNum = Convert.ToInt32(ID.Substring(8));
            int checkNo = 3;
            if (((int)sum) == sum)
            {
                checkNo = 0;
            }
            else
            {
                double sum1 = (((int)sum + 1) - sum) * 10;
                checkNo = (int)Math.Round(sum1);
            }
            if (checkNo != checkNum)
            {
                isValid = 1;
            }
            return isValid == 0 ? true : false;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

    }

}
