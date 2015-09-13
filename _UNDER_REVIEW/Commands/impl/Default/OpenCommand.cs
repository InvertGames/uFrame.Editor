using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public class OpenCommand : EditorCommand<DiagramNodeViewModel>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override bool CanProcessMultiple
        {
            get { return false; }
        }

        public override string Group
        {
            get { return "File"; }
        }
        public override void Perform(DiagramNodeViewModel node)
        {
            var generator = SelectedOption.Value as OutputGenerator;
            if (generator == null) return;
           // var pathStrategy = node.GraphItemObject.Graph.CodePathStrategy;


            //var filename = repository.GetControllerCustomFilename(this.Name);
          //  InvertGraphEditor.Platform.OpenScriptFile("Assets" + generator.UnityPath);
          
        }

        public override string CanPerform(DiagramNodeViewModel node)
        {
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var diagramItem = item as DiagramNodeViewModel;
            if (diagramItem == null) yield break;
            
            var generators = diagramItem.CodeGenerators.ToArray();

            foreach (var codeGenerator in generators.Where(p=>!p.AlwaysRegenerate))
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Open/" + codeGenerator.Filename,
                    Value = codeGenerator
                };
            }
            foreach (var codeGenerator in generators.Where(p => p.AlwaysRegenerate))
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Open/Designer Files/" + codeGenerator.Filename,
                    Value = codeGenerator
                };
            }
        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }
}