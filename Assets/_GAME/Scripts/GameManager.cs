using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DigitalRubyShared;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;
    }


    public float timer = 0.0f;

    public GameObject fadeSprite;

    public float gravityScale = 0.01f;
    public int level = 1;
    public float spawnDelay = 3;

    public TimerController timerController;

    public List<GameObject> clouds;
    public MathController mathController;
    float cloudTime = 0;
    float changeCloudColorDelay;
    public int cloudColor;
    public List<Color> cloudColors;

    public GameObject snowflakePrefab;
    public GameObject snowflakeNumberPrefab;
    public GameObject letterPrefab;
    public GameObject numberPrefab;

    public bool continueGame = true;

    public List<Transform> spawnPositions;


    //
    public List<GameObject> numberBoxes;
    public List<GameObject> letterBoxes;

    //LETTERS
    public GameObject firstLetter;
    public GameObject secondLetter;
    public GameObject thirdLetter;
    public GameObject fourthLetter;
    private string letter1;
    private string letter2;
    private string letter3;
    private string letter4;
    public int wordLength = 3;
    public List<GameObject> collectedLetters = new List<GameObject>();
    public List<Transform> letterPositions;
    public List<GameObject> placedLetters = new List<GameObject>();



    //NUMBERS
    public List<GameObject> collectedNumbers = new List<GameObject>();
    public List<Transform> numberPositions;
    public List<GameObject> placedNumbers = new List<GameObject>();
    public GameObject firstNumber;
    public GameObject secondNumber;
    public GameObject thirdNumber;
    private string number1;
    private string number2;
    private string number3;


    //SOUND

    public AudioClip addToBoxSound;
    public List<AudioClip> rightAnswerSounds;
    public List<AudioClip> wrongAnswerSounds;
    public AudioClip placeLetterSound;

    public AudioSource musicPlayer;
    public AudioSource longerAudio;
    public AudioClip removeSound;

    //POINTS
    public int points = 0;

    WordSplitClass wordSplitter = new WordSplitClass();
    private string[] threeWordArray;

    public GameObject santa;

    int runningSanta = Animator.StringToHash("runningSanta");
    int jumpingSanta = Animator.StringToHash("jumping");
    Animator animator;

    // Use this for initialization
    private void Start()
    {
        animator = santa.GetComponent<Animator>();
        LeanTween.alpha(fadeSprite, 0, 0.2f);
        threeWordArray = wordSplitter.splitAllWordsToArray();
        StartCoroutine(SnowflakeSpawner());
        ChangeCloudColor();
        cloudTime = 0;
        changeCloudColorDelay = Random.Range(3.0f, 5.0f);

    }

    public void GameOver()
    {
        
        SumScore.SaveHighScore();
        SceneManagerScript.Points = points;
        SceneManagerScript.GameOver = true;

        LeanTween.alpha(fadeSprite, 0.5f, 0.2f).setOnComplete(() => SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single));
    }

    private void SetPoints()
    {
        animator.SetTrigger(runningSanta);
        points += (level * 100);
        SumScore.Add(level*100);
        level += 1;
    }

    private void SubtractPoints()
    {
        points -= (level *50);
        SumScore.Subtract(level * 50);
    }

    int jumpingLevel = 0;

    private void Update()
    {
        timer += Time.deltaTime;
            if (timer > 15.0f)
            {
                timer += Time.deltaTime;
            jumpingLevel += 1;
            if (jumpingLevel % 2 == 0) {
                animator.SetTrigger(jumpingSanta);
            }
                
            
                if (spawnDelay > 1.3)
                {
                    spawnDelay -= 0.1f;
                }
                gravityScale += 0.001f;
                timer = 0;
            }

            cloudTime += Time.deltaTime;
            if (cloudTime >= changeCloudColorDelay)
            {
                ChangeCloudColor();
                cloudTime = 0;
            }



    }

    private void ChangeCloudColor()
    {
        int random = Random.Range(0, cloudColors.Count);
        cloudColor = random;
        foreach (GameObject cloud in clouds)
        {
            cloud.GetComponent<SpriteRenderer>().color = cloudColors[cloudColor];
        }
    }




    IEnumerator SnowflakeSpawner()
    {
        while (true)
        {
            bool letterSpawn = Random.value > 0.5;
            if (letterSpawn)
            {
                NewLetterSnowflake();
            }
            else
            {
                NewNumberSnowflake();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void NewNumberSnowflake()
    {
        GameObject newNumberSnowflake = Instantiate(snowflakeNumberPrefab);
        newNumberSnowflake.GetComponent<SnowflakeNumbersController>().spawnPositions = spawnPositions;
        newNumberSnowflake.GetComponent<SnowflakeNumbersController>().gravityScale = gravityScale;

    }

    void NewLetterSnowflake()
    {
        GameObject newSnowFlake = Instantiate(snowflakePrefab);
        newSnowFlake.GetComponent<SnowflakeController>().spawnPositions = spawnPositions;
        newSnowFlake.GetComponent<SnowflakeController>().gravityScale = gravityScale;

    }



    public void PlaceCollectedLetter(string letter, Transform letterPosition)
    {
        musicPlayer.clip = addToBoxSound;
        musicPlayer.Play();
        GameObject newLetter = Instantiate(letterPrefab);
        newLetter.GetComponent<LetterController>().letter = letter;
        newLetter.GetComponent<LetterController>().startingPosition = letterPosition;
        //newLetter.GetComponent<LetterController>().GameManager.Instance = this;

        collectedLetters.Add(newLetter);
    }


    public void PlaceCollectedNumber(string number, Transform numberPosition)
    {
        musicPlayer.clip = addToBoxSound;
        musicPlayer.Play();
        GameObject newNumber = Instantiate(numberPrefab);
        newNumber.GetComponent<NumberController>().number = number;
        newNumber.GetComponent<NumberController>().startingPosition = numberPosition;
        collectedNumbers.Add(newNumber);
    }

    public void RemoveCollectedLetter(GameObject letterObject)
    {

        StartCoroutine(OrderLetters(letterObject));
    }

    public void RemoveCollectedNumber(GameObject numberObject)
    {

        StartCoroutine(OrderNumbers(numberObject));
    }



    IEnumerator OrderNumbers(GameObject currentGameObject)
    {
        yield return null;
        if (collectedNumbers.Contains(currentGameObject))
        {
            collectedNumbers.Remove(currentGameObject);
        }
        if (placedNumbers.Contains(currentGameObject))
        {
            placedNumbers.Remove(currentGameObject);
        }

        yield return null;
        for (int i = 0; i < collectedNumbers.Count; i++)
        {
            if (collectedNumbers[i] == null)
            {
                collectedNumbers.RemoveAt(i);
            }
        }

        for (int i = 0; i < collectedNumbers.Count; i++)
        {
            Transform newPos = numberPositions[i];
            LeanTween.move(collectedNumbers[i], newPos, 0.2f);
            collectedNumbers[i].GetComponent<NumberController>().startingPosition = newPos;
        }
    }

    IEnumerator OrderLetters(GameObject currentGameObject)
    {
        yield return null;
        if (collectedLetters.Contains(currentGameObject))
        {
            collectedLetters.Remove(currentGameObject);
        }
        if (placedLetters.Contains(currentGameObject))
        {
            placedLetters.Remove(currentGameObject);
        }

        yield return null;
        for (int i = 0; i < collectedLetters.Count; i++)
        {
            if (collectedLetters[i] == null)
            {
                collectedLetters.RemoveAt(i);
            }
        }

        for (int i = 0; i < collectedLetters.Count; i++)
        {
            Transform newPos = letterPositions[i];
            LeanTween.move(collectedLetters[i], newPos, 0.2f);
            collectedLetters[i].GetComponent<LetterController>().startingPosition = newPos;
        }
    }




    public void AddLetterToBox(GameObject letterObject, string s, int i)
    {

        if (!placedLetters.Contains(letterObject))
        {
            placedLetters.Add(letterObject);
            musicPlayer.clip = placeLetterSound;
            musicPlayer.Play();
        }


        switch (i)
        {
            case 0:
                letter1 = s;
                firstLetter = letterObject;
                break;
            case 1:
                letter2 = s;
                secondLetter = letterObject;
                break;
            case 2:
                letter3 = s;
                thirdLetter = letterObject;
                break;
            case 3:
                letter4 = s;
                fourthLetter = letterObject;
                break;
            default:
                return;
        }

        checkIfWordIsOk();
    }


    private void checkIfWordIsOk()
    {
        if (letter1 != null && letter2 != null && letter3 != null)
        {
            string word = (letter1 + letter2 + letter3).ToLower();


            if (threeWordArray.Contains(word))
            {

                longerAudio.clip = rightAnswerSounds[Random.Range(0, rightAnswerSounds.Count)];
                longerAudio.Play();
                SetPoints();
                RemoveAllWordBoxes();
                timerController.RestartTimerAndReduceTime();

            }
            else
            {
                StartCoroutine(WrongLetterAnswer());
                longerAudio.clip = wrongAnswerSounds[Random.Range(0, wrongAnswerSounds.Count)];
                longerAudio.Play();
            }
        }
    }

    public void AddNumberToBox(GameObject numberObject, string s, int i)
    {
        if (!placedNumbers.Contains(numberObject))
        {
            placedNumbers.Add(numberObject);
            musicPlayer.clip = placeLetterSound;
            musicPlayer.Play();
        }

        switch (i)
        {
            case 0:
                number1 = s;
                firstNumber = numberObject;
                break;
            case 1:
                number2 = s;
                secondNumber = numberObject;
                break;
            case 2:
                number3 = s;
                thirdNumber = numberObject;
                break;
            default:
                return;
        }

        CheckIfSumIsOK();

    }



    public void PanLetterBackToSide(GameObject letterObject, int index)
    {

        if (placedLetters.Contains(letterObject))
        {
            placedLetters.Remove(letterObject);
        }
        collectedLetters.Add(letterObject);

        switch (index)
        {
            case 0:
                letter1 = null;
                firstLetter = null;
                break;
            case 1:
                letter2 = null;
                secondLetter = null;
                break;
            case 2:
                letter3 = null;
                thirdLetter = null;
                break;
            default:
                return;
        }
    }

    public void PanNumberBackToSide(GameObject numberObject, int index)
    {

        if (placedNumbers.Contains(numberObject))
        {
            placedNumbers.Remove(numberObject);
        }
        collectedNumbers.Add(numberObject);

        switch (index)
        {
            case 0:
                number1 = null;
                firstNumber = null;
                break;
            case 1:
                number2 = null;
                secondNumber = null;
                break;
            case 2:
                number3 = null;
                thirdNumber = null;
                break;
            default:
                return;
        }
    }


    public void MoveLetterToNewBox(GameObject letterBox, string letter, int newWordboxIndex, int oldWordboxIndex)
    {
        musicPlayer.clip = placeLetterSound;
        musicPlayer.Play();
        switch (oldWordboxIndex)
        {
            case 0:
                letter1 = null;
                firstLetter = null;
                break;
            case 1:
                letter2 = null;
                secondLetter = null;
                break;
            case 2:
                letter3 = null;
                thirdLetter = null;
                break;
            case 3:
                letter4 = null;
                fourthLetter = null;
                break;
            default:
                return;
        }

        switch (newWordboxIndex)
        {
            case 0:
                letter1 = letter;
                firstLetter = letterBox;
                break;
            case 1:
                letter2 = letter;
                secondLetter = letterBox;
                break;
            case 2:
                letter3 = letter;
                thirdLetter = letterBox;
                break;
            case 3:
                letter4 = letter;
                fourthLetter = letterBox;
                break;
            default:
                return;
        }
    }

    public void MoveNumberToNewBox(GameObject numberBox, string number, int newNumberBoxIndex, int oldNumberBoxIndex)
    {
        musicPlayer.clip = placeLetterSound;
        musicPlayer.Play();
        switch (oldNumberBoxIndex)
        {
            case 0:
                number1 = null;
                firstNumber = null;
                break;
            case 1:
                number2 = null;
                secondNumber = null;
                break;
            case 2:
                number3 = null;
                thirdNumber = null;
                break;
            default:
                return;
        }

        switch (newNumberBoxIndex)
        {
            case 0:
                number1 = number;
                firstNumber = numberBox;
                break;
            case 1:
                number2 = number;
                secondNumber = numberBox;
                break;
            case 2:
                number3 = number;
                thirdNumber = numberBox;
                break;
            default:
                return;
        }
    }



    public void RemoveAllWordBoxes()
    {

        for (int i = 0; i < wordLength; i++)
        {
            //yield return null;
            switch (i)
            {
                case 0:
                    letter1 = null;
                    firstLetter.GetComponent<LetterController>().selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
                    Destroy(firstLetter);
                    break;
                case 1:
                    letter2 = null;
                    secondLetter.GetComponent<LetterController>().selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
                    Destroy(secondLetter);
                    break;
                case 2:
                    letter3 = null;
                    thirdLetter.GetComponent<LetterController>().selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
                    Destroy(thirdLetter);
                    break;
                case 3:
                    letter4 = null;
                    fourthLetter.GetComponent<LetterController>().selectedWordBox.GetComponent<WordBoxesController>().wordBoxIsFull = false;
                    Destroy(fourthLetter);
                    break;
                default:
                    break;
            }

        }

    }

    public void RemoveAllNumberBoxes()
    {
        for (int i = 0; i < 3; i++)
        {
            //yield return null;
            switch (i)
            {
                case 0:
                    number1 = null;
                    firstNumber.GetComponent<NumberController>().selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;
                    Destroy(firstNumber);
                    break;
                case 1:
                    number2 = null;
                    secondNumber.GetComponent<NumberController>().selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;
                    Destroy(secondNumber);
                    break;
                case 2:
                    number3 = null;
                    thirdNumber.GetComponent<NumberController>().selectedNumberBox.GetComponent<NumberBoxesController>().numberBoxIsFull = false;
                    Destroy(thirdNumber);
                    break;
                default:
                    break;
            }

        }
    }

    private void CheckIfSumIsOK()
    {
        if (number1 != null && number2 != null && number3 != null)
        {
            int one = ConvertStringToInt(number1);
            int two = ConvertStringToInt(number2);
            int three = ConvertStringToInt(number3);
            if (mathController.CheckIfMathIsOk(one, two, three))
            {
                longerAudio.clip = rightAnswerSounds[Random.Range(0, rightAnswerSounds.Count)];
                longerAudio.Play();
                SetPoints();
                RemoveAllNumberBoxes();
                timerController.RestartTimerAndReduceTime();
            }
            else
            {
                StartCoroutine(WrongNumberAnswer());
                longerAudio.clip = wrongAnswerSounds[Random.Range(0, wrongAnswerSounds.Count)];
                longerAudio.Play();
            }
        }
    }

    IEnumerator WrongNumberAnswer()
    {
        SubtractPoints();
        foreach (GameObject numberBox in numberBoxes)
        {
            numberBox.GetComponent<SpriteRenderer>().color = Color.red;
        }
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject numberBox in numberBoxes)
        {
            numberBox.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    IEnumerator WrongLetterAnswer()
    {
        SubtractPoints();
        foreach (GameObject letterBox in letterBoxes)
        {
            letterBox.GetComponent<SpriteRenderer>().color = Color.red;
        }
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject letterBox in letterBoxes)
        {
            letterBox.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private int ConvertStringToInt(string intString)
    {
        int i = 0;
        if (!int.TryParse(intString, out i))
        {
            i = -1;
        }
        return i;
    }



}
