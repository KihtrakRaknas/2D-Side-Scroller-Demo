using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraFollow : MonoBehaviour
{
    public GameObject player;
    float interpSpeed = .1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        float wiggle = 3;
        float playY = player.transform.position.y+2.5f;
        Vector3 targetPos = new Vector3(player.transform.position.x- wiggle, playY, transform.position.z);
        if(player.transform.position.x < this.transform.position.x)
            targetPos = new Vector3(player.transform.position.x + wiggle, playY, transform.position.z);
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x)> wiggle)
            transform.position = Vector3.Lerp(transform.position, targetPos, interpSpeed);
        else
            transform.position = new Vector3(transform.position.x , playY, transform.position.z);

    }
}