using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// manages the level selection scene
/// </summary>
public class LevelSelection : MonoBehaviour
{
    public static string districtName;
    public static int districtNum;

    public GameObject button;
    public Text districtText;
    public GameObject coatOfArms;
    public Text infoTxt;
    public static bool isMultiplayer;

    private string[] districtInfo;


    // Start is called before the first frame update
    void Start()
    {
        if (Board.isMultiplayer)
        {
            isMultiplayer = true;
        }
        if (districtName == null)
        {
            districtName = "Error";
        }
        districtText.text = districtName;

        ///rowName.text = "Postleitzahl \nFlaeche \nEinwohner \nHoehe \nPhotovoltaik pro Einwohner";
        districtInfo = new string[27]
        {
            "4240\n12,86km²\n7 960\n560m",
            "4230\n27,77km²\n5 422\n425m",
            "4224\n19,41km²\n4 276\n477m",
            "4212\n46,67km²\n3 163\n632m",
            "4284\n39,47km²\n3 099\n491m",
            "4280\n73,38km²\n3 091\n614m",
            "4261\n49,27km²\n2 989\n719m",
            "4283\n45,52km²\n2 913\n515m",
            "4271\n40,98km²\n2 906\n608m",
            "4291\n43,80km²\n2 796\n574m",
            "4232\n15,05km²\n2 751\n444m",
            "4293\n45,28km²\n2 672\n589m",
            "4273\n48,73km²\n2 174\n640m",
            "4210\n11,42km²\n2 161\n333m",
            "4292\n27,81km²\n2 138\n516m",
            "4274\n38,54km²\n1 949\n635m",
            "4264\n36,08km²\n1 924\n721m",
            "4252\n76,31km²\n1 585\n970m",
            "4263\n42,83km²\n1 567\n723m",
            "4251\n58,32km²\n1 413\n927m",
            "4294\n35,01km²\n1 388\n810m",
            "4240\n26,53km²\n1 382\n685m",
            "4242\n23,63km²\n1 202\n640m",
            "4272\n43,72km²\n1 047\n733m",
            "4282\n22,72km²\n1 016\n494m",
            "4262\n25,80km²\n1 015\n630m",
            "4273\n17,20km²\n6 22\n842m",
        };
        
       
        int index = districtNum;

        if (Board.isOnlineMultiplayer && Matchmaking.role == "guest")
        {
            index = Matchmaking.level;
        }

        infoTxt.text =  Math.Round(DistrictArray.DisrictArr[index].Latitude, 2) + "\n" +
        Math.Round(DistrictArray.DisrictArr[index].Longitude, 2) + "\n" + 
        Math.Round(DistrictArray.DisrictArr[index].Area, 2) + "km² \n" +
        DistrictArray.DisrictArr[index].Residents + "\n" +
        Math.Round(DistrictArray.DisrictArr[index].PVs);
    }

    public void SetCoatOfArms()
    {
        coatOfArms = GameObject.Find("CoatOfArms"); 
    }
}
