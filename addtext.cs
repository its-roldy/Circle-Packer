using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class addtext : MonoBehaviour
{
    public string strtext;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] GameObject centerObj;

    public void Start()
    {
        //GameObject centObj = Instantiate(centerObj, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -1), Quaternion.identity);
    }


    public void UpdateText(string objtext)
    {
        objtext = gameObject.name + ", " + objtext;
        objtext = objtext.Replace("Circle", "");
        GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        prefab.transform.parent = gameObject.transform;
        prefab.GetComponentInChildren<TextMesh>().text = objtext;
    }
}
