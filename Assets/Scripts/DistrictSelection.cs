using System.Collections.Generic;
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
    public GameObject button;
    public List<Button> allButtons;
    public PinchableScrollRect scrollRect;
    public District[] districts;

    /// <summary>
    /// display district name, deactivate buttons for locked levels
    /// </summary>
    void Start()
    {
        districts = DistrictArray.GetAllDistricts();
        curDistrict = 1;
    }

    public void SelectDistrictTag()
    {
        string tag = EventSystem.current.currentSelectedGameObject.tag;
        curDistrict = int.Parse(tag) - 1;
        districtName.text = DistrictArray.GetDistrict(curDistrict).Name;
        LevelSelection.districtNum = curDistrict;
        LevelSelection.districtName = EventSystem.current.currentSelectedGameObject.name;
    }
}

