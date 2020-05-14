using System;
using System.Collections.Generic;
using System.Text;

namespace MyHashTableProject
{
    class Node
    {
        public int key;
        public string name;
        public Node link;

        public Node(int theKey, string theName)
        {
            key = theKey;
            name = theName;
            link = null;
        }
    }
}
