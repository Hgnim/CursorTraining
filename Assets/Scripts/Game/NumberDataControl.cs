using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static PublicClass.DLL;
using Debug = UnityEngine.Debug;

public class NumberDataControl : MonoBehaviour
{
    public Text ScoreUI_Text;
    public Text TimeUI_Text;
    public GameObject single_ClickTarget;

    public static bool gameIsStart = false;
    /// <summary>
    /// 游戏剩余时间，单位: 10ms
    /// </summary>
    private static int gameTime = 3000;
    public static int GetGameTime { get { return gameTime; } }
    /// <summary>
    /// 更改游戏剩余时间且更新UI显示<br/>
    /// 将值设置为-1时只刷新UI，不设置值
    /// </summary>
    public int GameTimeUI
    {
        get { return gameTime; }
        set
        {
            if(value!=-1)
                gameTime = value;
            string timeStr = gameTime.ToString().PadLeft(4, '0');
            TimeUI_Text.text = timeStr[..^2] + ":" + timeStr.Substring(timeStr.Length - 2, 2);
        }
    }
    /// <summary>
    /// 游戏得分
    /// </summary>
    private static int gameScore = 0;
    public static int GetGameScore
    {
        get { return gameScore; }
    }
    int GameScore
    {
        get { return gameScore; }
        set
        {
            gameScore = value;
            ScoreUI_Text.text = gameScore.ToString();
        }
    }

    /// <summary>
    /// 表示鼠标点击了某物
    /// </summary>
    /// <param name="isHit">是否命中目标</param>
    /// <param name="id">点击ID</param>
    internal delegate void MouseClickBall(bool isHit,int id);
    internal static event MouseClickBall MouseClickST_event;
    internal static void Trigger_MouseClickST_event(bool isHit_,int id_)
    {
        MouseClickST_event(isHit_,id_);
    }

    /// <summary>
    /// 全写: MouseClickBall<br/>
    /// 表示鼠标点击了某物
    /// </summary>
    /// <param name="isHit">是否命中目标</param>
    /// <param name="id">点击ID，用来判断加多少分</param>
    internal void MCB_Click(bool isHit, int id)
    {
        if (!gameIsStart)
        {
            gameIsStart = true;
            countdownThread = new(CountdownThread);
            countdownUIThread = new(CountdownUIThread);
            countdownThread.Start();
            countdownUIThread.Start();
        }
        if (isHit)
			GameScore+=id;
        else
            GameScore--;
        
    }
    Thread countdownThread;
    Thread countdownUIThread;
    void CountdownThread()
    {
        while (gameTime > 0)
        {
            Sleep(10);
            gameTime--;
        }
        gameTime = 0;

		ThreadDelegate.QueueOnMainThread((param) =>
		{
			if (Application.isPlaying)
              {
                GameObject gom = GameObject.Find("GameOverMenu_Root").transform.Find("GameOverMenu").gameObject;
                gom.SetActive(true);
				GameObject.Find("GameOverMenu/GameOverMenu_Score").GetComponent<Text>().text = $"最后得分: {gameScore}";
               GameOverMenu.RestartButton_ClickEvent += (() =>
            {
				single_ClickTarget.transform.localPosition = new Vector2(0, 0);
				GameScore = 0;
				gameIsStart = false;
				GameTimeUI = 3000;
                gom.SetActive(false);
			});
        }
		}, null);
	}
    void CountdownUIThread()
    {
        while(gameTime > 0)
        {            
            bool wait = false;
            ThreadDelegate.QueueOnMainThread((param) =>
            {
                GameTimeUI = -1;
                wait = true;
            }, null);
            Sleep(10);
            while (!wait) ;
        }
        ThreadDelegate.QueueOnMainThread((param) =>
        {
            GameTimeUI = 0;
        }, null);
    }
    private void Start()
    {
        NumberDataControl.MouseClickST_event += MCB_Click;
    }
    /// <summary>
    /// 更精准的Sleep函数
    /// </summary>
    /// <param name="ms">毫秒</param>
    static void Sleep(int ms)
    {
        var sw = Stopwatch.StartNew();
        var sleepMs = ms - 16;
        if (sleepMs > 0)
        {
            Thread.Sleep(sleepMs);
        }
        while (sw.ElapsedMilliseconds < ms)
        {
            Thread.Sleep(0);
        }
    }
}
