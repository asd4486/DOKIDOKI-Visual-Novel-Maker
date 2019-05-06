using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DokiVnMaker.MyEditor.Items
{
    public class NodeBaseList
    {
        public List<NodeBase> Nodes = new List<NodeBase>();
        
        //set node unique id
        public int SetNodeId()
        {
            if (Nodes == null) return 0;
            var id = 0;
            foreach (var n in Nodes)
            {
                if (id <= n.Id) id = n.Id + 1;
            }
            return id;
        }
    }
}
