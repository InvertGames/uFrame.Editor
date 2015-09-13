using System;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace Invert.Core.GraphDesigner
{
    public class LambdaFileSyncCommand : LambdaCommand, IFileSyncCommand
    {
        public LambdaFileSyncCommand(string title, Action action) : base(title, action)
        {
        }
    }
}
