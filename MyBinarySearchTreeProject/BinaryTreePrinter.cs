/* The code in this file was borrowed from Ivan Stoev, a user on StackOverflow.
 * I made some tweaks to match my own code and fix a formatting problem that the class
 * had with printing nodes that had a key value that was negative.
*/
using System;
using System.Collections.Generic;

namespace MyBinarySearchTreeProject
{
    //Displays a binary tree in a more natural way (vertically).
    static class BinaryTreePrinter
    {
        public static void Print(this Node root, int topMargin = 2, int leftMargin = 2)
        {
            if (root == null)
            {
                Console.WriteLine("The binary search tree is empty.");
                return;
            }
                
            int rootTop = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = root;
            for (int level = 0; next != null; level++)
            {
                var item = new NodeInfo { node = next, text = $" {next.data} " };
                if (level < last.Count)
                {
                    item.startPos = last[level].EndPos + 1;
                    last[level] = item;
                }
                else
                {
                    item.startPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.parent = last[level - 1];
                    if (next == item.parent.node.leftChild)
                    {
                        item.parent.leftChild = item;
                        item.EndPos = Math.Max(item.EndPos, item.parent.startPos);
                    }
                    else
                    {
                        item.parent.rightChild = item;
                        item.startPos = Math.Max(item.startPos, item.parent.EndPos);
                    }
                }
                next = next.leftChild ?? next.rightChild;
                for (; next == null; item = item.parent)
                {
                    Print(item, rootTop + 2 * level);
                    if (--level < 0) break;
                    if (item == item.parent.leftChild)
                    {
                        item.parent.startPos = item.EndPos;
                        next = item.parent.node.rightChild;
                    }
                    else
                    {
                        if (item.parent.leftChild == null)
                            item.parent.EndPos = item.startPos;
                        else
                            item.parent.startPos += (item.startPos - item.parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void Print(NodeInfo item, int top)
        {
            SwapColors();
            Print(item.text, top, item.startPos);
            SwapColors();
            if (item.leftChild != null)
                PrintLink(top + 1, "┌", "┘", item.leftChild.startPos + item.leftChild.Size / 2, item.startPos);
            if (item.rightChild != null)
                PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.rightChild.startPos + item.rightChild.Size / 2);
        }

        private static void PrintLink(int top, string start, string end, int startPos, int endPos)
        {
            Print(start, top, startPos);
            Print("─", top, startPos + 1, endPos);
            Print(end, top, endPos);
        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }

        private static void SwapColors()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
        }
    }
}
