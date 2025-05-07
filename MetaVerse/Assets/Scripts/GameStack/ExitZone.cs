using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("∏ﬁ¿Œ æ¿¿∏∑Œ ∫π±Õ");
            SceneManager.LoadScene("MainScene");
        }
    }
}
