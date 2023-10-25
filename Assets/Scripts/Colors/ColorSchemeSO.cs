using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class ColorSchemeSO : PersistentScriptableObject
{
    // Token: 0x17000011 RID: 17
    // (get) Token: 0x06000017 RID: 23 RVA: 0x00002227 File Offset: 0x00000427
    public ColorScheme colorScheme
    {
        get
        {
            return this._colorScheme;
        }
    }

    // Token: 0x04000010 RID: 16
    [SerializeField]
    private ColorScheme _colorScheme;
}
