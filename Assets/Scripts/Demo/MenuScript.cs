using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    protected int activeMenuIndex = -1;
    protected int previousMenuIndex = -1;
    public GameObject[] MenuScreens;
    public int StartingMenu = 0;

    private void Start()
    {
        foreach(GameObject menu in MenuScreens)
        {
            if(menu)
            {
                menu.SetActive(false);
            }
        }
        ChangeMenuTo(StartingMenu);
    }

    //MAIN MENU FUNCTIONALITY
    //
    //
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Close splash screen
    public void CloseSplash()
    {
        if(activeMenuIndex == 0)
        {
            ChangeMenuTo(1);
        }
    }

    //Navigate between menus.
    public void ChangeMenuTo(int newMenuIndex)
    {
        //Debug.Log("Previous:" + previousMenuIndex + ", Active:" + activeMenuIndex + ", New:" + newMenuIndex);
        if (newMenuIndex != activeMenuIndex)
        {
            if (0 <= activeMenuIndex && activeMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[activeMenuIndex])
                {
                    MenuScreens[activeMenuIndex].SetActive(false);
                }
            }
            if (0 <= newMenuIndex && newMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[newMenuIndex])
                {
                    MenuScreens[newMenuIndex].SetActive(true);
                }
            }

            previousMenuIndex = activeMenuIndex;
            activeMenuIndex = newMenuIndex;
        }
    }

    //Navigate to previous menu
    public void BackToPreviousMenu()
    {
        ChangeMenuTo(previousMenuIndex);
    }

    //Also used in pause menu
    public void QuitGame()
    {
        Application.Quit();
    }

    //
    //
    //PAUSE MENU FUNCTIONALITY
    //
    //
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
