using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Cinemachine;

// player probably doesnt need IDataPersistence anymore, look into if its still necessary for serializing
// replace any current serialization with dialogue controller's save system
public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 lastPlayerPosition;

    private RaycastHit hit;

    public NavMeshAgent agent;

    public Animator playerAnimator;

    //private InputManager inputManager = InputManager.Instance;
    [SerializeField] private IonaCamera ionaCamera; 

    public float speed; // character move speed
    public float heightOffset = 0.5f; // character height offset

    public float TouchSensitivity_x = 20f;
    public float TouchSensitivity_y = 20f;

    public float stopDistance = 1;
    public bool isMouseDown;

    public EditModeManager editModeManager; // to stop moving when in edit mode

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CinemachineCore.GetInputAxis = GetAxisCustom;

        ionaCamera.camera.transform.position = agent.transform.position + ionaCamera.cameraOffset;
        ionaCamera.cameraOffset = ionaCamera.transform.position - agent.transform.position;

        lastPlayerPosition = transform.position;

        ionaCamera.Initialize(transform);
        ionaCamera.FocusOnObject(transform);
        InputManager.onNumberKeyPressed += HandleNumberKeyPress;
    }

    void OnDestroy()
    {
        //prevent mem leaks
        InputManager.onNumberKeyPressed -= HandleNumberKeyPress;
    }

    void Update()
    { 
        //first check if topdown mode is enabled before doing any other updates. if it is, bail out while in top down mode
        if (editModeManager != null && editModeManager.IsEditModeActive()) 
        {
            if (agent.hasPath) agent.ResetPath();
            agent.velocity = Vector3.zero;
            return;
        }

        // must be set every frame, so stays in update
        // doing an if because of main menu not setting it
        if (InputManager.Instance != null)
        {
            isMouseDown = InputManager.Instance.IsMouseDown;
        }

        if(ionaCamera.recenterYAxis)
        {
            ionaCamera.FinishRecentering();
        }
        else
        {
            if (isMouseDown && !EventSystem.current.IsPointerOverGameObject())
            {
                    Ray ray = ionaCamera.camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        agent.SetDestination(hit.point);
                        agent.stoppingDistance = stopDistance;
                    }
            }

            // update camera position if player has moved
            if (HasPlayerMoved())
            {
                ionaCamera.camera.transform.position = agent.transform.position + ionaCamera.cameraOffset;
            }
        }
        UpdateAnimationState();
    }

    bool HasPlayerMoved()
    {
        if (Vector3.Distance(transform.position, lastPlayerPosition) > 0.01)
        {
            lastPlayerPosition = transform.position;
            return true;
        }

        return false;
    }

    private void UpdateAnimationState()
    {
        if(editModeManager != null && editModeManager.IsEditModeActive() ) 
        {
            playerAnimator.SetBool("IsWalking", false);
            return; 
        }
        if (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", true);
        }
    }

    public float GetAxisCustom(string axisName)
    {
        switch (axisName)
        {
            case "Mouse X":
                if (Input.touchCount > 0)
                {
                    //Is mobile touch
                    return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
                }
                else if (Input.GetMouseButton(1))
                {
                    // is mouse click
                    return Input.GetAxis("Mouse X");
                }
                break;
            case "Mouse Y":
                if (Input.touchCount > 0)
                {
                    // is mobile touch
                    return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
                }
                else if (Input.GetMouseButton(1))
                {
                    // is mouse click
                    return Input.GetAxis(axisName);
                }
                break;
            default:
                Debug.LogError("Input <" + axisName + "> not recognized.", this);
                break;
        }

        return 0f;
    }

    void HandleNumberKeyPress(int number)
    {
        if (number == 0)
        {
            // Trigger camera to return to its original position
            ionaCamera.SetRecenterCameraToPlayer();
        }

    }

    #region Data Persistence Methods

    #endregion

}
