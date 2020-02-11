using UnityEngine;
using UnityEngine.UI;

public static class SceneBackgroundInformation
{
    private static Camera camera = Camera.main;

    public static void SetBackground(int _difficulty)
    {
        if (_difficulty == 0)
        {
            camera.backgroundColor = Color.white;
        }
        if (_difficulty == 1)
        {
            Color32 greenColor = new Color32(182, 255, 184, 0);
            camera.backgroundColor = greenColor;
        }
        if (_difficulty == 2)
        {
            Color32 yellowColor = new Color32(255, 255, 182, 0);
            camera.backgroundColor = yellowColor;
        }
        if (_difficulty == 3)
        {
            Color32 blueColor = new Color32(110, 196, 226, 0);
            camera.backgroundColor = blueColor;
        }
        if (_difficulty == 4)
        {
            Color32 magetnaColor = new Color32(220, 182, 255, 0);
            camera.backgroundColor = magetnaColor;
        }
        if (_difficulty == 5)
        {
            Color32 redColor = new Color32(255, 182, 194, 0);
            camera.backgroundColor = redColor;
        }
        if (_difficulty == 6)
        {         
            Color32 grayColor = new Color32(116, 116, 111, 50);
            camera.backgroundColor = grayColor;
        }
    }
}
