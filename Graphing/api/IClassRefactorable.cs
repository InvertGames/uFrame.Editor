using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IClassRefactorable 
    {
        IEnumerable<string> ClassNameFormats { get; }
    }
}