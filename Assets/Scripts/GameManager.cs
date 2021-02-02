using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public firebaseScript db;
    public InputField nickname;
    public Button robot, traditional;
    public Slider progress;
    public static float counter = 0f;


    string path = "https://aidanharringtonunity.firebaseio.com/";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
       
        Camera.main.gameObject.AddComponent<firebaseScript>();
        db = Camera.main.GetComponent<firebaseScript>();
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "GameMenu")
        {
            try
            {
                if (nickname.text == "")
                {
                    robot.GetComponent<Button>().interactable = false;
                    traditional.GetComponent<Button>().interactable = false;
                }

                else
                {
                    robot.GetComponent<Button>().interactable = true;
                    traditional.GetComponent<Button>().interactable = true;
                }
            }
            catch (Exception e)
            {

            }
            
        }
        if (scene.name == "Game")
        {
            progress = FindObjectOfType<Slider>();

            counter -= Time.deltaTime;

            if(counter < 5f)
            {
                try
                {
                    if (progress.value < 5)
                    {
                        progress.value += 0.2f * Time.deltaTime;
                    }
                    if (progress.value == progress.maxValue)
                    {
                        progress.gameObject.SetActive(false);
                    }
                }
                catch (Exception e)
                {

                }
                
            }
        }
    }

    public void LoadBlackScene()
    {
        StartCoroutine(initDB(path));
        SceneManager.LoadScene("Game");
        StartCoroutine(db.downloadAndSaveImage());
        StartCoroutine(getAllBlackPieces());
    }

    public void LoadGreenScene()
    {
        StartCoroutine(initDB(path));
        SceneManager.LoadScene("Game");
        StartCoroutine(db.downloadAndSaveImage());
        StartCoroutine(getAllGreenPieces());
    }


    public void onClickQuit()
    {
        Application.Quit();
    }

    IEnumerator clearDB()
    {
        yield return db.clearFirebase();
        Application.Quit();
    }

    IEnumerator initDB(string path)
    {
        yield return db.initFirebase(path);
    }

    IEnumerator getAllBlackPieces()
    {
        yield return db.downloadAndSaveBlackPieces("blackQueen", this);
        yield return db.downloadAndSaveBlackPieces("blackKnight", this);
        yield return db.downloadAndSaveBlackPieces("blackBishop", this);
        yield return db.downloadAndSaveBlackPieces("blackKing", this);
        yield return db.downloadAndSaveBlackPieces("blackRook", this);
        yield return db.downloadAndSaveBlackPieces("blackPawn", this);
        yield return db.downloadAndSaveBlackPieces("whiteQueen", this);
        yield return db.downloadAndSaveBlackPieces("whiteKnight", this);
        yield return db.downloadAndSaveBlackPieces("whiteBishop", this);
        yield return db.downloadAndSaveBlackPieces("whiteKing", this);
        yield return db.downloadAndSaveBlackPieces("whiteRook", this);
        yield return db.downloadAndSaveBlackPieces("whitePawn", this);
    }

    IEnumerator getAllGreenPieces()
    {
        yield return db.downloadAndSaveGreenPieces("blackQueen", this);
        yield return db.downloadAndSaveGreenPieces("blackKnight", this);
        yield return db.downloadAndSaveGreenPieces("blackBishop", this);
        yield return db.downloadAndSaveGreenPieces("blackKing", this);
        yield return db.downloadAndSaveGreenPieces("blackRook", this);
        yield return db.downloadAndSaveGreenPieces("blackPawn", this);
        yield return db.downloadAndSaveGreenPieces("whiteQueen", this);
        yield return db.downloadAndSaveGreenPieces("whiteKnight", this);
        yield return db.downloadAndSaveGreenPieces("whiteBishop", this);
        yield return db.downloadAndSaveGreenPieces("whiteKing", this);
        yield return db.downloadAndSaveGreenPieces("whiteRook", this);
        yield return db.downloadAndSaveGreenPieces("whitePawn", this);
    }
}






