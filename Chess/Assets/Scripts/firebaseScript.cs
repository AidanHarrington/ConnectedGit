using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using Firebase.Unity.Editor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;

public class firebaseScript : MonoBehaviour
{
    public bool signedin = false;
    DatabaseReference reference;

    //reference to the storage bucket
    FirebaseStorage storage;

    //main data dictionary
    Dictionary<string, object> myDataDictionary;

    FirebaseAuth auth;

    string email = "aidanh@gmail.com";
    string password = "User123";
    string path = "https://aidanharringtonunity.firebaseio.com/";

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public IEnumerator downloadAndSaveImage()
    {
        string pathToSaveIn = Application.persistentDataPath;
        string[] backgroundImages = new string[] {"gs://aidanharringtonunity.appspot.com/wood1.jpg",
            "gs://aidanharringtonunity.appspot.com/wood2.jpg",
            "gs://aidanharringtonunity.appspot.com/wood3.jpg",
            "gs://aidanharringtonunity.appspot.com/wood4.jpg" };
        string randomBackgroundImage = backgroundImages[UnityEngine.Random.Range(0, backgroundImages.Length)];

        storage = FirebaseStorage.DefaultInstance;

        // Create local filesystem URL

        string filename = Application.persistentDataPath + "/BackgroundImage.jpg";

        StorageReference storage_ref = storage.GetReferenceFromUrl(randomBackgroundImage);

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(filename,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {
              // called periodically during the download
              Debug.Log(String.Format(
                "Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred,
                state.TotalByteCount
              ));
          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
            }
        });

        Debug.Log(filename);

        yield return new WaitUntil(() => task.IsCompleted);

        Sprite backgroundImage = LoadSprite(filename);
        GameObject.Find("BackgroundImage").GetComponent<SpriteRenderer>().sprite = backgroundImage;

        yield return null;
    }

    public IEnumerator downloadAndSaveBlackPieces(string name)
    {
        string pathToSaveIn = Application.persistentDataPath;
        string chessPiece = "gs://aidanharringtonunity.appspot.com/black/" + name + ".png";

        storage = FirebaseStorage.DefaultInstance;

        // Create local filesystem URL

        string filename = Application.persistentDataPath;

        StorageReference storage_ref = storage.GetReferenceFromUrl(chessPiece);

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(filename,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {
              // called periodically during the download
              Debug.Log(String.Format(
                "Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred,
                state.TotalByteCount
              ));
          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
            }
        });

        Debug.Log(filename);

        yield return new WaitUntil(() => task.IsCompleted);

        Sprite chessPieceSprite = LoadSprite(filename);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag(name))
        {
            print(name);
            g.GetComponent<SpriteRenderer>().sprite = chessPieceSprite;
        }


        yield return null;
    }

    public IEnumerator downloadAndSaveGreenPieces(string name)
    {
        string pathToSaveIn = Application.persistentDataPath;
        string chessPiece = "gs://aidanharringtonunity.appspot.com/green/" + name + ".png";

        storage = FirebaseStorage.DefaultInstance;

        // Create local filesystem URL

        string filename = Application.persistentDataPath;

        StorageReference storage_ref = storage.GetReferenceFromUrl(chessPiece);

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(filename,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {
              // called periodically during the download
              Debug.Log(String.Format(
                "Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred,
                state.TotalByteCount
              ));
          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
            }
        });

        Debug.Log(filename);

        yield return new WaitUntil(() => task.IsCompleted);

        Sprite chessPieceSprite = LoadSprite(filename);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag(name))
        {
            print(name);
            g.GetComponent<SpriteRenderer>().sprite = chessPieceSprite;
        }

        yield return null;
    }

    public IEnumerator clearFirebase()
    {
        Task removeAllRecords = reference.RemoveValueAsync().ContinueWithOnMainThread(
            rmAllRecords =>
            {
                if (rmAllRecords.IsCompleted)
                {
                    Debug.Log("Database clear");
                }
            });

        yield return new WaitUntil(() => removeAllRecords.IsCompleted);

    }
    public IEnumerator initFirebase(string path)
    {
        if (!signedin)
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(path);
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            yield return signInToFirebase();
            Debug.Log("Firebase Initialized!");
            yield return true;
            signedin = true;
        }
        else
        {
            yield return null;
        }
    }
    IEnumerator signInToFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        //the outside task is a DIFFERENT NAME to the anonymous inner class
        Task signintask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
             signInTask =>
             {
                 if (signInTask.IsCanceled)
                 {
                     //write cancelled in the console
                     Debug.Log("Cancelled!");
                     return;
                 }
                 if (signInTask.IsFaulted)
                 {
                     //write the actual exception in the console
                     Debug.Log("Something went wrong!" + signInTask.Exception);
                     return;
                 }

                 Firebase.Auth.FirebaseUser loggedInUser = signInTask.Result;
                 Debug.Log("User " + loggedInUser.DisplayName + " has logged in!");
             }
            );
        yield return new WaitUntil(() => signintask.IsCompleted);

        Debug.Log("User has signed in");
    }
}
