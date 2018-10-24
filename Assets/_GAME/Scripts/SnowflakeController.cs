using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class SnowflakeController : MonoBehaviour
{

    private TapGestureRecognizer tapGesture;

    public List<Transform> spawnPositions;

    // Speed in units per sec.
    public float speed;

    public GameObject text;

    private Rigidbody2D rb;

    public float gravityScale;


    private string[] firstGroup = { "A", "E", "S", "T", "R", "N"};
    private string[] secondGroup = { "I", "O",  "K", "G", "D", "L"};
    private string[] thirdGroup = { "Å", "U", "V", "F","M" };
    private string[] fourthGroup = { "Ä", "Ö", "B", "F", "Y" };
    private string[] fifthGroup = { "J", "X", "Z", "C", "Q", "W" , "H"};



    public List<Color> snowFlakeColors;
    private int snowflakeColor;

    void Start()
    {
        MeshRenderer sortingLayer = text.GetComponent<MeshRenderer>();
        sortingLayer.sortingLayerName = "SnowflakeContent";


        StartSnowflakeFall();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        tapGesture = new TapGestureRecognizer();
        tapGesture.AllowSimultaneousExecutionWithAllGestures();
        tapGesture.StateUpdated += TapGesture_StateUpdated;
        FingersScript.Instance.AddGesture(tapGesture);
    }




    void TapGesture_StateUpdated(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 pos = new Vector2(gesture.FocusX, gesture.FocusY);
            pos = Camera.main.ScreenToWorldPoint(pos);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (snowflakeColor == GameManager.Instance.cloudColor)
                {
                    if (GameManager.Instance.collectedLetters.Count < GameManager.Instance.letterPositions.Count){
                    Transform newPos = GameManager.Instance.letterPositions[GameManager.Instance.collectedLetters.Count].transform;
                        Destroy(rb);
                        MoveToNextPosition(newPos);
                    }
                    else
                    {
                        Debug.Log("positionList full. SHOW TEXT IN GAME");
                    }
                }
                else
                {

                }
            }
        }
    }

    private void OnDestroy()
    {
        FingersScript.Instance.RemoveGesture(tapGesture);

        //tapGesture.StateUpdated -= TapGesture_StateUpdated;
    }

    private void StartSnowflakeFall()
    {
        RandomizeLetterOnSnowflake();
        RandomizeStartingPosition();
        GetComponent<SpriteRenderer>().color = RandomizeSnowflakeColor();
    }


    private Color RandomizeSnowflakeColor()
    {
        int random = Random.Range(0, snowFlakeColors.Count);
        snowflakeColor = random;

        return snowFlakeColors[random];
    }


    private void RandomizeStartingPosition()
    {
        transform.position = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
    }

    private void RandomizeLetterOnSnowflake()
    {
        TextMesh texture = text.GetComponent<TextMesh>();
        int random = Random.Range(0, GameManager.Instance.level);
        if (random == 0) {
            texture.text = firstGroup[Random.Range(0, firstGroup.Length)];
        }
        else if(random == 1){
            texture.text = secondGroup[Random.Range(0, secondGroup.Length)];
        }
        else if(random == 2){
            texture.text = thirdGroup[Random.Range(0, thirdGroup.Length)];
        }
        else if (random == 3) {
            texture.text = fourthGroup[Random.Range(0, fourthGroup.Length)];
        }
        else {
            texture.text = fifthGroup[Random.Range(0, fifthGroup.Length)];
        }
            
       
    }


    //private string getRandomLetter(bool isVowel, int randomNumber)
    //{
    //    if (isVowel)
    //    {
    //        switch (randomNumber)
    //        {
    //            case 0:
    //                return "A";
    //            case 1:
    //                return "E";
    //            case 2:
    //                return "I";
    //            case 3:
    //                return "O";
    //            case 4:
    //                return "U";
    //            case 5:
    //                return "Y";
    //            case 6:
    //                return "Å";
    //            case 7:
    //                return "Ä";
    //            case 8:
    //                return "Ö";
    //        }
    //    }
    //    else
    //    {
    //        switch (randomNumber)
    //        {
    //            case 0:
    //                return "B";
    //            case 1:
    //                return "C";
    //            case 2:
    //                return "D";
    //            case 3:
    //                return "F";
    //            case 4:
    //                return "G";
    //            case 5:
    //                return "H";
    //            case 6:
    //                return "J";
    //            case 7:
    //                return "K";
    //            case 8:
    //                return "L";
    //            case 9:
    //                return "M";
    //            case 10:
    //                return "N";
    //            case 11:
    //                return "O";
    //            case 12:
    //                return "P";
    //            case 13:
    //                return "Q";
    //            case 14:
    //                return "R";
    //            case 15:
    //                return "S";
    //            case 16:
    //                return "T";
    //            case 17:
    //                return "V";
    //            case 18:
    //                return "W";
    //            case 19:
    //                return "X";
    //            case 20:
    //                return "Z";
    //        }
    //    }

    //    return "";
    //}


    void MoveToNextPosition(Transform newPos)
    {
        LeanTween.move(gameObject, newPos, 0.5f).setEaseOutElastic().setOnComplete(() => {
            Destroy(gameObject);
            string letter = text.GetComponent<TextMesh>().text;
            GameManager.Instance.PlaceCollectedLetter(letter, newPos);
        });

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GroundCollider")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Snowflake")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);

        }
     
    }


}
