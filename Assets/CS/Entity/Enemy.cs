using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharaInfo
{
    protected override void Awake()
    {
        base.Awake();
        mask = LayerMask.GetMask("Player");


    }
    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        
    }
}
