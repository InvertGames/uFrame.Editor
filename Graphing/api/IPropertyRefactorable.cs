using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IPropertyRefactorable : ITypedItem
    {
        IEnumerable<string> PropertyFormats { get; }
    }
}