using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Movement : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;

    private Vector3 hitPoint;
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    private bool shouldTeleport;
    private bool shouldSteer;
    private bool steering;
    public float steeringSpeed;
    public LayerMask steeringMask;
    public float sterringAvoidanceDistance;

    private SteamVR_LaserPointer laserPointer;

    private Vector2 touchpad;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        laserPointer = GetComponent < SteamVR_LaserPointer > ();
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    void Update()
    {
        if (!laserPointer.rockActive)
        {
            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                RaycastHit hit;

                if (!shouldSteer && Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
                {
                    hitPoint = hit.point;
                    reticle.SetActive(true);
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                    shouldTeleport = true;
                }
            }
            else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                shouldSteer = !shouldSteer;
            }
            else
            {
                reticle.SetActive(false);
            }
            if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
            {
                Teleport();
            }
            if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldSteer)
            {
                steering = false;
            }
            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && shouldSteer)
            {
                steering = true;
            }
            if (steering)
            {
                Steer();
            }
        }
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    private void Steer()
    {
        //Read the touchpad values
        touchpad = Controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        cameraRigTransform.position += Camera.main.transform.forward * touchpad.y * Time.deltaTime * steeringSpeed;
        
        //Fixes going through the floor issue, but steering feels bad
        //RaycastHit forwardMovementHit, backMovementHit;
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out forwardMovementHit, sterringAvoidanceDistance, steeringMask) ||
        //    Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.up, out backMovementHit, sterringAvoidanceDistance, steeringMask))
        //{
        //    cameraRigTransform.position -= Camera.main.transform.forward * touchpad.y * Time.deltaTime * steeringSpeed;
        //}
        //cameraRigTransform.position += Camera.main.transform.right * touchpad.x * Time.deltaTime * steeringSpeed;
    }
}
