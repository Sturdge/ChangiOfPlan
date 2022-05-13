using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class menuButton : MonoBehaviour
{

    public Canvas[] menuScreens;

    public InputField[] resolution = new InputField[2]; //1314371

    public Toggle isFull; //1314371

    public Text invalidNotify; //1314371

    void Start()
    {
        ScreenSwitch(0);
        //resolution [0] = GameObject.Find("inputWidth"); //1314371
        //resolution [1] = GameObject.Find("inputHeight"); //1314371 

    }

    public void NextLevelButton(int index)			//loads the specified level when the button is clicked
    {
        Application.LoadLevel(index);
    }

    public void NextLevelButton(string levelName)	//loads the specified level when the button is clicked
    {
        Application.LoadLevel(levelName);
    }

    public void ScreenSwitch(int index)				//switches between the menu panels
    {
        foreach (Canvas screen in menuScreens)
        {
            screen.enabled = false;
        }
        menuScreens[index].enabled = true;
    }

    #region 1314371's code

    public void confirmOptions()
    {

        if (resolution[0].text != "" && resolution[1].text != "")
        {

            resolutionChange(int.Parse(resolution[0].text), int.Parse(resolution[1].text));

        }

    }

    void resolutionChange(int w, int h)
    {

        if (w >= 640 && h >= 480)
        {

            if (w <= 1920 && h <= 1080)
            {

                if (isFull.isOn)
                {

                    Screen.SetResolution(w, h, true);

                    invalidNotify.text = "";

                }
                else
                {

                    Screen.SetResolution(w, h, true);

                    invalidNotify.text = "";

                }

            }
            else
            {

                invalidNotify.text = "Invalid resolution!";

            }

        }
        else
        {

            invalidNotify.text = "Invalid resolution!";

        }

    }
    #endregion

    public void ExitGame()							//exits the game
    {
        Application.Quit();
    }
}
