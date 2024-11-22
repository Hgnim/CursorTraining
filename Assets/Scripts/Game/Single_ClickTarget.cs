using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_ClickTarget : MonoBehaviour
{
	private void Start()
	{
		GameObject.Find("Single_ClickTarget").transform.localPosition = new Vector2(0, 0);
	}
	const float minX = -8.7f;
    const float minY = -4.4f;
    const float maxX = 8.7f;
    const float maxY = 4.4f;

    internal static bool HaveClick = false;
    internal static int[] GetNum = new int[3];
    void Update()
    {
        if (HaveClick) { 
            while (true)
            {
                bool whileLock=false;
                foreach(int n in GetNum)
                {
                    if (n == 0)
                    {
                        whileLock = true;
						break;
                    }
                }
				if (!whileLock)
					break;
			}
            int ID = 0;
            for(int i = 0; i < GetNum.Length; i++)
            {
                if( GetNum[i] == 1)
                {
                    ID = i + 1; break;
                }
            }
            if (ID == 0)
                NumberDataControl.Trigger_MouseClickST_event(false, 0);
            else
            {
                NumberDataControl.Trigger_MouseClickST_event(true, ID);
                GameObject.Find("Single_ClickTarget").transform.localPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            }

			HaveClick = false;
			GetNum = new int[3];
		}
    }
}
