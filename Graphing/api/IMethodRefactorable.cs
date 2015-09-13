using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IMethodRefactorable 
    {
        IEnumerable<string> MethodFormats { get; }
    }
}