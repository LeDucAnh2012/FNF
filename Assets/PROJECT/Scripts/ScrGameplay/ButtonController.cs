using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace FridayNightFunkin.GamePlay
{
    public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public int typeButton;
        public KeyCode key;

        public void OnPointerDown(PointerEventData eventData)
        {
            ClickDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ClickUp();
        }
        public void ClickDown()
        {
            Player.instance.ClickDown(typeButton);

        }
        public void ClickUp()
        {
            Player.instance.ClickUp(typeButton);
        }

        //private void OnMouseDown()
        //{
        //    Player.instance.ClickDown(typeButton);
        //}
        //private void OnMouseUp()
        //{
        //    Player.instance.ClickUp(typeButton);
        //}
        //private void Update()
        //{
        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
        //        if (hit.collider != null)
        //        {
        //            Debug.Log("touch success");
        //            Player.instance.ClickDown(typeButton);
        //        }
        //        else
        //        {
        //            Player.instance.ClickUp(typeButton);
        //        }
        //    }
        //}

    }
}