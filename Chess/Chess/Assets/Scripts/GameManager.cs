using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public firebaseScript fbScript;
    string path = "https://aidanharringtonunity.firebaseio.com/";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Camera.main.gameObject.AddComponent<firebaseScript>();
        fbScript = Camera.main.GetComponent<firebaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadScene()
    {
        StartCoroutine(initDB(path));
        SceneManager.LoadScene("Game");
        StartCoroutine(fbScript.downloadAndSaveImage());
    }

    public void onClickQuit()
    {
        Application.Quit();
    }

    IEnumerator clearDB()
    {
        yield return fbScript.clearFirebase();
        Application.Quit();
    }

    IEnumerator initDB(string path)
    {
        yield return fbScript.initFirebase(path);
    }
}



