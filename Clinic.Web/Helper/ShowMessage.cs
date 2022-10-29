using Newtonsoft.Json;

namespace Clinic.Web.Helper
{
    public static class ResultsMessage
    {
        public static object NotExistResult(string text = null) =>
            new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: العنصر غير موجود" : text,
                close = 1
            };

        public static object OperationSuccessResult(string text = null) =>
            new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: نجحت العملية" : text,
                close = 1
            };

        public static object AddSuccessResult(string text = null, int? status = null, int? close = null) =>
            new
            {
                status = !status.HasValue ? 1 : status.Value,
                msg = String.IsNullOrEmpty(text) ? "s: تمت الاضافة" : text,
                close = !close.HasValue ? 1 : close.Value,
            };

        public static object EditSuccessResult(string text = null) =>
            new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: تم التعديل" : text,
                close = 1
            };


        public static object DeleteSuccessResult(string text = null) =>
            new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "s: تم الحذف" : text,
                close = 1
            };

        public static object FailedResult(string text = null) =>
            new
            {
                status = 1,
                msg = String.IsNullOrEmpty(text) ? "e: فشلت العملية" : text,
                close = 1
            };

        public static object DuplicationResult(string text = null) =>
             new
             {
                 status = 1,
                 msg = String.IsNullOrEmpty(text) ? "w: العنصر موجود بالفعل" : text,
                 close = 1
             };

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
