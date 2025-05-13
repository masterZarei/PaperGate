using PaperGate.Shared.ReturnTypes;
using System.Globalization;

namespace PaperGate.Core.Libraries.Convertors;
public static class DateConvertor
{
    public static string ToShamsi(this DateTime value, bool FullData = false)
    {
        try
        {
            PersianCalendar pc = new();
            if (FullData)
            {

                if (pc.GetHour(value) > -1)
                {
                    return pc.GetHour(value).ToString("00") + ":" +
                    pc.GetMinute(value).ToString("00") + ":" +
                    pc.GetSecond(value).ToString("00") + " | " +
                    pc.GetYear(value) + "/" +
                    pc.GetMonth(value).ToString("00") + "/" +
                    pc.GetDayOfMonth(value).ToString("00");
                }
                else
                {
                    return pc.GetYear(value) + "/" +
                       pc.GetMonth(value).ToString("00") + "/" +
                       pc.GetDayOfMonth(value).ToString("00");

                }
            }
            return pc.GetYear(value) + "/" +
                  pc.GetMonth(value).ToString("00") + "/" +
                  pc.GetDayOfMonth(value).ToString("00");
        }

        catch
        {
            return "فرمت تاریخ اشتباه است";
        }
    }

    public static string ToShortShamsiTime(this DateTime value, bool FullData = false)
    {
        try
        {
            PersianCalendar pc = new();

            return pc.GetHour(value).ToString("00") + ":" +
                pc.GetMinute(value).ToString("00") + (pc.GetHour(value) >= 12 ? " ب.ظ" : " ق.ظ");
        }

        catch
        {
            return "فرمت تاریخ اشتباه است";
        }
    }

    public static DateTime ToMiladi(CDateTime value)
    {
        PersianCalendar pc = new();
        DateTime result = new(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, pc);
        return result;
    }
}
