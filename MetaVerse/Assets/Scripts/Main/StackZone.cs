using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackZone : MonoBehaviour
{
    public StackNpc stackNpc;
    public bool isPlayerInRange = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            stackNpc?.ShowExplain();
        }
            
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            stackNpc?.HideExplain();
        }
            
    }
    
}
