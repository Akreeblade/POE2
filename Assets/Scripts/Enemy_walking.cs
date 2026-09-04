using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_walking : MonoBehaviour
{
    private int PathSpawnedOn;

    private List<Vector3> path;
    private int currentPoint = 0;
    private int maxHealth = 5;
    private int currentHealth = 5;
    [SerializeField] String Tagtofind;
    Transform[] waypointTransforms;
    private GameObject  gameManager;
    Moneyholdscript moneyholdscript;

    [Header("Health Bar")]
    public Slider healthBar;
    public float healthBarVisibleTime = 2f;
    private Coroutine hideBarRoutine;
    private Transform cam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        cam = Camera.main.transform;

        currentHealth = maxHealth;
        gameManager = GameObject.Find("GameManager");
        moneyholdscript = gameManager.GetComponent<Moneyholdscript>();
        
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            healthBar.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <=0)
        {
            Destroy(this.gameObject);
        }
        move();

    }

    public float GetLifeinfloat()
    {
        float lifePercentage = (float)currentHealth / (float)maxHealth;
        return lifePercentage;
    }

    public void SetPath(List<Vector3> newPath)
    {
        if (newPath == null)
        {
            Debug.LogError("Enemy_walking received a NULL path!");
            return;
        }

        if (newPath.Count == 0)
        {
            Debug.LogError("Enemy_walking received an EMPTY path!");
            return;
        }
        path = newPath;
        currentPoint = 0;

        Debug.Log("Path successfully assigned. Points: " + path.Count);
    }
    public void move()
    {
        if (path == null || path.Count == 0)
            return;

        if (currentPoint >= path.Count)
            return;

        Vector3 target = path[currentPoint];

        transform.position = Vector3.MoveTowards(transform.position,target,Time.deltaTime * 1f);

        Vector3 direction = target - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= path.Count)
            {
                Debug.Log("Enemy reached the end!");
                Destroy(gameObject);
            }
        }
    }

    public void takedamage(int damage)
    {
        
        ShowHealthBar();

        
        currentHealth -= damage;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
    void ShowHealthBar()
    {
        if (healthBar == null) return;

        healthBar.gameObject.SetActive(true);

        if (hideBarRoutine != null)
            StopCoroutine(hideBarRoutine);

        hideBarRoutine = StartCoroutine(HideHealthBar());
    }
    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(healthBarVisibleTime);
        healthBar.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (healthBar == null || !healthBar.gameObject.activeSelf || cam == null)
            return;

        // Make it face the camera
        healthBar.transform.LookAt(cam);

        // Flip so it's not backwards
        healthBar.transform.Rotate(0, 180f, 0);
    }

    public void getmoney()
    {
        moneyholdscript.addmoney(25);
    }

    private void OnDestroy()
    {
        getmoney();
    }

    //public void setpath(int path)
    //{
    //    PathSpawnedOn = path;
    //    //GetWaypoints();
    //}
    //public void GetWaypoints()
    //{
    //    
    //    waypoints = null;
    //    waypointTransforms = null;
    //     waypoints = GameObject.FindGameObjectsWithTag(Tagtofind+PathSpawnedOn);
    //     waypointTransforms = new Transform[waypoints.Length];
    //
    //    for (int i = 0; i < waypoints.Length; i++)
    //    {
    //        waypointTransforms[i] = waypoints[i].transform;
    //    }
    //}


}
