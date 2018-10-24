using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class LetterController : MonoBehaviour
{

    private PanGestureRecognizer panGesture;
    private TapGestureRecognizer doubleTapGesture;
    private Vector3 offset;
    private bool dragging = false;

    public Transform startingPosition;
    public string letter;

    public GameObject textObject;

    Collider2D letterCollider;

    public GameObject selectedWordBox;
    private GameObject previousWordBox;
    private bool letterIsPlaced = false;
    public bool sideIsMarkt = false;
    private int wordBoxIndex = -1;
    private GameObject currentGameObject;

   // public List<Transform> letterBoxesPositions;

    public AudioClip removeSound;


    // Use this for initialization
    void Start()
    {
        letterCollider = gameObject.GetComponent<BoxCollider2D>();
        MeshRenderer sortingLayer = textObject.GetComponent<MeshRenderer>();
        sortingLayer.sortingLayerName = "CollectedSnowflakeContent";
        TextMesh texture = textObject.GetComponent<TextMesh>();
        texture.text = letter;


        transform.position = startingPosition.position;

        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGesture_StateUpdated; ;
        doubleTapGesture.AllowSimultaneousExecutionWithAllGestures();
        FingersScript.Instance.AddGesture(doubleTapGesture);

        panGesture = new PanGestureRecognizer();
        panGesture.ThresholdUnits = 0.0f;
        //panGesture.PlatformSpecificView = gameObject;
        panGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        panGesture.AllowSimultaneousExecutionWithAllGestures();
        panGesture.StateUpdated += PanGesture_StateUpdated;
        // panGesture.ClearTrackedTouchesOnEndOrFail = true;
        FingersScript.Instance.AddGesture(panGesture);

    }

    private void OnDestroy()
    {
        if (gameObject != null)
        {
            FingersScript.Instance.RemoveGesture(panGesture);
            FingersScript.Instance.RemoveGesture(doubleTapGesture);
            //doubleTapGesture.StateUpdated -= DoubleTapGesture_StateUpdated;
            //panGesture.StateUpdated -= PanGesture_StateUpdated;
            GameManager.Instance.RemoveCollectedLetter(gameObject);
            if (selectedWordBox != null)
            {
                selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;

            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "LetterBoxLayer")
        {



            if (selectedWordBox != null)
            {
                selectedWordBox.GetComponent<SpriteRenderer>().color = Color.white;
                previousWordBox = selectedWordBox;
                selectedWordBox = null;

            }

            WordBoxesController wordBoxesController = collision.GetComponent<WordBoxesController>();
            bool selectedBoxIsFull = wordBoxesController.wordBoxIsFull;

            if (!selectedBoxIsFull)
            {
                selectedWordBox = collision.gameObject;
                selectedWordBox.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else if (collision.tag == "LeftSide" && letterIsPlaced)
        {

            // MAKE A FUNCTION THAT REMOVES LETTEROBJECT FROM PLACED LETTERS AND ADDS TO COLLECTED LETTERS IF COLLECTED LETTERS IS NOT FULL
            sideIsMarkt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "ExitLetterLayer" && selectedWordBox != null)
        {
           //selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
            selectedWordBox.GetComponent<SpriteRenderer>().color = Color.white;
            previousWordBox = selectedWordBox;
            selectedWordBox = null;
        }
        else if (collision.tag == "LeftSide" && letterIsPlaced)
        {
            sideIsMarkt = false;
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Snowflake")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);

        }
    }







    //REMOVE LETTER
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
                        if (selectedWordBox != null)
                        {
                            selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
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
                    if (selectedWordBox != null)
                    {
                        selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = true;
                        WordBoxesController wordBoxesController = selectedWordBox.GetComponent<WordBoxesController>();
                        int newWordBoxIndex = selectedWordBox.GetComponent<WordBoxesController>().wordBoxIndex;
                        if (currentGameObject == gameObject)
                        {
                            if (letterIsPlaced)
                            {
                                GameManager.Instance.MoveLetterToNewBox(gameObject, letter, newWordBoxIndex, wordBoxIndex);
                            }
                            else
                            {
                                GameManager.Instance.RemoveCollectedLetter(gameObject);
                                GameManager.Instance.AddLetterToBox(gameObject, letter, newWordBoxIndex);

                            }
                            //GameObject kan bli null om senast adderade bokstaven gjorde så att ordet var rätt
                            if (gameObject != null && selectedWordBox != null)
                            {
                                transform.position = selectedWordBox.transform.position;
                                startingPosition = selectedWordBox.transform;
                                wordBoxIndex = newWordBoxIndex;
                            }
                             letterIsPlaced = true;
                        }
                        currentGameObject = null;
                    }
                    else
                    {
                        //Slide back to side
                        if (sideIsMarkt && (GameManager.Instance.collectedLetters.Count < GameManager.Instance.letterPositions.Count))
                        {
                            startingPosition = GameManager.Instance.letterPositions[GameManager.Instance.collectedLetters.Count].transform;
                            LeanTween.move(gameObject, startingPosition, 0.1f);

                            GameManager.Instance.PanLetterBackToSide(gameObject, wordBoxIndex);
                            sideIsMarkt = false;
                            letterIsPlaced = false;
                        }
                        else
                        {
                            //Slide back to previous position
                            LeanTween.move(gameObject, startingPosition.position, 0.2f);
                        }
                    }
                    dragging = false;

                    break;
            }
        }

    }

    private void check() {
        foreach (GameObject g in GameManager.Instance.letterBoxes){
            if (g.transform.position == gameObject.transform.position && selectedWordBox == null) {
                selectedWordBox = g;
                selectedWordBox.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }


}
