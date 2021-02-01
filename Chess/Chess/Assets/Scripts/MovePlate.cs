using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            if(cp.name == "whiteKing")
            {
                controller.GetComponent<Game>().Winner("black");
            } else
            {
                if (cp.name == "blackKing")
                {
                    controller.GetComponent<Game>().Winner("white");
                }
            }
            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessMan>().GetXBoard(), reference.GetComponent<ChessMan>().getYBoard());

        reference.GetComponent<ChessMan>().SetXBoard(matrixX);
        reference.GetComponent<ChessMan>().SetYBoard(matrixY);
        reference.GetComponent<ChessMan>().SetCoords();

        controller.GetComponent<Game>().setPosition(reference);

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<ChessMan>().DestroyMovePlates();
    }

    public void SetCoords (int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference (GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
