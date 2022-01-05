using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class circlePacking : MonoBehaviour
{
    [SerializeField] GameObject circleCenter;
    List<Circle> circles;
    private RectTransform parentRectT;
    public int count;
    public int attempts;
    private int numCircles;
    public static float width;
    public static float height;
    public bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        circles = new List<Circle>();
        parentRectT = gameObject.GetComponent<RectTransform>();
        width = parentRectT.rect.width;
        height = parentRectT.rect.height;
        count = 1;
        numCircles = 10;
        attempts = 0;

        StartCoroutine(GenerateCircles());
    }

    
    IEnumerator GenerateCircles()
    {
        while (count < numCircles + 1)
        {
            Circle newC = newCircle();

            if (newC != null)
            {
                circles.Add(newC);
                GameObject cCenter = Instantiate(circleCenter, newC.circleObj.transform.position, Quaternion.identity);
                cCenter.transform.parent = newC.circleObj.transform;
                count++;
                Debug.Log("count = " + count);
            }


            foreach (Circle c in circles)
            {
                c.growing = true;
                while (c.edges())
                {
                    //check to make sure circle c does not overlap otherC circles
                    foreach (Circle otherC in circles)
                    {
                        if (c.circleObj != otherC.circleObj)    //exclude checking against itself
                        {
                            float d = Distance(c.circleObj.transform.position, otherC.circleObj.transform.position);
                            if (d - c.lnwidth + 0.001 < c.r + otherC.r)
                            {
                                c.growing = false;
                                break;
                            }
                        }
                    }
                    c.show();
                    c.grow();
                    yield return null;
                }
            }
            count++;
        }
    }
    

    public Circle newCircle()
    {
        float padding = 0.05f;  //add some offset to the bounds so that a circle doesn't generate too close to the bounds

        Vector2 circlePosition = new Vector2(Random.Range(-width / 2 + padding, width / 2 - padding), Random.Range(-height / 2 + padding, height / 2 - padding));

        //check to make sure new circle location is not inside another circle in the list
        bool valid = true;
        foreach (Circle c in circles)
        {
            float d = Distance(circlePosition, c.circleObj.transform.position);
            if (d < c.r)
            {
                valid = false;
                break;
            }
        }

        if (valid)
        {
            return new Circle(circlePosition.x, circlePosition.y, count);
        }
        else
        {
            return null;
        }
    }

    public float Distance(Vector2 obj, Vector2 c)
    {
        float x = obj.x;
        float y = obj.y;
        float cx = c.x;
        float cy = c.y;

        return Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
    }
}

