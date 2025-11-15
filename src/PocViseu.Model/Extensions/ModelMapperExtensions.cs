using System.Globalization;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.ModelView;

namespace PocViseu.Model.Extensions
{
    public static class ModelMapperExtensions
    {
        private static CultureInfo Culture { get; set; } = CultureInfo.GetCultureInfo("en-GB");
        public static DateTime ParseDate(this string strDate, string strTime)
        {
            try
            {
                //strDate = strDate.Replace(".", "/");
                var date = DateTime.ParseExact($"{strDate} {strTime}:00", "dd/MM/yyyy HH:mm:ss", Culture);
                return date;
            }
            catch (Exception e)
            {
                return DateTime.Parse($"{strDate}");
            }
        }

        public static DateTime? ParseDate2(this string strDate)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                return null;
            }
            try
            {
                //strDate = strDate.Replace(".", "/");
                return DateTime.ParseExact($"{strDate} 00:00:00", "dd/MM/yyyy HH:mm:ss", Culture);
            }
            catch (Exception e)
            {
                return DateTime.Parse($"{strDate}");
            }
        }

        public static DateTime? ParseDate2End(this string strDate)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                return null;
            }
            try
            {
                //strDate = strDate.Replace(".", "/");
                return DateTime.ParseExact($"{strDate} 23:59:59", "dd/MM/yyyy HH:mm:ss", Culture);
            }
            catch (Exception e)
            {
                return DateTime.Parse($"{strDate}");
            }
        }


        public static LogSystem ToLog(this string value, string description, LogLevelStatus level, long userId, string trackKey = "")
        {
            return new LogSystem()
            {
                Value = value,
                Description = description,
                Level = (int)level,
                CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ),
                TraceKey = trackKey,
                UserId = userId,
            };
        }

        public static WebcorpConfig ToDataModel(this WebcorpConfigModelView view, string userId)
        {
            return new WebcorpConfig()
            {
                ParamKey = view.ParamKey,
                ParamValue = view.ParamValue,
                ParamDesc = view.ParamDesc,
                CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ),
                UserId = long.Parse(userId),
            };
        }

        public static Process ToDataModel(this ProcessModelView view, string userId)
        {
            return new Process()
            {
                HashId = view.HashId,
                //ProjectId = view.ProjectId,
                SchedulingExecType = view.SchedulingExecTypeId,
                //StartedIn = string.IsNullOrEmpty(view.StartedIn) ? null : view.StartedIn.ParseDate(view.StartedInTime),
                StartedIn = DateTime.Now.AddHours(SysConfig.TMZ),
                Status = view.Status,
                MenssageResponse = view.MenssageResponse,
                CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ),
                UserId = long.Parse(userId),
            };
        }


    }
}
