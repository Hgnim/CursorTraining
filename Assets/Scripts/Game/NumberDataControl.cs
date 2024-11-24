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
    /// <param name="id">���ID</param>
    internal delegate void MouseClickBall(bool isHit,int id);
    internal static event MouseClickBall MouseClickST_event;
    internal static void Trigger_MouseClickST_event(bool isHit_,int id_)
    {
        MouseClickST_event(isHit_,id_);
    }

    /// <summary>
    /// ȫд: MouseClickBall<br/>
    /// ��ʾ�������ĳ��
    /// </summary>
    /// <param name="isHit">�Ƿ�����Ŀ��</param>
    /// <param name="id">���ID�������жϼӶ��ٷ�</param>
    internal void MCB_Click(bool isHit, int id)
    {
        if (!gameIsStart)
        {
            gameIsStart = true;
			/*countdownThread = new(CountdownThread);
            countdownUIThread = new(CountdownUIThread);
            countdownThread.Start();
            countdownUIThread.Start();*/
			//��ΪWebGL��֧�ֶ��̣߳����Ը�ΪЭ������
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
				GameObject.Find("GameOverMenu/GameOverMenu_Score").GetComponent<Text>().text = $"���÷�: {gameScore}";
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
