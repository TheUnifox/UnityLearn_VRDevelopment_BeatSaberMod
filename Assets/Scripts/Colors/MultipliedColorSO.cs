using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class MultipliedColorSO : ColorSO
{
    // Token: 0x17000014 RID: 20
    // (get) Token: 0x0600001D RID: 29 RVA: 0x0000226A File Offset: 0x0000046A
    public override Color color
    {
        get
        {
            return this._multiplierColor * this._baseColor.color;
        }
    }

    // Token: 0x04000015 RID: 21
    [SerializeField]
    private SimpleColorSO _baseColor;

    // Token: 0x04000016 RID: 22
    [SerializeField]
    private Color _multiplierColor;
}
