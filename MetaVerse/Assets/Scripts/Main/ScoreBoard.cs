using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private GameObject scoreBoardUI;

    private void Awake()
    {
        scoreBoardUI.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            scoreBoardUI.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            scoreBoardUI.SetActive(false);
    }


}

