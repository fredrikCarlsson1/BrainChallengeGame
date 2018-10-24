using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MathController : MonoBehaviour {
    public GameObject sumText;
    public GameObject firstSign;
    public GameObject secondSign;

    private int sum;
    private string s1;
    private string s2;

    public int level = 1;

    //private List<int> minusAndMultiplie = new List<int>();
    //private List<int> plusAndMultiplie = new List<int>();

    private int[] minusMulti = {0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 24, 28,
        30, 32, 35, 36, 40, 42, 48, 56 };
    
    private int[] plusMulti = {0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 24, 27, 
        28, 30, 32, 35, 36, 40, 42, 45, 48, 50, 54, 56, 60, 63, 64, 70, 72, 80, 84, 90, 96, 112, 120 };

    private int[] multiMulti = { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 24,
        25, 27, 28, 30, 32, 35, 36, 40, 42, 45, 48, 49, 50, 54, 56, 60, 63, 64, 70, 72, 75, 80,
        81, 84, 90, 96, 98, 100, 105, 108, 112, 120, 126, 128, 135, 140, 144, 147, 150, 160, 162,
        168, 175, 180, 189, 192, 196, 200, 210, 216, 224, 225, 240, 243, 245, 250, 252, 256, 270,
        280, 288, 294, 300, 315, 320, 324, 336, 350, 360, 378, 384, 392, 400, 405, 420, 432, 441,
        448, 450, 480, 486, 490, 500, 504, 540, 560, 567, 576, 600, 630, 640, 648, 700, 720, 800,
        810 };

    private void Start()
    {
        setUpMathObjects();
    }


    public void setUpMathObjects() {
        s1 = GetRandomSign();
        s2 = GetRandomSign();
        sum = GetRandomNumberForDiffrentSigns();
        sumText.GetComponent<Text>().text = "= " + sum;
        firstSign.GetComponent<Text>().text = s1;
        secondSign.GetComponent<Text>().text = s2;
    }

    private string GetRandomSign() {
        int randomNumber = Random.Range(0, level);

        switch(randomNumber) {
            case 0:
                return "+";
            case 1:
                return "-";
            case 2:
                return "x";
            default:
                return "0";
        }
    }

    private int switchNumberAndSign(int currentSum, int number, string sign) {
        switch (sign) {
            case "+":
                return currentSum+number;
            case "-":
                return currentSum-number;
            case "x":
                return currentSum*number;
            default:
                return 0;
        }
    }


    private int GetRandomNumberForDiffrentSigns() {
        if (s1 == "+" && s2 == "+")
        {
            return Random.Range(5, 22);
        }
        else if (s1 == "+" && s2 == "-")
        {
            return Random.Range(0, 14);
        }
        else if (s1 == "+" && s2 == "*")
        {
            int randomTop = plusMulti.Length;
            return plusMulti[Random.Range(0, randomTop)];
        }
        else if (s1 == "-" && s2 == "+")
        {
            return Random.Range(5, 15);
        }
        else if (s1 == "-" && s2 == "-")
        {
            return Random.Range(0, 6);
        }
        else if (s1 == "-" && s2 == "*")
        {
            int randomTop = minusMulti.Length;
            return minusMulti[Random.Range(0, randomTop)];
        }
        else if (s1 == "*" && s2 == "+")
        {
            return Random.Range(30, 80);
        }
        else if (s1 == "*" && s2 == "-")
        {
            return Random.Range(5, 50);
        }
        else if (s1 == "*" && s2 == "*")
        {
            int randomTop = multiMulti.Length;
            return multiMulti[Random.Range(0, randomTop)];
        }

        else
            return 20;
    }

   

    public bool CheckIfMathIsOk(int firstNumber, int secondNumber, int thirdNumber)
    {
        int newSum = switchNumberAndSign(firstNumber, secondNumber, s1);
        newSum = switchNumberAndSign(newSum, thirdNumber, s2);

        if (newSum == sum)
        {
            if (level < 3)
            {
                level++;
            }

            setUpMathObjects();
            return true;
        }
        else
            return false;

    }



}
