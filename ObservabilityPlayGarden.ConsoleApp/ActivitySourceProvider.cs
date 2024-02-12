using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservabilityPlayGarden.ConsoleApp
{
    internal class ActivitySourceProvider
    {
        public static ActivitySource Source = new ActivitySource(StringConsts.ActivitySourceName);
        public static ActivitySource SourceFile = new ActivitySource(StringConsts.ActivitySourceFileName);
    }
}
