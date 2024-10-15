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
    /// ��Ϸʣ��ʱ�䣬��λ: 10ms
    /// </summary>
    private static int gameTime = 3000;
    public static int GetGameTime { get { return gameTime; } }
    /// <summary>
    /// ������Ϸʣ��ʱ���Ҹ���UI��ʾ<br/>
    /// ��ֵ����Ϊ-1ʱֻˢ��UI��������ֵ
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
    /// ��Ϸ�÷�
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
    /// ��ʾ�������ĳ��
    /// </summary>
    /// <param name="isHit">�Ƿ�����Ŀ��</param>
    internal delegate void MouseClickSomething(bool isHit);
    internal static event MouseClickSomething MouseClickST_event;
    internal static void Trigger_MouseClickST_event(bool isHit_)
    {
        MouseClickST_event(isHit_);
    }

    /// <summary>
    /// ȫд: MouseClickSomething<br/>
    /// ��ʾ�������ĳ��
    /// </summary>
    /// <param name="isHit">�Ƿ�����Ŀ��</param>
    internal void MouseClickST(bool isHit)
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
            GameScore++;
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

        MessageBox(IntPtr.Zero, "���÷�Ϊ: " + gameScore.ToString(), "��Ϸ����", 0);
        {
            bool wait = false;
            ThreadDelegate.QueueOnMainThread((param) =>
            {
                single_ClickTarget.transform.localPosition = new Vector2(0, 0);
                GameScore = 0;
                gameIsStart = false;
                GameTimeUI = 3000;
                wait = true;
            }, null);
            while (!wait) ;
        }
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
        NumberDataControl.MouseClickST_event += MouseClickST;
    }
    /// <summary>
    /// ����׼��Sleep����
    /// </summary>
    /// <param name="ms">����</param>
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
