using System;

namespace Invert.Core.GraphDesigner
{
    public interface IChildCommand
    {
        Type ChildCommandFor { get; }
    }
}