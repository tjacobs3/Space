using System.Collections;
using System.Collections.Generic;

public class Edge<T> {
    public T a;
    public T b;

    public Edge(T start, T endpoint) {
        a = start;
        b = endpoint;
    }
}
