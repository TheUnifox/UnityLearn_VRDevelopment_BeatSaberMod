using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class SimpleColorSO : ColorSO
{
    // Token: 0x17000015 RID: 21
    // (get) Token: 0x0600001F RID: 31 RVA: 0x00002282 File Offset: 0x00000482
    public override Color color
    {
        get
        {
            return this._color;
        }
    }

    // Token: 0x06000020 RID: 32 RVA: 0x0000228A File Offset: 0x0000048A
    public void SetColor(Color c)
    {
        this._color = c;
    }

    // Token: 0x04000017 RID: 23
    [SerializeField]
    protected Color _color;
}
