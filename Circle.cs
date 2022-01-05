using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
    private float x;
    private float y;
    public float r;
    private int segments;
    private int pointCount;
    private LineRenderer line;
    public bool growing;
    public float lnwidth;

    public GameObject circleObj;

    public Circle(float _x, float _y, int ind)
    {
        x = _x;
        y = _y;

        r = 0.1f;
        segments = 360;
        pointCount = segments + 1;  //add extra point to make startpoint and endpoint the same to close the circle
        growing = true;

        circleObj = new GameObject() { name = "Circle " + ind };
        circleObj.AddComponent<LineRenderer>();
        circleObj.transform.position = new Vector2(x, y);

        line = circleObj.GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = 0.04f;
        line.endWidth = 0.04f;
        lnwidth = line.startWidth;
        line.positionCount = segments + 1;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;
    }

    public void show()
    {
        var points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Cos(rad) * r, Mathf.Sin(rad) * r, 0);
        }

        line.SetPositions(points);
    }

    public void grow()
    {
        if (growing)
        {
            r += 0.02f;
        }
    }

    public bool edges()
    {
        //check to make sure circle is not exceeding the bounds
        return !((Mathf.Abs(x) + r + lnwidth) > circlePacking.width / 2 || (Mathf.Abs(y) + r + lnwidth) > circlePacking.height / 2);
    }
}
