using System;
using BGNet.Logging;

// Token: 0x02000029 RID: 41
public static class GameplayServerFiniteStateMachineFactory
{
    // Token: 0x06000186 RID: 390 RVA: 0x00006A94 File Offset: 0x00004C94
    public static GameplayServerFiniteStateMachine Create(GameplayServerFiniteStateMachine.InitParams initParams)
    {
        switch (initParams.configuration.gameplayServerMode)
        {
            case GameplayServerMode.Countdown:
                return new CountdownGameplayServerFiniteStateMachine(initParams);
            case GameplayServerMode.Managed:
                return new ManagedGameplayServerFiniteStateMachine(initParams);
            case GameplayServerMode.QuickStartOneSong:
                return new QuickStartOneSongGameplayServerFiniteStateMachine(initParams);
            default:
                Debug.LogError(string.Format("[GSFSM] Unexpected GameplayServerMode {0}", initParams.configuration.gameplayServerMode));
                return new GameplayServerFiniteStateMachine(initParams);
        }
    }
}
