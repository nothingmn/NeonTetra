using System;
using Hangfire;
using NeonTetra.Contracts.Services;

namespace NeonTetraWebApi.Scheduling
{
    public class HangFireCronProvider : ICronExpressions
    {
        public string Minutely()
        {
            return Cron.Minutely();
        }

        public string Hourly()
        {
            return Cron.Hourly();
        }

        public string Hourly(int minute)
        {
            return Cron.Hourly(minute);
        }

        public string Daily()
        {
            return Cron.Daily();
        }

        public string Daily(int hour)
        {
            return Cron.Daily(hour);
        }

        public string Daily(int hour, int minute)
        {
            return Cron.Daily(hour, minute);
        }

        public string Weekly()
        {
            return Cron.Weekly();
        }

        public string Weekly(DayOfWeek dayOfWeek)
        {
            return Cron.Weekly(dayOfWeek);
        }

        public string Weekly(DayOfWeek dayOfWeek, int hour)
        {
            return Cron.Weekly(dayOfWeek, hour);
        }

        public string Weekly(DayOfWeek dayOfWeek, int hour, int minute)
        {
            return Cron.Weekly(dayOfWeek, hour, minute);
        }

        public string Monthly()
        {
            return Cron.Monthly();
        }

        public string Monthly(int day)
        {
            return Cron.Monthly(day);
        }

        public string Monthly(int day, int hour)
        {
            return Cron.Monthly(day, hour);
        }

        public string Monthly(int day, int hour, int minute)
        {
            return Cron.Monthly(day, hour, minute);
        }

        public string Yearly()
        {
            return Cron.Yearly();
        }

        public string Yearly(int month)
        {
            return Cron.Yearly(month);
        }

        public string Yearly(int month, int day)
        {
            return Cron.Yearly(month, day);
        }

        public string Yearly(int month, int day, int hour)
        {
            return Cron.Yearly(month, day, hour);
        }

        public string Yearly(int month, int day, int hour, int minute)
        {
            return Cron.Yearly(month, day, hour, minute);
        }

        public string Never()
        {
            return Cron.Never();
        }
    }
}