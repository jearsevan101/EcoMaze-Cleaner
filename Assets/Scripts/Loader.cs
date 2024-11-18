using UnityEngine.SceneManagement;
public static class Loader
{
    public enum Scene
    {
        MainMenu,
        MainGame,
        Tutorial
    }

    public static void Load(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}