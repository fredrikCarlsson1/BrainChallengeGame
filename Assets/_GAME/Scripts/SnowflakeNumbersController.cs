using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class SnowflakeNumbersController : MonoBehaviour
{
    private TapGestureRecognizer tapGesture;

    public List<Transform> spawnPositions;

    // Speed in units per sec.
    public float speed;

    public GameObject text;

    private Rigidbody2D rb;
    public float gravityScale;

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
                    if (GameManager.Instance.collectedNumbers.Count < GameManager.Instance.numberPositions.Count)
                    {
                        Transform newPos = GameManager.Instance.numberPositions[GameManager.Instance.collectedNumbers.Count].transform;
                        Destroy(rb);
                        MoveToNextPosition(newPos);
                    }

                    else
                    {
                        Debug.Log("positionList full");

                    }
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
        RandomizeNumberOnSnowflake();
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

    private void RandomizeNumberOnSnowflake()
    {
        TextMesh texture = text.GetComponent<TextMesh>();
        int random = Random.Range(0, 11);
        texture.text = random.ToString();
    }

    void MoveToNextPosition(Transform newPos)
    {
        LeanTween.move(gameObject, newPos, 0.5f).setEaseOutElastic().setOnComplete(() =>
        {
            Destroy(gameObject);
            string number = text.GetComponent<TextMesh>().text;
            GameManager.Instance.PlaceCollectedNumber(number, newPos);
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
