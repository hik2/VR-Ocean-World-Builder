using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class spawnObjects : MonoBehaviour {

    public SteamVR_TrackedController trackedController;
    private ControllerGrabObject cgObject;

    //Fish
    public FlockManager flockA, flockB, flockC, flockD;
    public GameObject fishAPrefab, fishBPrefab, fishCPrefab, fishDPrefab;

    //Misc
    public GameObject conchShellA, seaShellB, bouncyBallC, stickD, waffleE, iceCreamF, pizzaG, watermelonH;

    //Rocks
    public GameObject rockA, rockB, rockC, rockD;

    //Object offsets
    public float fishCOffset, miscBOffset, miscDOffset;

    enum Selection {nothing, fishA, fishB, fishC, fishD, miscA, miscB, miscC, miscD, miscE, miscF, miscG, miscH, rockA, rockB, rockC, rockD };
    Selection selected = Selection.nothing;
    GameObject selectedObject;

    //Touchpad
    private Vector2 touchpad;
    private bool touchpadDown;

    private GameObject currentWaffle, currentIceCream, currentPizza, currentWatermelon;

    public void OnEnable()
    {
        trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.TriggerClicked += HandleTriggerClicked;
        trackedController.MenuButtonClicked += SelectNothing;
        trackedController.PadUnclicked += PadUnclicked;
        trackedController.PadClicked += PadClicked;
    }

    public void OnDisable()
    {
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.MenuButtonClicked -= SelectNothing;
        trackedController.PadUnclicked -= PadUnclicked;
        trackedController.PadClicked -= PadClicked;
        
    }

    public void SelectNothing(object sender, ClickedEventArgs e)
    {
        selected = Selection.nothing;
        selectedObject = null;
    }

    public void PadClicked(object sender, ClickedEventArgs e)
    {
        touchpadDown = true;

    }
    public void PadUnclicked(object sender, ClickedEventArgs e)
    {
        touchpadDown = false;
    }


    public void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (!cgObject.colliding)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                switch (selected)
                {
                    case Selection.nothing:

                        break;
                    case Selection.fishA:
                        flockA.SpawnFish(transform.position, transform.rotation);
                        break;
                    case Selection.fishB:
                        flockB.SpawnFish(transform.position, transform.rotation);
                        break;
                    case Selection.fishC:
                        flockC.SpawnFish(transform.position + transform.forward * fishCOffset, transform.rotation);
                        break;
                    case Selection.fishD:
                        flockD.SpawnFish(transform.position, transform.rotation);
                        break;
                    case Selection.miscA:
                        Instantiate(conchShellA, transform.position, transform.rotation);
                        break;
                    case Selection.miscB:                   
                        Instantiate(seaShellB, transform.position + transform.forward * miscBOffset, transform.rotation);
                        break;
                    case Selection.miscC:
                        Instantiate(bouncyBallC, transform.position, transform.rotation);
                        break;
                    case Selection.miscD:
                        Instantiate(stickD, transform.position + transform.forward * miscDOffset, transform.rotation);
                        break;
                    case Selection.miscE:
                        if(currentWaffle != null)
                        {
                            GameObject.Destroy(currentWaffle);
                        }
                        currentWaffle = Instantiate(waffleE, transform.position, transform.rotation);
                        flockA.fishBait = currentWaffle;
                        break;
                    case Selection.miscF:
                        if(currentIceCream != null)
                        {
                            GameObject.Destroy(currentIceCream);
                        }
                        currentIceCream = Instantiate(iceCreamF, transform.position, transform.rotation);
                        flockB.fishBait = currentIceCream;
                        break;
                    case Selection.miscG:
                        if (currentPizza != null)
                        {
                            GameObject.Destroy(currentPizza);
                        }
                        currentPizza = Instantiate(pizzaG, transform.position, transform.rotation);
                        flockC.fishBait = currentPizza;
                        break;
                    case Selection.miscH:
                        if (currentWatermelon != null)
                        {
                            GameObject.Destroy(currentWatermelon);
                        }
                        currentWatermelon = Instantiate(watermelonH, transform.position, transform.rotation);
                        flockD.fishBait = currentWatermelon;
                        break;
                    case Selection.rockA:
                        Instantiate(rockA, transform.position, transform.rotation);
                        break;
                    case Selection.rockB:
                        Instantiate(rockB, transform.position, transform.rotation);
                        break;
                    case Selection.rockC:
                        Instantiate(rockC, transform.position, transform.rotation);
                        break;
                    case Selection.rockD:
                        Instantiate(rockD, transform.position, transform.rotation);
                        break;
                }
            }
            else
            {
                string buttonName = EventSystem.current.currentSelectedGameObject.name;
                Image buttonObject = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();

                switch (buttonName)
                {
                    case "Button_Fish_Type_1":
                        selected = Selection.fishA;
                        selectedObject = fishAPrefab;
                        break;
                    case "Button_Fish_Type_2":
                        selected = Selection.fishB;
                        selectedObject = fishBPrefab;
                        break;
                    case "Button_Fish_Type_3":
                        selected = Selection.fishC;
                        selectedObject = fishCPrefab;
                        break;
                    case "Button_Fish_Type_4":
                        selected = Selection.fishD;
                        selectedObject = fishDPrefab;
                        break;
                    case "Button_Misc_Type_1":
                        selected = Selection.miscA;
                        selectedObject = conchShellA;
                        break;
                    case "Button_Misc_Type_2":
                        selected = Selection.miscB;
                        selectedObject = seaShellB;
                        break;
                    case "Button_Misc_Type_3":
                        selected = Selection.miscC;
                        selectedObject = bouncyBallC;
                        break;
                    case "Button_Misc_Type_4":
                        selected = Selection.miscD;
                        selectedObject = stickD;
                        break;
                    case "Button_Misc_Type_5":
                        selected = Selection.miscE;
                        selectedObject = waffleE;
                        break;
                    case "Button_Misc_Type_6":
                        selected = Selection.miscF;
                        selectedObject = iceCreamF;
                        break;
                    case "Button_Misc_Type_7":
                        selected = Selection.miscG;
                        selectedObject = pizzaG;
                        break;
                    case "Button_Misc_Type_8":
                        selected = Selection.miscH;
                        selectedObject = watermelonH;
                        break;
                    case "Button_Rock_Type_1":
                        selected = Selection.rockA;
                        selectedObject = rockA;
                        break;
                    case "Button_Rock_Type_2":
                        selected = Selection.rockB;
                        selectedObject = rockB;
                        break;
                    case "Button_Rock_Type_3":
                        selected = Selection.rockC;
                        selectedObject = rockC;
                        break;
                    case "Button_Rock_Type_4":
                        selected = Selection.rockD;
                        selectedObject = rockD;
                        break;
                }
            }
        }
    }



    // Use this for initialization
    void Start () {
        cgObject = GetComponent<ControllerGrabObject>();
    }
	
	// Update is called once per frame
	void Update () {
        //Read the touchpad values     
        if (touchpadDown && selectedObject != null)
        {
            float scaling = trackedController.controllerState.rAxis0.y * Time.deltaTime + 1;
            selectedObject.transform.localScale *= scaling;
            Rigidbody objectBody = selectedObject.transform.gameObject.GetComponent<Rigidbody>();
            if (objectBody != null)
            {
                objectBody.mass *= scaling;
            }
        }
    }

    private void OnApplicationQuit()
    {
        resetScale(fishAPrefab);
        resetScale(fishBPrefab);
        resetScale(fishCPrefab);
        resetScale(fishDPrefab);
        resetScale(conchShellA);
        resetScale(seaShellB);
        resetScale(bouncyBallC);
        resetScale(waffleE);
        resetScale(iceCreamF);
        resetScale(pizzaG);
        resetScale(watermelonH);
        resetScale(stickD);
        resetScale(rockA);
        resetScale(rockB);
        resetScale(rockC);
        resetScale(rockD);
    }

    private void resetScale(GameObject g)
    {
        g.transform.localScale = new Vector3(1, 1, 1);
    }
}
