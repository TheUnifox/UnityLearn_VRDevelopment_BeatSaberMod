using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200006C RID: 108
public static class PingUtility
{
    // Token: 0x060004BF RID: 1215 RVA: 0x0000CB44 File Offset: 0x0000AD44
    public static async Task<long> PingAsync(string url)
    {
        IPAddress[] source = await Dns.GetHostAddressesAsync(url);
        Ping[] pings = (from ip in source
                        select new Ping(ip.ToString())).ToArray<Ping>();
        long ping = -1L;
        bool flag = false;
        bool found = false;
        int t = 0;
        while (t < 1000 && !flag && !found)
        {
            await Task.Delay(10);
            flag = true;
            foreach (Ping ping2 in pings)
            {
                if (ping2.isDone && ping2.time > 0)
                {
                    ping = (long)ping2.time;
                    found = true;
                    break;
                }
                if (!ping2.isDone)
                {
                    flag = false;
                }
            }
            t += 10;
        }
        Ping[] array = pings;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].DestroyPing();
        }
        return ping;
    }
}