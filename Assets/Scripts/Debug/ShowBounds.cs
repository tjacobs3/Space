using UnityEngine;
using System.Collections;

public class ShowBounds : MonoBehaviour {

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;

    private Bounds bounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DrawBox();
	}

    void CalcPositons()
    {
        if (bounds == null)
            return;

        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
    }

    void DrawBox()
    {
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, Color.green);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, Color.green);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, Color.green);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, Color.green);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, Color.green);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, Color.green);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, Color.green);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, Color.green);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, Color.green);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, Color.green);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, Color.green);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, Color.green);
    }

    public void setBounds(Bounds bounds)
    {
        this.bounds = bounds;
        CalcPositons();
    }
}
