using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public firebaseScript db;
    public InputField nickname;
    public Button robot, traditional;

    string path = "https://aidanharringtonunity.firebaseio.com/";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
       
        Camera.main.gameObject.AddComponent<firebaseScript>();
        db = Camera.main.GetComponent<firebaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "GameMenu")
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
        yield return db.downloadAndSaveBlackPieces("blackQueen");
        yield return db.downloadAndSaveBlackPieces("blackKnight");
        yield return db.downloadAndSaveBlackPieces("blackBishop");
        yield return db.downloadAndSaveBlackPieces("blackKing");
        yield return db.downloadAndSaveBlackPieces("blackRook");
        yield return db.downloadAndSaveBlackPieces("blackPawn");
        yield return db.downloadAndSaveBlackPieces("whiteQueen");
        yield return db.downloadAndSaveBlackPieces("whiteKnight");
        yield return db.downloadAndSaveBlackPieces("whiteBishop");
        yield return db.downloadAndSaveBlackPieces("whiteKing");
        yield return db.downloadAndSaveBlackPieces("whiteRook");
        yield return db.downloadAndSaveBlackPieces("whitePawn");
    }

    IEnumerator getAllGreenPieces()
    {
        yield return db.downloadAndSaveGreenPieces("blackQueen");
        yield return db.downloadAndSaveGreenPieces("blackKnight");
        yield return db.downloadAndSaveGreenPieces("blackBishop");
        yield return db.downloadAndSaveGreenPieces("blackKing");
        yield return db.downloadAndSaveGreenPieces("blackRook");
        yield return db.downloadAndSaveGreenPieces("blackPawn");
        yield return db.downloadAndSaveGreenPieces("whiteQueen");
        yield return db.downloadAndSaveGreenPieces("whiteKnight");
        yield return db.downloadAndSaveGreenPieces("whiteBishop");
        yield return db.downloadAndSaveGreenPieces("whiteKing");
        yield return db.downloadAndSaveGreenPieces("whiteRook");
        yield return db.downloadAndSaveGreenPieces("whitePawn");
    }
}






