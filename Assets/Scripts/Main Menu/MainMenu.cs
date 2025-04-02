using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadSceneAsync("EntryScene");
    }
   public void QuitGame()
   {
    Application.Quit();
   }
   public void BathroomStage()
   {
    SceneManager.LoadSceneAsync("Bathroom_set(interior)");
   }
   public void RestaurantStage()
   {
    SceneManager.LoadSceneAsync("RestrauntScene");
   }
   // if aimed at, change appearance
    // if not, change back
   // if interacted with, activate
}
