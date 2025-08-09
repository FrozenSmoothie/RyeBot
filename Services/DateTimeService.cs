using System;
using NodaTime;
using NodaTime.Extensions;

namespace RyeBot.Services
{
    public static class DateTimeService
    {
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static LocalDateTime GetCurrentLocalDateTime()
        {
            return SystemClock.Instance.InZone(
                DateTimeZoneProviders.Tzdb["Europe/London"]).GetCurrentLocalDateTime();

        }

        public static LocalDateTime GetLocalDateTimeFromDateTimeOffset(
            DateTimeOffset dateTimeOffset)
        {
            return LocalDateTime.FromDateTime(dateTimeOffset.DateTime);
        }

        public static string GetFormattedYearMonthDayDurationAsString(
            Period period)
        {
            var formattedDurationString = "";

            if (period.Days > 0)
            {
                formattedDurationString += $"{period.Days} day";

                if (period.Days > 1)
                {
                    formattedDurationString += "s";
                }
            }

            if (period.Months > 0)
            {
                var monthsDuration = $"{period.Months} month";

                if (period.Months > 1)
                {
                    monthsDuration += "s";
                }

                formattedDurationString = $"{monthsDuration}, {formattedDurationString}";
            }

            if (period.Years > 0)
            {
                var yearsDuration = $"{period.Years} year";

                if (period.Years > 1)
                {
                    yearsDuration += "s";
                }

                formattedDurationString = $"{yearsDuration}, {formattedDurationString}";
            }
            
            return formattedDurationString;
        }

        public static string GetFormattedSecondMinuteHourDayDurationAsString(
            Period period)
        {
            var formattedDurationString = "";

            if (period.Seconds > 0)
            {
                formattedDurationString += $"{period.Seconds} second";

                if (period.Seconds > 1)
                {
                    formattedDurationString += "s";
                }
            }

            if (period.Minutes > 0)
            {
                var minutesDuration = $"{period.Minutes} minute";

                if (period.Minutes > 1)
                {
                    minutesDuration += "s";
                }

                formattedDurationString = $"{minutesDuration}, {formattedDurationString}";
            }

            if (period.Hours > 0)
            {
                var hoursDuration = $"{period.Hours} hour";

                if (period.Hours > 1)
                {
                    hoursDuration += "s";
                }

                formattedDurationString = $"{hoursDuration}, {formattedDurationString}";
            }

            if (period.Days > 0)
            {
                var daysDuration = $"{period.Days} day";

                if (period.Days > 1)
                {
                    daysDuration += "s";
                }

                formattedDurationString = $"{daysDuration}, {formattedDurationString}";
            }

            return formattedDurationString;
        }

        public static string GetFormattedDayHourMinuteSecondDurationAsString(TimeSpan timeSpan)
        {
            var value = "";

            if (timeSpan.Days != 0)
            {
                var metric = "day";
                if (timeSpan.Days > 1)
                {
                    metric += "s";
                }

                value += $"{timeSpan.Days} {metric} ";
            }

            if (timeSpan.Hours != 0)
            {
                var metric = "hour";
                if (timeSpan.Hours > 1)
                {
                    metric += "s";
                }

                value += $"{timeSpan.Hours} {metric} ";
            }

            if (timeSpan.Minutes != 0)
            {
                var metric = "minute";
                if (timeSpan.Minutes > 1)
                {
                    metric += "s";
                }

                value += $"{timeSpan.Minutes} {metric} ";
            }

            if (timeSpan.Seconds != 0)
            {
                var metric = "second";
                if (timeSpan.Seconds > 1)
                {
                    metric += "s";
                }

                value += $"{timeSpan.Seconds} {metric} ";
            }

            return value;
        }
    }
}
