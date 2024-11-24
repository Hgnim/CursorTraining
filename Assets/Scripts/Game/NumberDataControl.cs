using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Timers;

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
			/*countdownThread = new(CountdownThread);
            countdownUIThread = new(CountdownUIThread);
            countdownThread.Start();
            countdownUIThread.Start();*/
			//因为WebGL不支持多线程，所以改为协程运行
			StartCoroutine( CountdownThread());
			//CountdownUIThread();
		}
        if (isHit)
			GameScore+=id;
        else
            GameScore--;
        
    }
	IEnumerator CountdownThread()
    {
		static double tenms(DateTime startTime, DateTime endTime)
		{
			TimeSpan secondSpan = new(endTime.Ticks - startTime.Ticks);
			return secondSpan.TotalMilliseconds/10;
		}
		DateTime start= DateTime.Now;
        int gameTimeStartValue = gameTime;
		while (gameTime > 0)
		{
			//await TaskSleep(10);
			//await UniTask.Delay(10);
			yield return new WaitForSeconds(0.01f);
			//gameTime--;
			gameTime = gameTimeStartValue-(int)tenms(start,DateTime.Now);
			GameTimeUI = -1;
		}
        gameTime = 0;
		GameTimeUI = 0;

		/*ThreadDelegate.QueueOnMainThread((param) =>
		{*/
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
		/*}, null);*/	
	}
    private void Start()
    {
        NumberDataControl.MouseClickST_event += MCB_Click;
    }

}
