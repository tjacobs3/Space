using System.Collections;
using System.Collections.Generic;

public class TreeNode<T> {
    public T data;
    public List<TreeNode<T>> neighbors;

    public TreeNode(T data)
    {
        this.data = data;
        neighbors = new List<TreeNode<T>>();
    }
}
