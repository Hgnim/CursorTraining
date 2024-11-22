using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KeyManager : MonoBehaviour
{
    Thread keyListenThread;
    void Start()
    {
        keyListenThread = new(KeyListenThread);
        keyListenThread.Start();
    }
    public static class WaitLock
    {
        internal static bool exitGameWaitLock = false;
    }

    void KeyListenThread()
    {
        bool CheckEscKey()
        {
            bool tmp = false;
            bool wait = false;
            ThreadDelegate.QueueOnMainThread((param) =>
            {
                tmp = GameKey.EscClick();
                wait = true;
            }, null);
            while (!wait) ;
            return tmp;
        }
        while (keyListenThread.IsAlive)
        {
            if (!WaitLock.exitGameWaitLock &&
                CheckEscKey())
            {
                WaitLock.exitGameWaitLock = true;
                //ExitGameWait();
                ExitGame();
                }
            Thread.Sleep(10);
        }
    }
    /// <summary>
    /// ³¤°´escÍË³öÓÎÏ·
    /// </summary>
    void ExitGameWait()
    {
        ThreadDelegate.QueueOnMainThread((param) =>
        {
            GameObject quitText = GameObject.Find("MainCamera").transform.Find("Quit_Text").gameObject;
            quitText.SetActive(true);
            Text quitText_text = quitText.transform.Find("Quit_Text_text").gameObject.GetComponent<Text>();
            const string mTxt = "quit game";
            const int maxTime = 5;

            Thread runThread = new(() =>
            {
                int i = 0;
                bool keyCheck()
                {
                    bool wait = false;
                    bool tmp = false;
                    ThreadDelegate.QueueOnMainThread((param) =>
                    {
                        quitText_text.text = mTxt.PadRight(mTxt.Length + i, '.');
                        if (!GameKey.EscClick())
                            tmp = false;
                        else
                            tmp = true;
                        wait = true;
                    }, null);
                    while (!wait) ;
                    return tmp;
                }
                for (; i < maxTime; i++)
                {
                    if (!keyCheck())
                    {
                        break;
                    }
                    Thread.Sleep(240);
                }
                if (!keyCheck())//×îºóÒ»´Î¼ì²é£¬±ÜÃâÖÍºó
                {
                    goto exit;
                }
                ExitGame();
                goto over;
    exit:;//Í£Ö¹ÍË³ö³ÌÐò
                {
                    bool wait = false;
                    ThreadDelegate.QueueOnMainThread((param) =>
                    {
                        quitText_text.text = "";
                        quitText.SetActive(false);
                        wait = true;
                    }, null);
                    while (!wait) ;
                }
    over:;
                
            });
            runThread.Start();
        }, null);
    }
    void ExitGame()
    {
            bool wait = false;
            ThreadDelegate.QueueOnMainThread((param) =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
				wait = true;
            }, null);
            while (!wait) ;

        WaitLock.exitGameWaitLock = false;
    }
}
public static class GameKey
{
    /// <summary>
    /// XBox°´¼ü
    /// </summary>
    public static class XBox
    {
        /// <summary>
        /// ×ó²àÒ¡¸ËË®Æ½Öá
        /// X axis
        /// </summary>
        public const string LeftStickHorizontal = "LeftStickHorizontal";
        /// <summary>
        /// ×ó²àÒ¡¸Ë´¹Ö±Öá
        /// Y axis
        /// </summary>
        public const string LeftStickVertical = "LeftStickVertical";
        /// <summary>
        /// ÓÒ²àÒ¡¸ËË®Æ½Öá
        /// 4th axis
        /// </summary>
        public const string RightStickHorizontal = "RightStickHorizontal";
        /// <summary>
        /// ÓÒ²àÒ¡¸Ë´¹Ö±Öá
        /// 5th axis
        /// </summary>
        public const string RightStickVertical = "RightStickVertical";
        /// <summary>
        /// Ê®×Ö·½ÏòÅÌË®Æ½Öá
        /// 6th axis
        /// </summary>
        public const string DPadHorizontal = "DPadHorizontal";
        /// <summary>
        /// Ê®×Ö·½ÏòÅÌ´¹Ö±Öá
        /// 7th axis
        /// </summary>
        public const string DPadVertical = "DPadVertical";
        /// <summary>
        /// LT
        /// 9th axis
        /// </summary>
        public const string LT = "LT";
        /// <summary>
        /// RT
        /// 10th axis
        /// </summary>
        public const string RT = "RT";
        /// <summary>
        /// ×ó²àÒ¡¸Ë°´¼ü
        /// joystick button 8
        /// </summary>
        public const KeyCode LeftStick = KeyCode.JoystickButton8;
        /// <summary>
        /// ÓÒ²àÒ¡¸Ë°´¼ü
        /// joystick button 9
        /// </summary>
        public const KeyCode RightStick = KeyCode.JoystickButton9;
        /// <summary>
        /// A¼ü
        /// joystick button 0
        /// </summary>
        public const KeyCode A = KeyCode.JoystickButton0;
        /// <summary>
        /// B¼ü
        /// joystick button 1
        /// </summary>
        public const KeyCode B = KeyCode.JoystickButton1;
        /// <summary>
        /// X¼ü
        /// joystick button 2
        /// </summary>
        public const KeyCode X = KeyCode.JoystickButton2;
        /// <summary>
        /// Y¼ü
        /// joystick button 3
        /// </summary>
        public const KeyCode Y = KeyCode.JoystickButton3;
        /// <summary>
        /// LB¼ü
        /// joystick button 4
        /// </summary>
        public const KeyCode LB = KeyCode.JoystickButton4;
        /// <summary>
        /// RB¼ü
        /// joystick button 5
        /// </summary>
        public const KeyCode RB = KeyCode.JoystickButton5;
        /// <summary>
        /// ViewÊÓÍ¼¼ü
        /// joystick button 6
        /// </summary>
        public const KeyCode View = KeyCode.JoystickButton6;
        /// <summary>
        /// Menu²Ëµ¥¼ü
        /// joystick button 7
        /// </summary>
        public const KeyCode Menu = KeyCode.JoystickButton7;
    }


    readonly static KeyCode[] escKey = new KeyCode[] { XBox.Menu, KeyCode.Escape };
    readonly static KeyCode upKey = KeyCode.UpArrow;
    readonly static KeyCode downKey = KeyCode.DownArrow;
    readonly static KeyCode leftKey = KeyCode.LeftArrow;
    readonly static KeyCode rightKey = KeyCode.RightArrow;
    static float DPadH() { return Input.GetAxis(XBox.DPadHorizontal); }
    static float DPadV() { return Input.GetAxis(XBox.DPadVertical); }

    public static bool EscClick()
    {
        foreach (KeyCode k in escKey)
        {
            if (Input.GetKey(k))
                return true;
        }
        return false;
    }   

    public static bool UpKeyClick()
    {
        if (Input.GetKey(upKey)) return true;
        else if (DPadV() < 0) return true;
        else return false;
    }
    public static bool DownKeyClick()
    {
        if (Input.GetKey(downKey)) return true;
        else if (DPadV() > 0) return true;
        else return false;
    }
    public static bool LeftKeyClick()
    {
        if (Input.GetKey(leftKey)) return true;
        else if (DPadH() < 0) return true;
        else return false;
    }
    public static bool RightKeyClick()
    {
        if (Input.GetKey(rightKey)) return true;
        else if (DPadH() > 0) return true;
        else return false;
    }
}