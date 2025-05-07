using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : BaseController
{
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 0f, -10f);
    private Camera camera;
    [SerializeField] private StackZone stackZone;
    private IInteractable currentInteractable;
    
    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
    }
    protected override void HandleAction()
    {





    }
    private void LateUpdate()
    {
        if (camera != null)
        {
            Vector3 targetPos = transform.position+ cameraOffset;
            camera.transform.position = targetPos;
        }
    }

    public void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }
    public void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);
        if (lookDirection.magnitude < .9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }
    

    public void OnInteract()
    {
        if (stackZone != null && stackZone.isPlayerInRange)
        {
            stackZone.stackNpc?.Interact();
            Debug.Log("StackZone 안에서 F 입력됨");
        }

    }
    public void OnCancel()
    {
        if (stackZone != null && stackZone.isPlayerInRange)
        {
            stackZone.stackNpc?.Cancel();
            Debug.Log("StackZone 안에서 ESC 입력됨");
        }

    }
}
