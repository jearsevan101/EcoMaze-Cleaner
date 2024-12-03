using UnityEngine.SceneManagement;
public static class Loader
{
    public enum Scene
    {
        MainMenu,
        MainGame,
        Tutorial,
        Level0,
        Level1,
        Level2,
        Level3
    }

    public static void Load(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
    public static void LoadString(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
    public static void RetryGame()
    {
        // Get the active scene and reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}