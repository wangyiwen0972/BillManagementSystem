using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.Model;

namespace EE.BM
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true)]
    public class PermissionAttribute:Attribute
    {
        private int rightID;
        public int RightID { get { return rightID; } }

        private Action action;
        public Action Action { get { return action; } }

        public PermissionAttribute(int rightID, Action action)
        {
            this.rightID = rightID;
            this.action = action;
        }
    }
    [Flags]
    public enum Action
    {
        Invisible = 0,
        Visible = 1,
        Executable = 2,
    }
}
