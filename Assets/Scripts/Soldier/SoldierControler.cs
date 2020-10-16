using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Attacker,
    Defender
}

public class SoldierControler : MonoBehaviour
{
    [HideInInspector]
    public float reactivateTime = 0;
    protected float curSpeed;
    protected Vector3 targetMove;
    [HideInInspector]
    public int index;

    [HideInInspector]
    public Team team;

    // Start is called before the first frame update
    void Start()
    {
        InitDefaultValue();
    }

    // Update is called once per frame
    void Update()
    {
        SoldierMove();
    }

    private void LateUpdate()
    {
        SetSoldierMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter : " + collision.gameObject.name);
        ProcessCollision(collision);
    }

    protected virtual void SoldierMove() { }
    protected virtual void InitDefaultValue() { }
    protected virtual void SetSoldierMaterial() { }
    protected virtual void ProcessCollision(Collision collision) { }
}