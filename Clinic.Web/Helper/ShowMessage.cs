using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Helper
{
    public static class ShowMessage
    {
        public static string NotExistResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: العنصر غير موجود" : text,
                close = 1
            });
        }

        public static string OperationSuccessResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: نجحت العملية" : text,
                close = 1
            });
        }

        public static string AddSuccessResult(string text = null, int? status = null, int? close = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = !status.HasValue ? 1 : status.Value,
                msg = String.IsNullOrEmpty(text) ? "s: تمت الاضافة" : text,
                close = !close.HasValue ? 1 : close.Value,
            });
        }

        public static string EditSuccessResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: تم التعديل" : text,
                close = 1
            });
        }

        public static string DeleteSuccessResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: تم الحذف" : text,
                close = 1
            });
        }

        public static string FailedResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "e: فشلت العملية" : text,
                close = 2
            });
        }

        public static string DuplicationResult(string text = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "w: العنصر موجود بالفعل" : text,
                close = 2
            });
        }

        public static string TempDataObj(string text = null, string status = null, string id = null)
        {
            return JsonConvert.SerializeObject(new MessegeResult
            {
                status = String.IsNullOrEmpty(status) ? "s" : status,
                msg = String.IsNullOrEmpty(text) ? "نجحت العملية" : text,
                id = String.IsNullOrEmpty(id) ? null : id,
            });
        }

    }

    public class MessegeResult
    {
        public string msg { get; set; }
        public string status { get; set; }
        public string id { get; set; }
    }
}
