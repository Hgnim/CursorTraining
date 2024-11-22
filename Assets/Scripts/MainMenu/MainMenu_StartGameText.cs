using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PublicClass.DLL;

public class MainMenu_StartGameText : MonoBehaviour
{
    public const string version = "1.1.0.20241122";



    private void Update()
    {        
        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.A)) 
            {
                MessageBox(IntPtr.Zero,
                    "游戏名: 光标训练(CursorTraining)\r\n" +
                    "版本: V"+version+"\r\n" +
                    "Copyright (C) 2024 Hgnim, All rights reserved.\r\n" +
                    "Github: https://github.com/Hgnim/CursorTraining"
                    , "关于", 0);
                goto exitVoid;
            }
            else if(Input.GetKey(KeyCode.Escape)) 
                goto exitVoid;


            DontDestroyOnLoad(GameObject.Find("KeyManager")); 
            DontDestroyOnLoad(GameObject.Find("ThreadDelegate"));
            SceneManager.LoadScene("Game");
		}
exitVoid:;
    }
}
