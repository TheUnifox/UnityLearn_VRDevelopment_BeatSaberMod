using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class ColorSchemesListSO : ScriptableObject
{
    // Token: 0x17000012 RID: 18
    // (get) Token: 0x06000019 RID: 25 RVA: 0x0000222F File Offset: 0x0000042F
    public ColorSchemeSO[] colorSchemes
    {
        get
        {
            return this._colorSchemes;
        }
    }

    // Token: 0x04000011 RID: 17
    [SerializeField]
    private ColorSchemeSO[] _colorSchemes;
}
