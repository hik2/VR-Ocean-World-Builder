using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class laserTrackedController : MonoBehaviour {

    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;
    private SteamVR_TrackedObject trackedObj;
    private Vector2 touchpad;
    private bool touchpadDown;
    enum Selection { nothing, fB1, fB2, fB3, fB4, mB1, mB2, mB3, mB4, mB5, mB6, mB7, mB8, rB1, rB2, rB3, rB4 };
    enum CategorySelection { nothing, fishCategory, miscCategory, rockCategory };
    Selection selected = Selection.nothing;
    CategorySelection categorySelected = CategorySelection.nothing;

    public GameObject fishCategoryButton, miscCategoryButton, rockCategoryButton;
    public GameObject fishButton1, fishButton2, fishButton3, fishButton4;
    public GameObject miscButton1, miscButton2, miscButton3, miscButton4, miscButton5, miscButton6, miscButton7, miscButton8;
    public GameObject rockButton1, rockButton2, rockButton3, rockButton4;
    public GameObject FishBlueTrans, FishYellowRedTrans, DolphinTrans, fishMelonTrans,
        conchShellTrans, seaShellTrans, bouncyBallTrans, stickTrans, waffleTrans, iceCreamTrans, pizzaTrans, watermelonTrans, 
        rockATrans, rockBTrans, rockCTrans, rockDTrans;

    public Movement movementScript;

    private GameObject transparentObject;


    // Use this for initialization
    void Start()
    {
        displayNothing();
    }

    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerOut += HandlePointerOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;
        trackedController.MenuButtonClicked += SelectNothing;

        trackedController.PadUnclicked += PadUnclicked;
        trackedController.PadClicked += PadClicked;
    }

    public void PadClicked(object sender, ClickedEventArgs e)
    {
        touchpadDown = true;
        
    }
    public void PadUnclicked(object sender, ClickedEventArgs e)
    {
        touchpadDown = false;
    }

    public void OnDisable()
    {
        trackedController.MenuButtonClicked -= SelectNothing;
        trackedController.PadUnclicked -= PadUnclicked;
        trackedController.PadClicked -= PadClicked;
    }

    public void SelectNothing(object sender, ClickedEventArgs e)
    {
        deselectButton();
        deselectCategoryButton();
        displayNothing();
        selected = Selection.nothing;
        movementScript.enabled = true;
        transparentObject = null;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            Image buttonObject = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();

            selectButton(buttonObject);
               
            if(buttonName == "Button_Fish" || buttonName == "Button_Misc" || buttonName == "Button_Rocks")
            {
                deselectCategoryButton();
            }
            else
            {
                deselectButton();
                movementScript.enabled = false;
            }
                switch (buttonName)
                {
                    case "Button_Fish":
                        categorySelected = CategorySelection.fishCategory;
                        displayFishButtons();
                        break;
                    case "Button_Misc":
                        categorySelected = CategorySelection.miscCategory;
                        displayPlantButtons();
                        break;
                    case "Button_Rocks":
                        categorySelected = CategorySelection.rockCategory;
                        displayRockButtons();
                        break;
                    case "Button_Fish_Type_1":
                        transparentObject = FishBlueTrans;
                        FishBlueTrans.SetActive(true);
                        selected = Selection.fB1;
                        break;
                    case "Button_Fish_Type_2":
                        transparentObject = FishYellowRedTrans;
                        FishYellowRedTrans.SetActive(true);
                        selected = Selection.fB2;
                        break;
                    case "Button_Fish_Type_3":
                        transparentObject = DolphinTrans;
                        DolphinTrans.SetActive(true);
                        selected = Selection.fB3;
                        break;
                    case "Button_Fish_Type_4":
                        transparentObject = fishMelonTrans;
                        fishMelonTrans.SetActive(true);
                        selected = Selection.fB4;
                        break;
                    case "Button_Misc_Type_1":
                        transparentObject = conchShellTrans;
                        conchShellTrans.SetActive(true);
                        selected = Selection.mB1;
                        break;
                    case "Button_Misc_Type_2":
                        transparentObject = seaShellTrans;
                        seaShellTrans.SetActive(true);
                        selected = Selection.mB2;
                        break;
                    case "Button_Misc_Type_3":
                        transparentObject = bouncyBallTrans;
                        bouncyBallTrans.SetActive(true);
                        selected = Selection.mB3;
                        break;
                    case "Button_Misc_Type_4":
                        transparentObject = stickTrans;
                        stickTrans.SetActive(true);
                        selected = Selection.mB4;
                        break;
                    case "Button_Misc_Type_5":
                        transparentObject = waffleTrans;
                        waffleTrans.SetActive(true);
                        selected = Selection.mB5;
                        break;
                    case "Button_Misc_Type_6":
                        transparentObject = iceCreamTrans;
                        iceCreamTrans.SetActive(true);
                        selected = Selection.mB6;
                        break;
                    case "Button_Misc_Type_7":
                        transparentObject = pizzaTrans;
                        pizzaTrans.SetActive(true);
                        selected = Selection.mB7;
                        break;
                    case "Button_Misc_Type_8":
                        transparentObject = watermelonTrans;
                        watermelonTrans.SetActive(true);
                        selected = Selection.mB8;
                        break;
                    case "Button_Rock_Type_1":
                        transparentObject = rockATrans;
                        rockATrans.SetActive(true);
                        selected = Selection.rB1;
                        break;
                    case "Button_Rock_Type_2":
                        transparentObject = rockBTrans;
                        rockBTrans.SetActive(true);
                        selected = Selection.rB2;
                        break;
                    case "Button_Rock_Type_3":
                        transparentObject = rockCTrans;
                        rockCTrans.SetActive(true);
                        selected = Selection.rB3;
                        break;
                    case "Button_Rock_Type_4":
                        transparentObject = rockDTrans;
                        rockDTrans.SetActive(true);
                        selected = Selection.rB4;
                        break;
                }
        }
    }

    private void selectButton(Image buttonObject)
    {
        //Permanently turns button green
        buttonObject.color = new Color32(0  , 255, 64, 255);
    }

    private void deselectCategoryButton()
    {
        GameObject prevCategoryButton = null;
        switch (categorySelected)
        {
            case CategorySelection.nothing:

                break;
            case CategorySelection.fishCategory:
                prevCategoryButton = fishCategoryButton;
                break;
            case CategorySelection.miscCategory:
                prevCategoryButton = miscCategoryButton;
                break;
            case CategorySelection.rockCategory:
                prevCategoryButton = rockCategoryButton;
                break;
        }
        if (prevCategoryButton != null)
        {
            prevCategoryButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    //Clears the prev button
    private void deselectButton()
    {
        GameObject prevButton = null;
        switch (selected)
        {
            case Selection.nothing:

                break;
            case Selection.fB1:
                prevButton = fishButton1;
                FishBlueTrans.SetActive(false);
                break;
            case Selection.fB2:
                prevButton = fishButton2;
                FishYellowRedTrans.SetActive(false);
                break;
            case Selection.fB3:
                prevButton = fishButton3;
                DolphinTrans.SetActive(false);
                break;
            case Selection.fB4:
                prevButton = fishButton4;
                fishMelonTrans.SetActive(false);
                break;
            case Selection.mB1:
                prevButton = miscButton1;
                conchShellTrans.SetActive(false);
                break;
            case Selection.mB2:
                prevButton = miscButton2;
                seaShellTrans.SetActive(false);
                break;
            case Selection.mB3:
                prevButton = miscButton3;
                bouncyBallTrans.SetActive(false);
                break;
            case Selection.mB4:
                prevButton = miscButton4;
                stickTrans.SetActive(false);
                break;
            case Selection.mB5:
                prevButton = miscButton5;
                waffleTrans.SetActive(false);
                break;
            case Selection.mB6:
                prevButton = miscButton6;
                iceCreamTrans.SetActive(false);
                break;
            case Selection.mB7:
                prevButton = miscButton7;
                pizzaTrans.SetActive(false);
                break;
            case Selection.mB8:
                prevButton = miscButton8;
                watermelonTrans.SetActive(false);
                break;
            case Selection.rB1:
                prevButton = rockButton1;
                rockATrans.SetActive(false);
                break;
            case Selection.rB2:
                rockBTrans.SetActive(false);
                prevButton = rockButton2;
                break;
            case Selection.rB3:
                rockCTrans.SetActive(false);
                prevButton = rockButton3;
                break;
            case Selection.rB4:
                rockDTrans.SetActive(false);
                prevButton = rockButton4;
                break;
        }
        if (prevButton != null)
        {
            prevButton.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.Select();
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void displayFishButtons()
    {
        fishButton1.SetActive(true);
        fishButton2.SetActive(true);
        fishButton3.SetActive(true);
        fishButton4.SetActive(true);

        miscButton1.SetActive(false);
        miscButton2.SetActive(false);
        miscButton3.SetActive(false);
        miscButton4.SetActive(false);
        miscButton5.SetActive(false);
        miscButton6.SetActive(false);
        miscButton7.SetActive(false);
        miscButton8.SetActive(false);

        rockButton1.SetActive(false);
        rockButton2.SetActive(false);
        rockButton3.SetActive(false);
        rockButton4.SetActive(false);
    }

    private void displayPlantButtons()
    {
        miscButton1.SetActive(true);
        miscButton2.SetActive(true);
        miscButton3.SetActive(true);
        miscButton4.SetActive(true);
        miscButton5.SetActive(true);
        miscButton6.SetActive(true);
        miscButton7.SetActive(true);
        miscButton8.SetActive(true);

        fishButton1.SetActive(false);
        fishButton2.SetActive(false);
        fishButton3.SetActive(false);
        fishButton4.SetActive(false);

        rockButton1.SetActive(false);
        rockButton2.SetActive(false);
        rockButton3.SetActive(false);
        rockButton4.SetActive(false);
    }

    private void displayRockButtons()
    {
        rockButton1.SetActive(true);
        rockButton2.SetActive(true);
        rockButton3.SetActive(true);
        rockButton4.SetActive(true);

        fishButton1.SetActive(false);
        fishButton2.SetActive(false);
        fishButton3.SetActive(false);
        fishButton4.SetActive(false);

        miscButton1.SetActive(false);
        miscButton2.SetActive(false);
        miscButton3.SetActive(false);
        miscButton4.SetActive(false);
        miscButton5.SetActive(false);
        miscButton6.SetActive(false);
        miscButton7.SetActive(false);
        miscButton8.SetActive(false);
    }

    private void displayNothing()
    {
        fishButton1.SetActive(false);
        fishButton2.SetActive(false);
        fishButton3.SetActive(false);
        fishButton4.SetActive(false);

        miscButton1.SetActive(false);
        miscButton2.SetActive(false);
        miscButton3.SetActive(false);
        miscButton4.SetActive(false);
        miscButton5.SetActive(false);
        miscButton6.SetActive(false);
        miscButton7.SetActive(false);
        miscButton8.SetActive(false);

        rockButton1.SetActive(false);
        rockButton2.SetActive(false);
        rockButton3.SetActive(false);
        rockButton4.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        //Read the touchpad values     
        if (touchpadDown && transparentObject != null)
        {
            float scaling = trackedController.controllerState.rAxis0.y * Time.deltaTime + 1;
            transparentObject.transform.localScale *= scaling;
        }
    }
}
