/* The code in this file was borrowed from Ivan Stoev, a user on StackOverflow.
   I made some tweaks to match my own code.
*/

namespace MyBinarySearchTreeProject
{
    class NodeInfo
    {
        public Node node;
        public string text;
        public int startPos;
        public NodeInfo parent, leftChild, rightChild;

        public int Size { get { return text.Length; } }
        public int EndPos { get { return startPos + Size; } set { startPos = value - Size; } }
    }
}
