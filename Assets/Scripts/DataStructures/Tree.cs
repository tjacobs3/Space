using System.Collections;
using System.Collections.Generic;

public class Tree<T> {
    public List<TreeNode<T>> nodes;

    public Tree() {
        nodes = new List<TreeNode<T>>();
    }

    public List<Edge<TreeNode<T>>> createMST()
    {
        List<TreeNode<T>> pickedNodes = new List<TreeNode<T>>();
        List<Edge<TreeNode<T>>> pickedEdges = new List<Edge<TreeNode<T>>>();
        List<Edge<TreeNode<T>>> validEdges = new List<Edge<TreeNode<T>>>();

        while (pickedNodes.Count < nodes.Count)
        {
            if(pickedNodes.Count == 0)
                pickedNodes.Add(nodes[0]);

            foreach (TreeNode<T> n in pickedNodes[pickedNodes.Count - 1].neighbors)
            {
                validEdges.Add(new Edge<TreeNode<T>>(pickedNodes[pickedNodes.Count - 1], n));
            }

            validEdges.RemoveAll(edge =>
            {
                return pickedNodes.Contains(edge.a) && pickedNodes.Contains(edge.b);
            });

            if (validEdges.Count > 0)
            {
                Edge<TreeNode<T>> pe = validEdges[validEdges.Count - 1];
                pickedEdges.Add(pe);
                if (!pickedNodes.Contains(pe.a))
                    pickedNodes.Add(pe.a);
                else
                    pickedNodes.Add(pe.b);
                validEdges.RemoveAt(validEdges.Count - 1);
            }
        }
        return pickedEdges;
    }
}
