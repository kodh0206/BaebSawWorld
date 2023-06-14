using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public event Action StateUpdate;

    [SerializeField]
    int score;
    [SerializeField]
    int topScore;
    [SerializeField]
    //벽돌갯수
    int bricksCount;
    [SerializeField]
    BrickState [] bricks = new BrickState[0];
    [SerializeField]
    //초밥가격
    long[] presetPrices = new long[0];
    
    [SerializeField]
    int maxOpenLevel;
    [SerializeField]
    int prevLevelStats;
    [SerializeField]
    string rewardDateTime;
    [SerializeField]
    public double coin;

    int userLevels;
    int diamonds;
    public Int32 Score
    {
        get => score;
        set
        {
            score = value;

            if (score > topScore)
                topScore = score;

            StateUpdate?.Invoke();
        }
    }
    public double Coin{
        get => coin;
        set => coin = value;

    }

    public int Diamonds{
        get => diamonds;
        set => diamonds = value;

    }

    public int TopScore => topScore;

    public int BricksCount
    {
        get => bricksCount;
        set => bricksCount = value;
    }

    public int MaxOpenLevel
    {
        get => maxOpenLevel;
        set => maxOpenLevel = value;
    }

    public int UserLevels
    {
        get => userLevels;
        set => userLevels = value;
    }
    
    public void SetField(BrickState[] value)
    {
        bricks = (BrickState[]) value.Clone();
    }
    
    public BrickState[] GetField()
    {
        return (BrickState[]) bricks.Clone();
    }

    public void SetPresetsPrices(long[] value)
    {
        presetPrices = (long[])value.Clone();
    }

    public long[] GetPresetsPrices()
    {
        return presetPrices.Clone() as long[];
    }

    public void SetRewardTimer(DateTime time)
    {
        rewardDateTime = time.ToString (System.Globalization.CultureInfo.InvariantCulture);
    }

    public DateTime GetRewardTime()
    {
        DateTime dateTime;

        bool didParse = DateTime.TryParse (rewardDateTime, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);
        return didParse ? dateTime : DateTime.UtcNow;
    }

    public void ResetState()
    {   
        score = 0;
        bricks = new BrickState[0];
        bricksCount = 3;
        prevLevelStats = 0;
        MergeController.Instance.ResetMaxLevel();
        for (int i = 0; i < presetPrices.Length; i++)
        {
            presetPrices[i] = 0;
            }
        
        MergeController.Instance.ResetPresets();
        
    }
}

[Serializable]
public class BrickState
{
    [SerializeField]
    public int level;
    [SerializeField]
    public int open;
    [SerializeField]
    public int type;

    public BrickState(int value)
    {
        level = value;
        open = value;
        type = value;
    }
    
    public BrickState(int level, bool open, int type)
    {
        this.level = level;
        this.open = open ? 0 : 1;
        this.type = type;
    }
}
