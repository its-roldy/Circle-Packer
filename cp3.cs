using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using TMPro;


public class cp3 : MonoBehaviour
{
    [SerializeField] GameObject circleCenter;
    [SerializeField] public GameObject circlePrefab;
    [SerializeField] public GameObject parentObj;
    public List<GameObject> circles;
    public List<Vector2> cPositions;
    private RectTransform parentRectT;
    public static int count;
    public int numCircles;
    public static float width;
    public static float height;
    public Vector2 circlePosition = new Vector2();
    public int iteration;
    public static float elapsedTime;
    public DateTime timeStart = new DateTime();
    public DateTime timeNow = new DateTime();
    public TimeSpan deltaTime = TimeSpan.Zero;
    public bool valid = true;
    public int counter;


    // Start is called before the first frame update
    void Start()
    {
        circles = new List<GameObject>();
        cPositions = new List<Vector2>();
        parentRectT = gameObject.GetComponent<RectTransform>();
        width = parentRectT.rect.width;
        height = parentRectT.rect.height;
        count = 1;
        numCircles = 100;
        counter = 0;

        iteration = 0;

        generatePositions();
        StartCoroutine(GenerateCircles());
    }

    private void generatePositions()
    {
        int i = 0;

        do
        {
            //write code to generate field of positions using poisson disk sampling
            Vector2 circlePosition = new Vector2(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2));
            
            /*
            if (i == 0)
            {
                Vector2 circlePosition = new Vector2(1, 0);
                cPositions.Add(circlePosition);
            }
            else if (i == 1)
            {
                Vector2 circlePosition = new Vector2(0, 0);
                cPositions.Add(circlePosition);
            }
            */

            cPositions.Add(circlePosition);
            i++;
        } while (i < numCircles);

        GameObject newC = null;
        i = 0;
        do
        {
            newC = newCircle();
            newC.name = "Circle" + (i + 1);

            if (newC != null)
            {
                circles.Add(newC);
                /*
                if (i == 0)
                {
                   newC.GetComponent<circleProps3>().SetSpawn(0);
                }
                else
                {
                    newC.GetComponent<circleProps3>().SetSpawn(10000);
                }
                */
                i++;
            }
            if (cPositions.Count == 0)
            {
                break;
            }
        } while (i < numCircles);

    }

    IEnumerator GenerateCircles()
    {
        elapsedTime = 0;
        timeStart = DateTime.Now;

        while (counter != numCircles)
        {
            valid = true;
            foreach (GameObject c in circles)
            {
                /*
                CHECK CONDITIONS:
                growing is stopped if circle c
                1.) touches a boundary
                2.) touches another circle
                3.) location is inside another circle
                */

                /*
                ISSUES:
                1.) circles still overlap boundary
                3.) some circles overlap other circles
                */

                if (c.GetComponent<circleProps3>().doCheck)  //checkC is false if any of the check conditions have been met, no need to check them again
                {
                    //1.) test for boundary touch
                    if (c.GetComponent<circleProps3>().Edges()) //if true
                    {
                        counter++;
                        //Debug.Log("touched");
                        //c.GetComponent<circleProps3>().SetGrowing(false);
                        //c.GetComponent<circleProps3>().SetdoCheck(false);
                        continue;   //still need to check other circles for boundary touch
                    }
                }
                
                //perform checks on circle c against other circles
                foreach (GameObject otherC in circles)
                {
                    //don't perform checks against itself
                    if (c != otherC)
                    {
                        if (c.GetComponent<circleProps3>().doCheck)  //only do check condition evaluations on circles that need to be checked on next pass of the iteration loop
                        {
                            float d = Vector2.Distance(c.transform.position, otherC.transform.position);

                            //2.) test for circle touch
                            //if (d + 0.0001 < c.GetComponent<circleProps3>().r + otherC.GetComponent<circleProps3>().r)
                            //localScale.x / 2 is the current radius of the circle
                            if (d + 0.001 < c.GetComponent<SpriteRenderer>().transform.localScale.x / 2 + otherC.GetComponent<SpriteRenderer>().transform.localScale.x / 2 & otherC.GetComponent<circleProps3>().circleActive)
                            //if (d + 0.001 <= c.GetComponent<circleProps3>().r + otherC.GetComponent<circleProps3>().r & otherC.GetComponent<circleProps3>().circleActive)
                            {
                                c.GetComponent<circleProps3>().SetGrowing(false);
                                c.GetComponent<circleProps3>().SetdoCheck(false);
                                valid = false;
                                //Debug.Log("d(" + d + ")" + " <= c radius (" + c.GetComponent<circleProps3>().r + ") + " + "otherC radius (" + otherC.GetComponent<circleProps3>().r + ")" + " = " + c.GetComponent<circleProps3>().r + otherC.GetComponent<circleProps3>().r);
                                //otherC.GetComponent<addtext>().UpdateText(c.name);
                                //Debug.Log(c.name + " is touching " + otherC.name);
                                break;  //no need to check for touching on other circles
                            }

                            //3.) test for inside circle. Not needed. If otherC hasn't started growing by the time circle C has reached the center of otherC, then otherC is not activated for growth or for checking.
                            /*
                            if (d <= c.GetComponent<SpriteRenderer>().transform.localScale.x / 2)
                            {
                                otherC.GetComponent<circleProps3>().SetGrowing(false);
                                c.GetComponent<circleProps3>().SetdoCheck(false);
                                valid = false;
                                break;
                            }
                            */
                        }
                    }
                }
                
                if (!valid)
                {
                    counter++;
                    break;
                }

                deltaTime = DateTime.Now - timeStart;
                elapsedTime = (float)deltaTime.TotalMilliseconds;
                c.GetComponent<circleProps3>().Grow();
                yield return null;
            }
            //iteration++;
            //Debug.Log(counter);
        }
        Debug.Log("FINISHED");
    }


    public GameObject newCircle()
    {

        int ind = (Random.Range(0, cPositions.Count - 1));

        bool valid = true;

        if (valid)
        {
            GameObject go = Instantiate(circlePrefab, new Vector2(cPositions[ind].x, cPositions[ind].y), Quaternion.identity);
            cPositions.RemoveAt(ind);   //remove element that was just used so it is not used again
            return go;
        }
        else
        {
            return null;
        }
    }

}


