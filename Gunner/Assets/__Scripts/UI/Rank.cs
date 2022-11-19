using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rank
{
    public const string GeneralRank = "generalRank";
    public const string ScientistRank = "scientistRank";
    public const string ThiefRank = "thiefRank";
    public const string KnightRank = "knightRank";

    [Tooltip("Name should be: general, scientist or thief")]
    public static int GetRank(string name)
    {
        if (name == "general")
        {
            return PlayerPrefs.GetInt(GeneralRank, 0);
        }

        else if (name == "scientist")
        {
            return PlayerPrefs.GetInt(ScientistRank, 0);
        }

        else if (name == "thief")
        {
            return PlayerPrefs.GetInt(ThiefRank, 0);
        }

        else if (name == "knight")
        {
            return PlayerPrefs.GetInt(KnightRank, 0);
        }

        return 0;
    }

    [Tooltip("Name should be: general, scientist or thief")]
    public static void SetRank(string name, int rank)
    {
        if (name == "general")
        {
            PlayerPrefs.SetInt(GeneralRank, rank);
        }

        else if (name == "scientist")
        {
            PlayerPrefs.SetInt(ScientistRank, rank);
        }

        else if (name == "thief")
        {
            PlayerPrefs.SetInt(ThiefRank, rank);
        }

        else if (name == "knight")
        {
            PlayerPrefs.SetInt(ThiefRank, rank);
        }
    }
}
