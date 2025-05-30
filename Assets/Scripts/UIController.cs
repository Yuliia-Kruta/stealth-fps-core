using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Image visibilityEyeImage;


    void Start()
    {
        visibilityEyeImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();
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
}
