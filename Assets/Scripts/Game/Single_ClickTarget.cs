using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_ClickTarget : MonoBehaviour
{
    const float minX = -8.7f;
    const float minY = -4.4f;
    const float maxX = 8.7f;
    const float maxY = 4.4f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
        {
            if (NumberDataControl.GetGameTime != 0)
            {
                // 射线检测碰撞器是否被点击
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
                // 不为null，则认为有物体撞到
                if (hit.collider != null)
                {
                    var hitObj = hit.collider.gameObject;
                    // 自行逻辑处理

                    transform.localPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    NumberDataControl.Trigger_MouseClickST_event(true);
                }
                else
                    NumberDataControl.Trigger_MouseClickST_event(false);
            }
        }
    }
}
