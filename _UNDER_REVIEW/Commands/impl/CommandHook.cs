using System;

namespace Invert.Core.GraphDesigner
{
    public class CommandHook
    {
        public CommandHookLifetime Lifetime { get; set; }
        public Action Action { get; set; }
    }
}