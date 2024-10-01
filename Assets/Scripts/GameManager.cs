

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;




public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public MenuManager endmenu;
  public TextMeshProUGUI scoretext;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Auskommentiert da es den Bug behebt dass wenn man stirbt der Endscreen nicht mehr
            //gezeigt werden kann
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndMenu(int score){
        //Debug.Log("ENDMENU TRIGGER");
        endmenu.gameObject.SetActive(true);
        scoretext.text = "Score: "+ score;
    }

}


