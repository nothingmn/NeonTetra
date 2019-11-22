using System;

namespace NeonTetra.Contracts.Services
{
    public interface ICronExpressions
    {
        string Minutely();

        string Hourly();

        string Hourly(int minute);

        string Daily();

        string Daily(int hour);

        string Daily(int hour, int minute);

        string Weekly();

        string Weekly(DayOfWeek dayOfWeek);

        string Weekly(DayOfWeek dayOfWeek, int hour);

        string Weekly(DayOfWeek dayOfWeek, int hour, int minute);

        string Monthly();

        string Monthly(int day);

        string Monthly(int day, int hour);

        string Monthly(int day, int hour, int minute);

        string Yearly();

        string Yearly(int month);

        string Yearly(int month, int day);

        string Yearly(int month, int day, int hour);

        string Yearly(int month, int day, int hour, int minute);

        string Never();
    }
}