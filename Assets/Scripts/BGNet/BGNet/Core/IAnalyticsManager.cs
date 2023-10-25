using System;

namespace BGNet.Core
{
    // Token: 0x020000A2 RID: 162
    public interface IAnalyticsManager : IPollable, IDisposable
    {
        // Token: 0x06000617 RID: 1559
        void UpdateState(MetricIdentifier metricIdentifier, long state, AnalyticsMetricUnit unit = AnalyticsMetricUnit.None, bool alarmMetric = false);

        // Token: 0x06000618 RID: 1560
        void UpdateAverage(MetricIdentifier metricIdentifier, double value, AnalyticsMetricUnit unit = AnalyticsMetricUnit.None, bool alarmMetric = false);

        // Token: 0x06000619 RID: 1561
        void IncrementCounter(MetricIdentifier metricIdentifier, long incrementAmount = 1L, AnalyticsMetricUnit unit = AnalyticsMetricUnit.Count);
    }
}
