using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200023C RID: 572
    [NoReflectionBaking]
    [ZenjectAllowDuringValidation]
    [Serializable]
    public class ZenjectSettings
    {
        // Token: 0x06000D27 RID: 3367 RVA: 0x00023854 File Offset: 0x00021A54
        public ZenjectSettings(ValidationErrorResponses validationErrorResponse, RootResolveMethods validationRootResolveMethod = RootResolveMethods.NonLazyOnly, bool displayWarningWhenResolvingDuringInstall = true, bool ensureDeterministicDestructionOrderOnApplicationQuit = false, ZenjectSettings.SignalSettings signalSettings = null)
        {
            this._validationErrorResponse = validationErrorResponse;
            this._validationRootResolveMethod = validationRootResolveMethod;
            this._displayWarningWhenResolvingDuringInstall = displayWarningWhenResolvingDuringInstall;
            this._ensureDeterministicDestructionOrderOnApplicationQuit = ensureDeterministicDestructionOrderOnApplicationQuit;
            this._signalSettings = (signalSettings ?? ZenjectSettings.SignalSettings.Default);
        }

        // Token: 0x06000D28 RID: 3368 RVA: 0x0002388C File Offset: 0x00021A8C
        public ZenjectSettings() : this(ValidationErrorResponses.Log, RootResolveMethods.NonLazyOnly, true, false, null)
        {
        }

        // Token: 0x170000EC RID: 236
        // (get) Token: 0x06000D29 RID: 3369 RVA: 0x0002389C File Offset: 0x00021A9C
        public ZenjectSettings.SignalSettings Signals
        {
            get
            {
                return this._signalSettings;
            }
        }

        // Token: 0x170000ED RID: 237
        // (get) Token: 0x06000D2A RID: 3370 RVA: 0x000238A4 File Offset: 0x00021AA4
        public ValidationErrorResponses ValidationErrorResponse
        {
            get
            {
                return this._validationErrorResponse;
            }
        }

        // Token: 0x170000EE RID: 238
        // (get) Token: 0x06000D2B RID: 3371 RVA: 0x000238AC File Offset: 0x00021AAC
        public RootResolveMethods ValidationRootResolveMethod
        {
            get
            {
                return this._validationRootResolveMethod;
            }
        }

        // Token: 0x170000EF RID: 239
        // (get) Token: 0x06000D2C RID: 3372 RVA: 0x000238B4 File Offset: 0x00021AB4
        public bool DisplayWarningWhenResolvingDuringInstall
        {
            get
            {
                return this._displayWarningWhenResolvingDuringInstall;
            }
        }

        // Token: 0x170000F0 RID: 240
        // (get) Token: 0x06000D2D RID: 3373 RVA: 0x000238BC File Offset: 0x00021ABC
        public bool EnsureDeterministicDestructionOrderOnApplicationQuit
        {
            get
            {
                return this._ensureDeterministicDestructionOrderOnApplicationQuit;
            }
        }

        // Token: 0x040003C7 RID: 967
        public static ZenjectSettings Default = new ZenjectSettings();

        // Token: 0x040003C8 RID: 968
        [SerializeField]
        private bool _ensureDeterministicDestructionOrderOnApplicationQuit;

        // Token: 0x040003C9 RID: 969
        [SerializeField]
        private bool _displayWarningWhenResolvingDuringInstall;

        // Token: 0x040003CA RID: 970
        [SerializeField]
        private RootResolveMethods _validationRootResolveMethod;

        // Token: 0x040003CB RID: 971
        [SerializeField]
        private ValidationErrorResponses _validationErrorResponse;

        // Token: 0x040003CC RID: 972
        [SerializeField]
        private ZenjectSettings.SignalSettings _signalSettings;

        // Token: 0x0200023D RID: 573
        [Serializable]
        public class SignalSettings
        {
            // Token: 0x06000D2F RID: 3375 RVA: 0x000238D0 File Offset: 0x00021AD0
            public SignalSettings(SignalDefaultSyncModes defaultSyncMode, SignalMissingHandlerResponses missingHandlerDefaultResponse = SignalMissingHandlerResponses.Warn, bool requireStrictUnsubscribe = false, int defaultAsyncTickPriority = 1)
            {
                this._defaultSyncMode = defaultSyncMode;
                this._missingHandlerDefaultResponse = missingHandlerDefaultResponse;
                this._requireStrictUnsubscribe = requireStrictUnsubscribe;
                this._defaultAsyncTickPriority = defaultAsyncTickPriority;
            }

            // Token: 0x06000D30 RID: 3376 RVA: 0x000238F8 File Offset: 0x00021AF8
            public SignalSettings() : this(SignalDefaultSyncModes.Synchronous, SignalMissingHandlerResponses.Warn, false, 1)
            {
            }

            // Token: 0x170000F1 RID: 241
            // (get) Token: 0x06000D31 RID: 3377 RVA: 0x00023904 File Offset: 0x00021B04
            public int DefaultAsyncTickPriority
            {
                get
                {
                    return this._defaultAsyncTickPriority;
                }
            }

            // Token: 0x170000F2 RID: 242
            // (get) Token: 0x06000D32 RID: 3378 RVA: 0x0002390C File Offset: 0x00021B0C
            public SignalDefaultSyncModes DefaultSyncMode
            {
                get
                {
                    return this._defaultSyncMode;
                }
            }

            // Token: 0x170000F3 RID: 243
            // (get) Token: 0x06000D33 RID: 3379 RVA: 0x00023914 File Offset: 0x00021B14
            public SignalMissingHandlerResponses MissingHandlerDefaultResponse
            {
                get
                {
                    return this._missingHandlerDefaultResponse;
                }
            }

            // Token: 0x170000F4 RID: 244
            // (get) Token: 0x06000D34 RID: 3380 RVA: 0x0002391C File Offset: 0x00021B1C
            public bool RequireStrictUnsubscribe
            {
                get
                {
                    return this._requireStrictUnsubscribe;
                }
            }

            // Token: 0x06000D36 RID: 3382 RVA: 0x00023930 File Offset: 0x00021B30
            private static object __zenCreate(object[] P_0)
            {
                return new ZenjectSettings.SignalSettings();
            }

            // Token: 0x06000D37 RID: 3383 RVA: 0x00023948 File Offset: 0x00021B48
            [Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(ZenjectSettings.SignalSettings), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ZenjectSettings.SignalSettings.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x040003CD RID: 973
            public static ZenjectSettings.SignalSettings Default = new ZenjectSettings.SignalSettings();

            // Token: 0x040003CE RID: 974
            [SerializeField]
            private SignalDefaultSyncModes _defaultSyncMode;

            // Token: 0x040003CF RID: 975
            [SerializeField]
            private SignalMissingHandlerResponses _missingHandlerDefaultResponse;

            // Token: 0x040003D0 RID: 976
            [SerializeField]
            private bool _requireStrictUnsubscribe;

            // Token: 0x040003D1 RID: 977
            [SerializeField]
            private int _defaultAsyncTickPriority;
        }
    }
}
