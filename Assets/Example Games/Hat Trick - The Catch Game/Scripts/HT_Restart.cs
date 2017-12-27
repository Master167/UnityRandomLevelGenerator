using UnityEngine;
using System.Collections;

public class HT_Restart : MonoBehaviour {

	public void OnMouseDown () {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
