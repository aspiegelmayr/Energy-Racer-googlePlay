using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// manages the district selection scene
/// </summary>
public class DistrictSelection : MonoBehaviour
{
    public Text districtName;
    public static int curDistrict;
    public static int unlockedDistricts;
    //public GameObject button;
    public List<Button> allButtons;
    public PinchableScrollRect scrollRect;
    public District[] districts;
    public GameObject dialogCanvas;
    public Sprite[] coa;
    public Image coaImg;
    public Button backBtn;

    /// <summary>
    /// display district name, deactivate buttons for locked levels
    /// </summary>
    void Start()
    {
        if (Board.isOnlineMultiplayer)
        {
            backBtn.interactable = false;
        }

        dialogCanvas.SetActive(false);
        districts = DistrictArray.GetAllDistricts();
        curDistrict = 1;
    }

    public void SelectDistrictTag()
    {
        string tag = EventSystem.current.currentSelectedGameObject.tag;
        curDistrict = int.Parse(tag) - 1;
        districtName.text = districts[curDistrict].Name;
        coaImg.sprite = coa[curDistrict];
        LevelSelection.districtNum = curDistrict;
        Debug.Log(curDistrict);
        LevelSelection.districtName = EventSystem.current.currentSelectedGameObject.name;
    }
}
