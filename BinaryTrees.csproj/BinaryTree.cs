using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTreeNode<T>
    {

        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }
        public T Value { get; }
        public int Size = 1;

        public BinaryTreeNode(T value)
        {
            Value = value;
        }
    }


    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private BinaryTreeNode<T> treeNode;
        public BinaryTreeNode<T> AddToTree(BinaryTreeNode<T> side,T key, ref bool flag)
        {
            if (side == null)
            {
                flag = false;
                return new BinaryTreeNode<T>(key);
            }
            return side;
        }

        public void Add(T key)
        {
            var newBinaryNode = treeNode;
            if (treeNode == null)
                treeNode = new BinaryTreeNode<T>(key);
            else
            {
                bool flag = true;
                while (flag)
                {
                    newBinaryNode.Size++;
                    if (newBinaryNode.Value.CompareTo(key) > 0)
                    {
                        newBinaryNode.Left = AddToTree(newBinaryNode.Left, key, ref flag);
                        if (flag) newBinaryNode = newBinaryNode.Left;
                    }
                    else
                    {
                        newBinaryNode.Right = AddToTree(newBinaryNode.Right, key, ref flag);
                        if (flag) newBinaryNode = newBinaryNode.Right;
                    }
                }
            }
        }


        public bool Contains(T key)
        {
            var newTreeNode = treeNode;
            while (newTreeNode != null)
            {
                var result = newTreeNode.Value.CompareTo(key);
                if (result == 0)
                    return true;
                newTreeNode = result > 0 ? newTreeNode.Left : newTreeNode.Right;
            }

            return false;
        }
        public T this[int i]
        {
            get
            {
                var tree = treeNode;
                while (true)
                {
                    if (tree == null) continue;
                    var leftSize = tree.Left?.Size ?? 0;
                    if (i == leftSize)
                        return tree.Value;
                    if (i < leftSize)
                        tree = tree.Left;
                    else if (i > leftSize)
                    {
                        tree = tree.Right;
                        i -= leftSize + 1;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetValues(treeNode).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IEnumerable<T> GetValues(BinaryTreeNode<T> treeNode)
        {
            while (true)
            {
                if (treeNode == null) yield break;

                foreach (var value in GetValues(treeNode.Left))
                    yield return value;

                yield return treeNode.Value;

                treeNode = treeNode.Right;
            }
        }
    }
}
