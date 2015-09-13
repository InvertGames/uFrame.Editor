using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IDynamicOptionsCommand
    {
        Type For { get; }
        IEnumerable<UFContextMenuItem> GetOptions(object item);
        UFContextMenuItem SelectedOption { get; set; }
        MultiOptionType OptionsType { get; }
    }
}