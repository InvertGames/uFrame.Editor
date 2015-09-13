﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Invert.Core.GraphDesigner.Unity.KoinoniaSystem.Commands
{
    public class RunQueuedOperationsCommand : IBackgroundCommand
    {
        public BackgroundWorker Worker { get; set; }
        public string Title { get; set; }
    }
}
