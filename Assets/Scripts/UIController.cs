using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Image visibilityEyeImage;

    private Image stickIcon;
    private Image stoneIcon;
    private Image grenadeIcon;

    // reference to the WeaponType enum
    private WeaponType weaponType;

    void Start()
    {
        visibilityEyeImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();

        // Retriving components from the children of the child "WeaponHUD"
        stickIcon = this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        stoneIcon = this.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        grenadeIcon = this.gameObject.transform.GetChild(1).GetChild(2).GetComponent<Image>();

    }


    void Update()
    {
        
    }


    // Update the visibility eye UI element
    public void updateVisibilityEye(string state)
    {
        switch (state)
        {
            case "shut":
                visibilityEyeImage.sprite = Resources.Load<Sprite>("eyeshut");
                break;
            case "open":
                visibilityEyeImage.sprite = Resources.Load<Sprite>("eyeopen");
                break;
            case "openred":
                visibilityEyeImage.sprite = Resources.Load<Sprite>("eyeopenred");
                break;
        }
    }

    // Update the weapon icon UI elements
    // To be called when the the player swaps an object
    // Waiting for the swap function and HUD script to be committed
    public void updateWeaponSelection()
    {

        // set Icons back to the default image
        stickIcon.sprite = Resources.Load<Sprite>("Stick");
        stoneIcon.sprite = Resources.Load<Sprite>("Stone");
        grenadeIcon.sprite = Resources.Load<Sprite>("Grenade");


        // Load weapon selected image based on Weapon Type
        switch (weaponType)
        {
            case WeaponType.stone:
                stoneIcon.sprite = Resources.Load<Sprite>("Selected_Stone");
                break;
            case WeaponType.stick:
                stickIcon.sprite = Resources.Load<Sprite>("Selected_Stick");
                break;
            case WeaponType.grenade:
                grenadeIcon.sprite = Resources.Load<Sprite>("Selected_Grenade");
                break;
        }
    }
}
