using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ActionHelper
{
    public static IEnumerator StartAction(UnityAction action, float delay, bool unscaleTime = false)
    {
        if (unscaleTime)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        action();
    }
    /// <summary>
    /// Clear ALL object child of object parent
    /// </summary>
    /// <param name="transform"></param>
    public static void Clear(this Transform transform)
    {
        while (transform.childCount > 0)
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }
    /// <summary>
    /// set Vibration
    /// </summary>
    public static void SetVibration()
    {
    }
    public static string FomatHealth(string _str)
    {
        string strFomat = "";
        char[] listString = strFomat.ToCharArray();

        return strFomat;
    }
    //======================================================================================================================
    public static DateTime ConfigTimeLateMonth(DateTime _date)
    {
        if (_date.Month % 2 == 0) // 2 4 6 8 10 12
        {
            if (_date.Month == 2)
            {
                if (DateTime.IsLeapYear(_date.Year))
                {
                    if (_date.Day + 1 > 29)
                    {
                        DateTime dateTime = new DateTime(_date.Year, _date.Month + 1, 1);
                        return dateTime;
                    }
                }
                else
                {

                    if (_date.Day + 1 > 28)
                    {
                        DateTime dateTime = new DateTime(_date.Year, _date.Month + 1, 1);
                        return dateTime;
                    }
                }
            }

            if (_date.Month < 7) // 30 day
            {
                if (_date.Day + 1 > 30)
                {
                    DateTime dateTime = new DateTime(_date.Year, _date.Month + 1, 1);
                    return dateTime;
                }
            }
            else // 31 day
            {
                if (_date.Month == 12)
                {
                    if (_date.Day + 1 > 31)
                    {
                        DateTime dateTime = new DateTime(_date.Year + 1, 1, 1);
                        return dateTime;
                    }
                }
            }
        }

        else // 1 3 5 7 9 11
        {
            if (_date.Month <= 7) // 31 day
            {
                if (_date.Day + 1 > 31)
                {
                    DateTime dateTime = new DateTime(_date.Year, _date.Month + 1, 1);
                    return dateTime;
                }
            }
            else // 30 day
            {
                if (_date.Day + 1 > 30)
                {
                    DateTime dateTime = new DateTime(_date.Year + 1, 1, 1);
                    return dateTime;
                }
            }
        }
        return new DateTime(_date.Year, _date.Month, _date.Day + 1);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void CountTimeGetHeart(Text txtCountTime, float _timer, UnityAction callback)
    {
        if (VariableSystem.StoreHeart >= VariableSystem.MaxHeart)
        {
            txtCountTime.gameObject.SetActive(false);
            return;
        }
        txtCountTime.gameObject.SetActive(true);
        float t = VariableSystem.CountTimeGetHeart;

        int seconds = (int)(t % 60); // return the remainder of the seconds divide by 60 as an int
        t /= 60; // divide current time y 60 to get minutes
        int minutes = (int)(t % 60); //return the remainder of the minutes divide by 60 as an int

        txtCountTime.text = "(" + minutes + " : " + seconds.ToString("00") + ")";
        _timer += Time.deltaTime;
        if (_timer >= 1)
        {
            _timer = 0.0f;
            VariableSystem.CountTimeGetHeart--;
            if (VariableSystem.CountTimeGetHeart <= 0)
            {
                VariableSystem.CountTimeGetHeart = (int)VariableSystem.TimeGetHeart;
                VariableSystem.StoreHeart++;
                callback?.Invoke();
            }
        }
    }

}