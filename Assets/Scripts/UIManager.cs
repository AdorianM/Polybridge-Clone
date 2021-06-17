using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button RoadButton;
    public Button WoodButton;
    public Button StartButton;
    public Button RestartButton;
    public BarCreator barCreator;

    private void Start()
    {
        RoadButton.onClick.Invoke();
    }

    public void ChangeBar(int myBarType)
    {
        if(myBarType == 0)
        {
            WoodButton.GetComponent<Outline>().enabled = false;
            RoadButton.GetComponent<Outline>().enabled = true;
            barCreator.BarToInstantiate = barCreator.RoadBar;
        }
        if (myBarType == 1)
        {
            WoodButton.GetComponent<Outline>().enabled = true;
            RoadButton.GetComponent<Outline>().enabled = false;
            barCreator.BarToInstantiate = barCreator.WoodBar;
        }
    }

    public void Play()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
