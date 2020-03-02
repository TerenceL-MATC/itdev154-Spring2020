using System;
using System.Collections.Generic;
using System.Text;

namespace MyLinkedListProject
{
    class Node
    {
        public int data;
        public Node link;

        public Node(int info)
        {
            data = info;
            link = null;
        }
    }
}
