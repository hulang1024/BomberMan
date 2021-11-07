using System;

public class ScoreProcessor
{
    private const int max_score = 100000000 - 1;

    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = Math.Min(value, max_score);
            ScoreTextSprite.Text = score.ToString();
        }
    }

    public TextSprite ScoreTextSprite;
}