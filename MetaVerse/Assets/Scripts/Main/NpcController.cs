using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NpcController : BaseController
{
    
    private Transform player;
    protected override void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            lookDirection = direction;
        }
        base.Update();
    }
    protected override void FixedUpdate()
    {
        movementDirection = Vector2.zero; // Npc는 안움직임
        base.FixedUpdate();
    }



    protected override void HandleAction()
    {
       
    }

  
}
