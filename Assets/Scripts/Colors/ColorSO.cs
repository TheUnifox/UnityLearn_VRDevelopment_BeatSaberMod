using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public abstract class ColorSO : PersistentScriptableObject
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000001 RID: 1
    public abstract Color color { get; }

    // Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
    public static implicit operator Color(ColorSO c)
    {
        if (c == null)
        {
            return Color.clear;
        }
        return c.color;
    }
}
