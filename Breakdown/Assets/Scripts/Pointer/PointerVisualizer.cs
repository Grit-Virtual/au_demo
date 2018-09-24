using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerVisualizer : MonoBehaviour {
    
    public static PointerVisualizer instance;

    public LineRenderer lineRenderer;
    public Renderer cursorRenderer;
    public LineRenderer originRenderer;

    public Transform leftController, rightController;
    Transform activeController;

    public Material[] materials;

    enum Pointers {
        INVALID,
        MENU,
        OBJECT
    }

    void Awake(){
        instance = this;
        #if UNITY_EDITOR
            gameObject.SetActive(false);
        #endif
        activeController = rightController;
        // foreach(string joystick in Input.GetJoystickNames()){
        //     if(joystick.EndsWith("Left")){
        //         activeController = leftController;
        //         break;
        //     }
        // }
    }

    void Update(){
        for(int i = 0; i < Input.GetJoystickNames().Length; i++){
            if(Input.GetJoystickNames()[i].EndsWith("Left") && activeController == rightController){
                activeController = leftController;
                break;
            } else if(Input.GetJoystickNames()[i].EndsWith("Right") && activeController == leftController){
				activeController = rightController;
				break;
			}
        }
    }

    public void SetPointer(RaycastHit hit) {
        originRenderer.SetPosition(0, activeController.position + activeController.forward * .012f);
        originRenderer.SetPosition(1, activeController.position + activeController.forward * .5f);
        lineRenderer.SetPosition(0, activeController.position + activeController.forward * .012f);
        if(hit.collider != null){
            lineRenderer.SetPosition(1, hit.point);
            cursorRenderer.transform.position = hit.point;
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("UI")){
                Controller.instance.ShowSelectPointer();
                DisplayMenuPointer();
            } else {
                if(Controller.instance.pointerMode == PointerMode.MOVE){
                    Controller.instance.ShowMovePointer();
                } else {
                    if(hit.collider.tag == "UID Object"){
                        DisplayObjectPointer();
                    } else {
                        DisplayInvalidPointer();
                    }
                }
            }
        } else {
            if(Controller.instance.pointerMode == PointerMode.MOVE){
                Controller.instance.ShowMovePointer();
            } else {
                lineRenderer.SetPosition(1, activeController.position + (activeController.forward * 10));
                cursorRenderer.transform.position = activeController.position + (activeController.forward * 10);
                DisplayInvalidPointer();
            }
        }
    }

    public void DisplayMenuPointer(){
        lineRenderer.sharedMaterial = materials[(int)Pointers.MENU];
        cursorRenderer.sharedMaterial = materials[(int)Pointers.MENU];
        cursorRenderer.transform.localScale = Vector3.one * 0.025f;
        originRenderer.sharedMaterial = materials[(int)Pointers.MENU];
    }

    public void DisplayObjectPointer(){
        lineRenderer.sharedMaterial = materials[(int)Pointers.OBJECT];
        cursorRenderer.sharedMaterial = materials[(int)Pointers.OBJECT];
        cursorRenderer.transform.localScale = Vector3.one * 0.05f;
        originRenderer.sharedMaterial = materials[(int)Pointers.OBJECT];
    }

    public void DisplayInvalidPointer(){
        lineRenderer.sharedMaterial = materials[(int)Pointers.INVALID];
        cursorRenderer.sharedMaterial = materials[(int)Pointers.INVALID];
        cursorRenderer.transform.localScale = Vector3.one * 0.05f;
        originRenderer.sharedMaterial = materials[(int)Pointers.INVALID];
    }

    public void SetActive(bool active){
        lineRenderer.enabled = active;
        cursorRenderer.enabled = active;
        originRenderer.enabled = active;
    }
}