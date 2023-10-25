using System;

// Token: 0x02000039 RID: 57
public interface IEquatableByReference<T> where T : struct
{
    // Token: 0x060001D1 RID: 465
    bool Equals(in T other);
}
