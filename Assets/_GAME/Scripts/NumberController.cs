using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class NumberController : MonoBehaviour
{


    private TapGestureRecognizer doubleTapGesture;
    private PanGestureRecognizer panGesture;
    private Vector3 offset;
    private bool dragging = false;

    public Transform startingPosition;
    public string number;

    public GameObject textObject;
    public GameObject selectedNumberBox;

    private bool numberIsPlaced = false;
    public bool sideIsMarkt = false;
    private int numberBoxIndex;

    public List<Transform> numberBoxesPositions;

    private GameObject currentGameObject;

    public AudioClip removeSound;

    // Use this for initialization
    void Start()
    {
        MeshRenderer sortingLayer = textObject.GetComponent<MeshRenderer>();
        sortingLayer.sortingLayerName = "CollectedSnowflakeContent";
        TextMesh texture = textObject.GetComponent<TextMesh>();
        texture.text = number;

        transform.position = startingPosition.position;

        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGesture_StateUpdated; ;
        doubleTapGesture.AllowSimultaneousExecutionWithAllGestures();
        FingersScript.Instance.AddGesture(doubleTapGesture);

        panGesture = new PanGestureRecognizer();
        panGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        panGesture.ThresholdUnits = 0.0f;
        panGesture.PlatformSpecificView = gameObject;
        panGesture.AllowSimultaneousExecutionWithAllGestures();
        panGesture.StateUpdated += PanGesture_StateUpdated;
        panGesture.ClearTrackedTouchesOnEndOrFail = true;
        FingersScript.Instance.AddGesture(panGesture);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NumberBoxLayer")
        {
            if (selectedNumberBox != null)
            {
                selectedNumberBox.GetComponent<SpriteRenderer>().color = Color.white;
                selectedNumberBox = null;

            }

            NumberBoxesController numberController = collision.GetComponent<NumberBoxesController>();
            bool selectedBoxIsFull = numberController.numberBoxIsFull;

            if (!selectedBoxIsFull)
            {
                selectedNumberBox = collision.gameObject;
                selectedNumberBox.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else if (collision.tag == "RightSide" && numberIsPlaced)
        {
            sideIsMarkt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ExitNumberLayer" && selectedNumberBox != null)
        {
            selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;
            selectedNumberBox.GetComponent<SpriteRenderer>().color = Color.white;
            selectedNumberBox = null;
        }
        else if (collision.tag == "RightSide" && numberIsPlaced)
        {
            sideIsMarkt = false;
        }

    }

    private void OnDestroy()
    {
        if (gameObject != null)
        {
            FingersScript.Instance.RemoveGesture(panGesture);
            FingersScript.Instance.RemoveGesture(doubleTapGesture);
            //doubleTapGesture.StateUpdated -= DoubleTapGesture_StateUpdated;
            //panGesture.StateUpdated -= PanGesture_StateUpdated;
            GameManager.Instance.RemoveCollectedNumber(gameObject);
            if (selectedNumberBox != null)
            {
                selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;

            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Snowflake")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);

        }
    }



    void DoubleTapGesture_StateUpdated(GestureRecognizer gesture)
    {

        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 pos = new Vector2(gesture.FocusX, gesture.FocusY);
            pos = Camera.main.ScreenToWorldPoint(pos);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject && gameObject != null)
            {
                GameManager.Instance.musicPlayer.clip = removeSound;

                GameManager.Instance.musicPlayer.Play();
                Destroy(gameObject);
            }
        }

    }


    void PanGesture_StateUpdated(GestureRecognizer gesture)
    {
        if (gameObject != null)
        {
            switch (gesture.State)
            {
                case GestureRecognizerState.Began:
                    Vector2 pos = new Vector2(gesture.FocusX, gesture.FocusY);

                    pos = Camera.main.ScreenToWorldPoint(pos);
                    offset = transform.position - (Vector3)pos;

                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        check();
                        currentGameObject = gameObject;
                        dragging = true;
                        if (selectedNumberBox != null)
                        {
                            selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;
                        }
                    }
                    break;
                case GestureRecognizerState.Executing:
                    if (dragging)
                    {
                        Vector2 newPos = new Vector2(gesture.FocusX, gesture.FocusY);
                        newPos = Camera.main.ScreenToWorldPoint(newPos);

                        transform.position = (Vector3)newPos + offset;

                    }
                    break;
                case GestureRecognizerState.Ended:
                    if (selectedNumberBox != null)
                    {
                        NumberBoxesController numberBoxesController = selectedNumberBox.GetComponent<NumberBoxesController>();
                        int newNumberBoxIndex = numberBoxesController.numberBoxIndex;
                        if (currentGameObject == gameObject)
                        {

                            if (numberIsPlaced)
                            {
                                GameManager.Instance.MoveNumberToNewBox(gameObject, number, newNumberBoxIndex, numberBoxIndex);
                            }
                            else
                            {
                                GameManager.Instance.RemoveCollectedNumber(gameObject);
                                GameManager.Instance.AddNumberToBox(gameObject, number, newNumberBoxIndex);
                                numberIsPlaced = true;
                            }
                            //GameObject kan bli null om senast adderade bokstaven gjorde så att ordet var rätt
                            if (gameObject!=null) {
                                selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = true;
                                transform.position = selectedNumberBox.transform.position;
                                startingPosition = selectedNumberBox.transform;
                                numberBoxIndex = newNumberBoxIndex;
                            }
                        }
                        currentGameObject = null;
                    }
                    else
                    {
                        if (sideIsMarkt && (GameManager.Instance.collectedNumbers.Count < GameManager.Instance.numberPositions.Count))
                        {
                            startingPosition = GameManager.Instance.numberPositions[GameManager.Instance.collectedNumbers.Count].transform;
                            LeanTween.move(gameObject, startingPosition, 0.1f);

                            GameManager.Instance.PanNumberBackToSide(gameObject, numberBoxIndex);
                            //LeanTween.scale(leftSide, new Vector3(originalLeftSideSize.x - 0.1f, originalLeftSideSize.y, 0), 0.1f);
                            sideIsMarkt = false;
                            numberIsPlaced = false;


                        }
                        else
                        {
                            LeanTween.move(gameObject, startingPosition.position, 0.1f);
                            if (selectedNumberBox != null)
                            {
                                selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = true;
                            }
                        }
                    }
                    dragging = false;

                    break;
            }
        }

    }

    private void check()
    {
        foreach (GameObject g in GameManager.Instance.numberBoxes)
        {
            if (g.transform.position == gameObject.transform.position && selectedNumberBox == null)
            {
                selectedNumberBox = g;
                selectedNumberBox.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

}
