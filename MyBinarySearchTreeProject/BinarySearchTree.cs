using static System.Console;

namespace MyBinarySearchTreeProject
{
    class BinarySearchTree
    {
        private Node root;

        public BinarySearchTree()
        {
            root = null;
        }

        public void Insert(int key)
        {
            if(Insert(ref root, key)) //We were able to insert the key into the search tree.
            {
                WriteLine($"{key} was successfully inserted in the search tree.");
            }
            else //Key is already in the search tree.
            {
                WriteLine($"{key} is already in the search tree.");
            }
        }

        private bool Insert(ref Node rootNode, int key)
        {
            if (rootNode is null) //We have found the location to insert the new key in the search tree
            {
                rootNode = new Node(key);
                return true;
            }

            if (key < rootNode.data)
            {
                return Insert(ref rootNode.leftChild, key);
            }

            if (key > rootNode.data)
            {
                return Insert(ref rootNode.rightChild, key);
            }

            return false; //key is already in the search tree
        }

        public void Delete(int key)
        {
            Node nullNode = null; //The root node of the search tree doesn't have a parent.

            if(Delete(ref nullNode, ref root, key)) //Key has been deleted to the search tree
            {
                WriteLine($"{key} was successfully deleted from the search tree.");
            }
            else //Key wasn't found in the search tree 
            {
                WriteLine(@$"{key} wasn't found in the search tree.");
            }
        }

        private bool Delete(ref Node parentNode, ref Node pNode, int key)
        {
            if(pNode is null) //Tree is empty and/or we have reached an external node
            {
                return false;
            }

            if(key < pNode.data) // key is less than the key of pNode
            {
                return Delete(ref pNode, ref pNode.leftChild, key);
            }

            if(key > pNode.data) //key is greater than the key of pNode
            {
                return Delete(ref pNode, ref pNode.rightChild, key);
            }

            //key == pNode.data
            if (pNode.leftChild != null && pNode.rightChild != null) //Node to be deleted has two child nodes
            {
                Node successor = pNode.rightChild, 
                        parentOfSuccessor = pNode;

                //Searching for the inorder successor
                while (successor.leftChild != null)
                {
                    parentOfSuccessor = successor;
                    successor = successor.leftChild;
                }

                pNode.data = successor.data;
                    
                //Deletes the successor node from the search tree.
                if(parentOfSuccessor == pNode)
                {
                    parentOfSuccessor.rightChild = successor.rightChild;
                }
                else
                {
                    parentOfSuccessor.leftChild = successor.rightChild;
                }
            }
            else //Node to be deleted has one child node or less
            {
                //Finds the child of the node to be deleted even if it's an external node
                Node orphanChild = pNode.leftChild ?? pNode.rightChild;

                if (parentNode != null) //Node to be deleted isn't the root node
                {
                    if (pNode == parentNode.leftChild) //Node to be deleted is a left child
                    {
                        parentNode.leftChild = orphanChild;
                    }
                    else //Node to be deleted is a right child
                    {
                        parentNode.rightChild = orphanChild;
                    }
                }
                else //Deleting the root node of the tree
                {
                    pNode = orphanChild;
                }
            }

            return true;
            
        }

        public void Display()
        {
            BinaryTreePrinter.Print(root);
        }
    }
}
