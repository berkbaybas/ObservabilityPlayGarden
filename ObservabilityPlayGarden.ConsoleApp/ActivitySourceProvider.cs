using System.Diagnostics;

namespace ObservabilityPlayGarden.ConsoleApp;

internal class ActivitySourceProvider
{
    public static ActivitySource Source = new ActivitySource(StringConsts.ActivitySourceName);
    public static ActivitySource SourceFile = new ActivitySource(StringConsts.ActivitySourceFileName);
}
