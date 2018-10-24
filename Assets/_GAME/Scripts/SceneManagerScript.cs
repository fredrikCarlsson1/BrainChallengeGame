using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneManagerScript {
    private static int points, level;
    private static bool gameOver = false;

    public static bool GameOver 
    {
        get
        {
            return gameOver;
        }
        set
        {
            gameOver = value;
        } 
    }

    public static int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    public static int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }



}
