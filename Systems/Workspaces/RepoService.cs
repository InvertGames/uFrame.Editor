using Invert.Data;
using Invert.IOC;

namespace Invert.Core.GraphDesigner
{
    public class RepoService : DiagramPlugin
    {
        public IRepository Repository { get; set; }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);

        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            Repository = container.Resolve<IRepository>();
        }
    }
}