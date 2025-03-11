using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For accessing UI elements

public class Scen_2_scp : MonoBehaviour
{
    public void Player(){
        SceneManager.LoadSceneAsync(0);
    }

    public void fastPlayer(){
        SceneManager.LoadSceneAsync(2);
    }

}
