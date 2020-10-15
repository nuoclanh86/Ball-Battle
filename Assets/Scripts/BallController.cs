using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideTheBall()
    {
        this.gameObject.transform.position = new Vector3(99.0f, 99.0f, 99.0f); //move it out of the screen
    }
}
