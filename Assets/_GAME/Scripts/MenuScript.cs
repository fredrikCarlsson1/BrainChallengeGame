using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    Animator animator;

    public AudioSource audioPlayer;
    public AudioClip jingleBells;
    public AudioClip starSound;
    public AudioClip startAppSound;

    private TapGestureRecognizer tapGesture;

    public GameObject menuBackground;

    public GameObject gameText;

    public GameObject scoreView;
    public List<GameObject> goldenStars;
    public GameObject backgroundStars;

    public GameObject playAgain;
    public GameObject playAgainText;
    public GameObject score;

    public GameObject santa;
    public GameObject deadSanta;
    int deadSantaHash = Animator.StringToHash("deadSanta");
    int walkingSantaHash = Animator.StringToHash("walkingSanta");
    int jumpingSantaHash = Animator.StringToHash("jumpingSanta");
    int backToIdle = Animator.StringToHash("backToIdle");

    public GameObject fadeSprite;


    // Use this for initialization
    void Start()
    {
        LeanTween.alpha(fadeSprite, 0, 0.2f);
        playAgainText.GetComponent<Renderer>().sortingLayerName = "TopMenuTextContent";
        SumScore.Add(SceneManagerScript.Points);

        tapGesture = new TapGestureRecognizer();
        tapGesture.AllowSimultaneousExecutionWithAllGestures();
        tapGesture.StateUpdated += TapGesture_StateUpdated;
        FingersScript.Instance.AddGesture(tapGesture);
        if (SceneManagerScript.GameOver)
        {
            GameOver();
        }
        else
        {
            StartingView();
        }


    }

    private void Awake()
    {


    }



    private IEnumerator ShowStars(int stars)
    {
        for (int i = 0; i < stars; i++)
        {
            
            Vector3 goldenStarSmallSize = goldenStars[i].transform.localScale;
            yield return new WaitForSeconds(1);
            LeanTween.alpha(goldenStars[i], 1, 0.1f);
            LeanTween.scale(goldenStars[i], new Vector3(goldenStarSmallSize.x + (goldenStarSmallSize.x * 0.1f), goldenStarSmallSize.y + (goldenStarSmallSize.y * 0.1f), 0), 0.5f).setEaseInCubic();
            audioPlayer.clip = starSound;
            audioPlayer.Play();
            //LeanTween.scale(goldenStars[i], new Vector3(goldenStarSmallSize.x - (goldenStarSmallSize.x * 0.1f), goldenStarSmallSize.y - (goldenStarSmallSize.y * 0.1f), 0), 0.5f);

            //goldenStars[i]
        }


    }



    public void GameOver()
    {
        
        animator = deadSanta.GetComponent<Animator>();
        deadSanta.SetActive(true);
        santa.SetActive(false);
        GameObject newNumberSnowflake = Instantiate(scoreView);
        gameText.SetActive(false);
        scoreView.SetActive(true);


        StartCoroutine(ShowStars(getNumberOfStars()));
        StartCoroutine(StartBackgroundSound());
        animator.SetTrigger(walkingSantaHash);
    }

    private IEnumerator StartBackgroundSound() {
        yield return new WaitForSeconds(5);
        audioPlayer.clip = jingleBells;
        audioPlayer.Play();
    }

    private int getNumberOfStars() {
        if(SceneManagerScript.Points > 500) {
            return 1;
        }
        else if (SceneManagerScript.Points > 1800) {
            return 2;
        }
        else if (SceneManagerScript.Points > 5000)
        {
            return 3;
        }
        else {
            return 0;
        }
    }


    private void StartingView()
    {
        audioPlayer.clip = startAppSound;
        audioPlayer.Play();
        animator = santa.GetComponent<Animator>();
        deadSanta.SetActive(false);
        santa.SetActive(true);
        backgroundStars.SetActive(false);
        scoreView.SetActive(false);
        gameText.SetActive(true);
        animator.SetTrigger(jumpingSantaHash);

    }




    void TapGesture_StateUpdated(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 pos = new Vector2(gesture.FocusX, gesture.FocusY);
            pos = Camera.main.ScreenToWorldPoint(pos);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == playAgain)
            {
                LeanTween.alpha(fadeSprite, 0.5f, 0.2f).setOnComplete(() => SceneManager.LoadScene("GameScene", LoadSceneMode.Single));

            }
        }
    }

}
