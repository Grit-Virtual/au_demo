using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMenuStateManager : MonoBehaviour {

    public static ModeMenuStateManager instance;

    public GameObject moveSubMenu;
    public GameObject selectSubMenu;
    public GameObject filterSubMenu;
	
    public int currentMode = 0;

    void Awake(){
        instance = this;
    }

    void Start(){
        SwitchMode(0);
    }

    public void SwitchMode(int mode) {
        switch(mode) {
            case 0:
                moveSubMenu.SetActive(true);
                selectSubMenu.SetActive(false);
                filterSubMenu.SetActive(false);
				Controller.instance.SetPointerMode((PointerMode)0);
                currentMode = 0;
                break;
            case 1:
                moveSubMenu.SetActive(false);
                selectSubMenu.SetActive(true);
                filterSubMenu.SetActive(false);
				Controller.instance.SetPointerMode((PointerMode)1);
                currentMode = 1;
                break;
            case 2:
                moveSubMenu.SetActive(false);
                selectSubMenu.SetActive(false);
                filterSubMenu.SetActive(true);
				Controller.instance.SetPointerMode((PointerMode)1);
                currentMode = 2;
                break;
        }
    }
}
