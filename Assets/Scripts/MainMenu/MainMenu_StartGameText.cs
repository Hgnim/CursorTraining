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
    public const string version = "1.1.1.20241124";

	private void Start()
	{
		GameObject.Find("TextUI/About").GetComponent<Text>().text =
@$"°æ±¾: {version}
Copyright (C) 2024 Hgnim, All rights reserved.
Github: https://github.com/Hgnim/CursorTraining";
	}

	private void Update()
    {        
        if (Input.anyKeyDown)
        {
            if(Input.GetKey(KeyCode.Escape)) 
                goto exitVoid;


            DontDestroyOnLoad(GameObject.Find("KeyManager")); 
            //DontDestroyOnLoad(GameObject.Find("ThreadDelegate"));
            SceneManager.LoadScene("Game");
		}
exitVoid:;
    }
}
