using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
	internal static event Action RestartButton_ClickEvent;
	public void RestartButton_Click()
	{
		RestartButton_ClickEvent();
	}
	/*internal static Action CallShowMenu;
	void ShowMenu()
	{
		gameObject.SetActive(true);
	}
	private void Start()
	{
		CallShowMenu=new(ShowMenu);
	}*/
}
