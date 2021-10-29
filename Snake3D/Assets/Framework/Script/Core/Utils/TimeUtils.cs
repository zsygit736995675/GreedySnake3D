﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeUtils 
{
    // DateTime --> long
    public static long DateTimeToLong (this DateTime dt)
    {
        DateTime dtStart = TimeZone. CurrentTimeZone. ToLocalTime(new DateTime(1970, 1, 1));
        TimeSpan toNow = dt. Subtract(dtStart);
        long timeStamp = toNow. Ticks;
        timeStamp = long. Parse(timeStamp. ToString(). Substring(0, timeStamp. ToString(). Length - 4));
        return timeStamp;
    }


    // long --> DateTime
    public static DateTime LongToDateTime (this long d)
    {
        DateTime dtStart = TimeZone. CurrentTimeZone. ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long. Parse(d + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart. Add(toNow);
        return dtResult;
    }
}
