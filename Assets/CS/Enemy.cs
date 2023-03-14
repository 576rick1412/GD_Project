using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharaInfo
{
    public override void Awake()
    {
        base.Awake();
        
        mask = LayerMask.GetMask("Player");
    }
    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }
}
