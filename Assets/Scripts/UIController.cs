using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private Image visibilityEyeImage;

    // Set visual weapon selection
    private Image stickIcon;
    private Image stoneIcon;
    private Image grenadeIcon;
    
    [SerializeField] private GameObject visibilityEye;
    [SerializeField] private GameObject weaponHUD;

    // Set visual weapon count
    public TMPro.TMP_Text stickCount;
    public TMPro.TMP_Text stoneCount;
    public TMPro.TMP_Text grenadeCount;

    // Menu panels
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject finishPanel;

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

        // Set Icons back to thir greyed-out version
        stickIcon.sprite = Resources.Load<Sprite>("Stick");
        stoneIcon.sprite = Resources.Load<Sprite>("Stone");
        grenadeIcon.sprite = Resources.Load<Sprite>("Grenade");


        // Load 'Selected_Weapon' images based on the WeaponType
        switch (type)
        {
            case WeaponType.Stone:
                stoneIcon.sprite = Resources.Load<Sprite>("Selected_Stone");
                break;
            case WeaponType.Stick:
                stickIcon.sprite = Resources.Load<Sprite>("Selected_Stick");
                break;
            case WeaponType.Grenade:
                grenadeIcon.sprite = Resources.Load<Sprite>("Selected_Grenade");
                break;
            case WeaponType.None:
                Debug.Log("No selected weapon to highlight");
                break;
        }
    }
    
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        visibilityEye.SetActive(false);
        weaponHUD.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        visibilityEye.SetActive(true);
        weaponHUD.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        visibilityEye.SetActive(false);
        weaponHUD.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowWinPanel()
    {
        finishPanel.SetActive(true);
        visibilityEye.SetActive(false);
        weaponHUD.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Reloads the current scene to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // To continue game from Pause menu
    public void ContinueGame()
    {
        HidePausePanel(); 
    }

    // This can be updated to load other levels / scenes. Currently same as RestartGame()
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
