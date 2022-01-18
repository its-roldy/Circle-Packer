using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class circleProps3 : MonoBehaviour
{
    public float r;
    public bool growing;
    public float correction = 0;
    public float spawnTime;
    public GameObject circlePrefab;
    public bool doCheck;
    public float rate;
    public bool circleActive;
    public bool start;
    
    public void Start()
    {
        spawnTime = Random.Range(0, 10000);    //set a random spawn delay in milliseconds
        doCheck = true;
        rate = 0.001f;
        growing = false;// true;
        r = 0;
        circleActive = false;
        start = false;
        //gameObject.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        //growing = false;
        //circlePrefab = Resources.Load("prefabs/Circle", typeof(GameObject)) as GameObject;
    }


    public void Update()
    {
        if (start)
        {
            if (cp3.elapsedTime > spawnTime)
            {
                if (growing)
                {
                    this.transform.localScale = Vector3.one * 2 * r;
                    r += rate;
                    circleActive = true;
                    gameObject.GetComponent<SpriteRenderer>().color = SetColor(r);
                }
            }
        }
    }

     public bool Edges()
    {
        //bool val = ((Mathf.Abs(this.transform.position.x) + r) < cp3.width / 2 || (Mathf.Abs(this.transform.position.y) + r) < cp3.height / 2 );
        //check to see if circle is touching bounds or over edge of bounds
        float cr = r;

        bool val = 
           this.transform.position.x + cr >  cp3.width  / 2 - correction ||
           this.transform.position.x - cr < -cp3.width  / 2 + correction ||
           this.transform.position.y + cr >  cp3.height / 2 - correction ||
           this.transform.position.y - cr < -cp3.height / 2 + correction;

        if (val)
        {
            //circleActive = false;
            growing = false;
            doCheck = false;
        }
        else
        {
            growing = true;
            doCheck = true;
        }

        return val;
    }

    public void SetGrowing(bool growingboolVal)
    {
        this.growing = growingboolVal;
    }

    /*
    public bool GetGrowing()
    {
        return this.growing;
    }
    */

    public void Grow()
    {
        start = true;
    }

    /*
    public void Show()
    {
        //if(this.gameObject.GetComponent<circleProps3>().circlePrefab == null)
        {
            //circlePrefab = Resources.Load("prefabs/Circle", typeof(GameObject)) as GameObject;
            //Instantiate(circlePrefab, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            //this.gameObject.name = "Circle" + cp3.count;
        }
    }
    */

    public void SetdoCheck(bool checkCVal)
    {
        this.doCheck = checkCVal;
    }

    public Color SetColor(float radius)
    {
        Color startColor = new Color(142, 78, 113);
        Color endColor = new Color(15, 77, 168);

        int num_steps = 50;

        float maxCircle = 2;
        float minCircle = 0;

        int r_num = Remap(radius, minCircle, maxCircle, 0, num_steps);
        
        var rAverage = startColor.r + (int)((endColor.r - startColor.r) * r_num / num_steps);
        var gAverage = startColor.g + (int)((endColor.g - startColor.g) * r_num / num_steps);
        var bAverage = startColor.b + (int)((endColor.b - startColor.b) * r_num / num_steps);

        return new Color(rAverage / 255, gAverage / 255, bAverage / 255);
    }

    public int Remap(float inputVal, float low1, float high1, float low2, float high2)
    {
        //remaps an input value from one range into another range
        //low1, high1 - minimum and maximum values that inputVal can have
        //low2, high2 - minimum and maximum values of the mapped range

        float val = low2 + (inputVal - low1) * (high2 - low2) / (high1 - low1);
        return (int) val;
    }

    /*
    public void SetSpawn(float spawnVal)
    {
        this.spawnTime = spawnVal;
    }
    */
}
