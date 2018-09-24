using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Restifizer;
using ControllerSelection;

public enum PointerMode {
    MOVE,
    SELECT
}

public class Controller : MonoBehaviour {

	public static Controller instance;

    public Transform cameraRig;
	public Transform cameraTransform;
    public Transform trackingSpaceTransform;
    
	public GameObject movePointer;
	public PointerVisualizer pointerVisualizer;
    public Transform menuAnchor;

    public PointerMode pointerMode;

    Transform groundTransform;
    Quaternion targetRotation;

    bool switchInput, recenterPressed;

    public MainMenuButton moveButton, selectButton;

    public float elevationAmount;

    void Awake () {
		instance = this;
        GameObject ground = GameObject.FindGameObjectWithTag ("Ground");
        if(ground != null){
    		groundTransform = ground.transform;
        }
	}

	void Update () {
        if(!HMDManager.paused){
    		HandleInput();
            RecenterMenus ();
        }
    }

	void HandleInput(){
        if(moveButton != null && selectButton != null){
            if(OVRInput.GetDown (OVRInput.Button.PrimaryTouchpad)){
                if(ModeMenuStateManager.instance.currentMode == 0){
                    ModeMenuStateManager.instance.SwitchMode(1);
                    selectButton.Click();
                } else {
                    ModeMenuStateManager.instance.SwitchMode(0);
                    moveButton.Click();
                }
            }
        }
        recenterPressed = OVRInput.GetControllerWasRecentered();
        #if UNITY_EDITOR
            recenterPressed = Input.GetKeyDown(KeyCode.Return);
        #endif
	}

    void RecenterMenus() {
        if (recenterPressed) {
            recenterPressed = false;
            Quaternion tsRotation = Quaternion.Euler(new Vector3(0, trackingSpaceTransform.rotation.eulerAngles.y, 0));
            targetRotation = Quaternion.Euler(new Vector3(0, cameraTransform.rotation.eulerAngles.y - trackingSpaceTransform.rotation.eulerAngles.y, 0));
            menuAnchor.localRotation = targetRotation;
        }
    }

    public void Elevate(bool up) {
        Scaffolding.instance.HideScaffolding();
        if (up) {
            cameraRig.Translate(Vector3.up * elevationAmount);
        } else {
            cameraRig.Translate(Vector3.down * elevationAmount);
        }
    }

    public void ResetElevation(){
        RaycastHit hit;
        if(Physics.Raycast(cameraRig.position, cameraRig.up * -1f, out hit)){
            cameraRig.position = new Vector3(cameraRig.position.x, hit.point.y + 1.66f, cameraRig.position.z);
        } else {
            cameraRig.position = new Vector3(cameraRig.position.x, groundTransform.position.y + 1.66f, cameraRig.position.z);
        }
    }

    public void Rotate(bool left) {
        if (left) {
            cameraRig.rotation *= Quaternion.Euler(new Vector3(0f, -45f, 0f));
        } else {
            cameraRig.rotation *= Quaternion.Euler(new Vector3(0f, 45f, 0f));
        }
    }

	public void SetPointerMode(PointerMode mode) {
        pointerMode = mode;
        switch (mode) {
            case PointerMode.MOVE:
                MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
                break;
            case PointerMode.SELECT:
                ShowSelectPointer();
                break;
        }
    }

    public void ShowMovePointer(){
		pointerVisualizer.SetActive(false);
        movePointer.SetActive(true);
    }

    public void ShowSelectPointer(){
        movePointer.SetActive(false);
        pointerVisualizer.SetActive(true);
    }

    public bool BackButtonPress(){
        #if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.Escape);
        # else
            return OVRInput.GetDown (OVRInput.Button.Two);
        #endif
    }
}
