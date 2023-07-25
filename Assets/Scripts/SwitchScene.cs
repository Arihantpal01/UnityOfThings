using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
   public void PlayARCamera(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }
   public void BackToHome(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
   }

   public void GoToSettings(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
   }
   public void BackToHomeFromSettings(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
   }
    public void GoToEnergyUse(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }
    public void BackToSettings(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
   }
}
