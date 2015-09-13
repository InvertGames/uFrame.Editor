using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IIdentifierRefactorable 
    {
        IEnumerable<string> IdentifierFormats { get;  }
    }
}