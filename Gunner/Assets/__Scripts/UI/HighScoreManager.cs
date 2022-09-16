using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class HighScoreManager : SingletonMonobehaviour<HighScoreManager>
{
    private HighScores highScores = new HighScores();
    private const string filePath = "/GunnerHighScores.dat";

    protected override void Awake()
    {
        base.Awake();
        LoadScores();
    }

    private void LoadScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + filePath))
        {
            ClearScoreList();

            FileStream file = File.OpenRead(Application.persistentDataPath + filePath);
            highScores = (HighScores)bf.Deserialize(file);

            file.Close();
        }
    }

    private void ClearScoreList()
    {
        highScores.scoreList.Clear();
    }

    public void AddScore(Score score, int rank)
    {
        highScores.scoreList.Insert(rank - 1, score);

        if (highScores.scoreList.Count > Settings.numberOfHighScoresToSava)
        {
            highScores.scoreList.RemoveAt(Settings.numberOfHighScoresToSava);
        }

        SaveScores();
    }

    private void SaveScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + filePath);
        bf.Serialize(file, highScores);

        file.Close();
    }

    public HighScores GetHighScores()
    {
        return highScores;
    }

    public int GetRank(long playerScore)
    {
        if (highScores.scoreList.Count == 0) return 1;

        int index = 0;

        for (int i = 0; i < highScores.scoreList.Count; i++)
        {
            index++;

            if (playerScore >= highScores.scoreList[i].playerScore)
            {
                return index;
            }
        }

        if (highScores.scoreList.Count < Settings.numberOfHighScoresToSava)
            return (index + 1);

        return 0;
    }
}
