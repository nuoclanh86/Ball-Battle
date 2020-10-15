using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierControler : MonoBehaviour
{
    protected float reactivateTime = 0;
    protected float curSpeed;
    protected Vector3 targetMove;
    protected int index;

    // Start is called before the first frame update
    void Start()
    {
        InitDefaultValue();
    }

    // Update is called once per frame
    void Update()
    {
        SoldierMove(this.gameObject);
    }

    private void LateUpdate()
    {
        SetSoldierMaterial(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter : " + collision.gameObject.name);
        ProcessCollision(collision, this.gameObject);
    }

    protected virtual void SoldierMove(GameObject sld) { }
    protected virtual void InitDefaultValue() { }
    protected virtual void SetSoldierMaterial(GameObject sld) { }
    protected virtual void ProcessCollision(Collision collision, GameObject sld) { }
}