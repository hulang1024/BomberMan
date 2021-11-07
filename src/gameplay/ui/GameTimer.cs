using Godot;
using System;

public class GameTimer : Control
{
    public Action OnTimeout;

    private int totalSeconds = 102;
    private int time;

    private Timer timer;
    private TextSprite minutesTextSprite;
    private TextSprite secondsTextSprite;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, "OnTick");

        minutesTextSprite = GetNode<TextSprite>("MinutesTextSprite");
        secondsTextSprite = GetNode<TextSprite>("SecondsTextSprite");

        Start();
    }

    public void Start()
    {
        time = totalSeconds;
        UpdateText();
        timer.Start(1);
    }

    private void OnTick()
    {
        time--;
        if (time >= 0)
        {
            UpdateText();
        }
        else
        {
            OnTimeout?.Invoke();
        }
    }

    private void UpdateText()
    {
        minutesTextSprite.Text = ((int)time / 60).ToString();
        secondsTextSprite.Text = string.Format("{0:d2}", time % 60);
    }
}
