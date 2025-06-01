using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private Image visibilityEyeImage;

    private Image stickIcon;
    private Image stoneIcon;
    private Image grenadeIcon;

    // Reference to the WeaponType enum
    //private WeaponType weaponType;

    public TMPro.TMP_Text stickCount;
    public TMPro.TMP_Text stoneCount;
    public TMPro.TMP_Text grenadeCount;

    void Start()
    {
        visibilityEyeImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();

        // Get components from the children of the child "WeaponHUD"
        stickIcon = this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        stoneIcon = this.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        grenadeIcon = this.gameObject.transform.GetChild(1).GetChild(2).GetComponent<Image>();

        // Find direct component, instead of getting them from the child of a child of another child
        stickCount = GameObject.Find("StickCount").GetComponent<TMP_Text>();
        stoneCount = GameObject.Find("StoneCount").GetComponent<TMP_Text>();
        grenadeCount = GameObject.Find("GrenadeCount").GetComponent<TMP_Text>();
    }

    // Update the visibility eye UI element
    public void UpdateVisibilityEye(string state)
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
    public void UpdateWeaponSelection(WeaponType type)
    {

        // Set Icons back to the greyed-out image
        stickIcon.sprite = Resources.Load<Sprite>("Stick");
        stoneIcon.sprite = Resources.Load<Sprite>("Stone");
        grenadeIcon.sprite = Resources.Load<Sprite>("Grenade");


        // Load 'Selected_Weapon' images based on the WeaponType
        switch (type)
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
            case WeaponType.None:
                Debug.Log("No selected weapon to highlight");
                break;
        }
    }
}
