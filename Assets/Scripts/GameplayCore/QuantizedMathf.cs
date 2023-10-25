using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public abstract class QuantizedMathf
{
    // Token: 0x060000F5 RID: 245 RVA: 0x00005504 File Offset: 0x00003704
    public static bool Approximately(Quaternion a, Quaternion b)
    {
        int num = 0;
        int num2 = 0;
        Mathf.Abs(a.x - b.x);
        Mathf.Abs(a.y - b.y);
        Mathf.Abs(a.z - b.z);
        Mathf.Abs(a.w - b.w);
        if (QuantizedMathf.Approximately(a.x, b.x, 6.103888E-05f))
        {
            num++;
        }
        else if (QuantizedMathf.Approximately(a.x, b.x, 0.00012207776f))
        {
            num2++;
        }
        if (QuantizedMathf.Approximately(a.y, b.y, 6.103888E-05f))
        {
            num++;
        }
        else if (QuantizedMathf.Approximately(a.y, b.y, 0.00012207776f))
        {
            num2++;
        }
        if (QuantizedMathf.Approximately(a.z, b.z, 6.103888E-05f))
        {
            num++;
        }
        else if (QuantizedMathf.Approximately(a.z, b.z, 0.00012207776f))
        {
            num2++;
        }
        if (QuantizedMathf.Approximately(a.w, b.w, 6.103888E-05f))
        {
            num++;
        }
        else if (QuantizedMathf.Approximately(a.w, b.w, 0.00012207776f))
        {
            num2++;
        }
        return num == 4 || (num == 3 && num2 == 1);
    }

    // Token: 0x060000F6 RID: 246 RVA: 0x00005659 File Offset: 0x00003859
    public static bool Approximately(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

    // Token: 0x060000F7 RID: 247 RVA: 0x00005666 File Offset: 0x00003866
    public static bool Approximately(int a, int b, int epsilon)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

    // Token: 0x060000F8 RID: 248 RVA: 0x00005674 File Offset: 0x00003874
    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return QuantizedMathf.Approximately(a.x, b.x, 0.001f) && QuantizedMathf.Approximately(a.y, b.y, 0.001f) && QuantizedMathf.Approximately(a.z, b.z, 0.001f);
    }

    // Token: 0x060000F9 RID: 249 RVA: 0x000056C9 File Offset: 0x000038C9
    public static bool Approximately(Pose a, Pose b)
    {
        return QuantizedMathf.Approximately(a.position, b.position) && QuantizedMathf.Approximately(a.rotation, b.rotation);
    }

    // Token: 0x060000FA RID: 250 RVA: 0x000056F1 File Offset: 0x000038F1
    public static string QuantizedVectorComponentToString(int v)
    {
        if (v >= 0)
        {
            return string.Format("{0}.{1:000}", (float)v / 1000f, (float)v % 1000f);
        }
        return "-" + QuantizedMathf.QuantizedVectorComponentToString(-v);
    }

    // Token: 0x040000ED RID: 237
    public const int kQuaternionSerializableScaleFactor = 16383;

    // Token: 0x040000EE RID: 238
    private const float kQuaternionSerializableEpsilon = 6.103888E-05f;

    // Token: 0x040000EF RID: 239
    private const int kVectorSerializableScaleInt = 1000;

    // Token: 0x040000F0 RID: 240
    public const float kVectorSerializableScale = 1000f;

    // Token: 0x040000F1 RID: 241
    private const float kVectorSerializableEpsilon = 0.001f;
}
