 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainGame);
        });
        tutorialButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Tutorial);
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
