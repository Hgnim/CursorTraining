using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Single_ClickTarget;

public class Single_ClickTarget_in : MonoBehaviour
{
	public int TheID;
	void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			if ( NumberDataControl.GetGameTime != 0 && GetNum[TheID]==0)
			{
				HaveClick = true;
				Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

				if (hit.collider != null && hit.collider.gameObject.name == $"Single_ClickTarget_in{TheID}")				
						GetNum[TheID] = 1;	
				else
				GetNum[TheID] = 2;
			}
		}
	}
}
