using System;

// Token: 0x02000026 RID: 38
public abstract class GameState : IDisposable
{
    // Token: 0x06000172 RID: 370 RVA: 0x0000697C File Offset: 0x00004B7C
    public GameState(GameplayServerFiniteStateMachine fsm)
    {
        this.fsm = fsm;
    }

    // Token: 0x06000173 RID: 371
    public abstract void Dispose();

    // Token: 0x06000174 RID: 372
    public abstract void Init();

    // Token: 0x040000E5 RID: 229
    protected readonly GameplayServerFiniteStateMachine fsm;
}
