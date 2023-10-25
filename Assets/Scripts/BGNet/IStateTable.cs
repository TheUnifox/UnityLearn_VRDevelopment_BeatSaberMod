using System;

// Token: 0x0200008C RID: 140
public interface IStateTable<TStateTable, TType, TState> where TStateTable : IStateTable<TStateTable, TType, TState> where TType : struct, IConvertible where TState : struct
{
    // Token: 0x060005B8 RID: 1464
    TState GetState(TType type);

    // Token: 0x060005B9 RID: 1465
    void SetState(TType type, TState state);

    // Token: 0x060005BA RID: 1466
    TStateTable GetDelta(in TStateTable stateTable);

    // Token: 0x060005BB RID: 1467
    TStateTable ApplyDelta(in TStateTable delta);

    // Token: 0x060005BC RID: 1468
    int GetSize();
}
