using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_fast_walking : EnemyClass
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    

    void LateUpdate()
    {
        if (healthBar == null || !healthBar.gameObject.activeSelf || cam == null)
            return;

        // Make it face the camera
        healthBar.transform.LookAt(cam);

        // Flip so it's not backwards
        healthBar.transform.Rotate(0, 180f, 0);
    }




}
