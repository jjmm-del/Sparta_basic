using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StackNpc : NpcController, IInteractable
{
    [SerializeField] private GameObject ExplainUI;
    

    public void Interact()
    {
        SceneManager.LoadScene("GameStackScene");
    }
    public void Cancel()
    {
        
    }
    public void ShowExplain()
    {
        ExplainUI.SetActive(true);
    }
    public void HideExplain()
    {
        ExplainUI.SetActive(false);
    }
}
