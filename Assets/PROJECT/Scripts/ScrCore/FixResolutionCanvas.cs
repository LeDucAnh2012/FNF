using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FixResolutionCanvas : MonoBehaviour {
	public Camera MainCam ;

    public static FixResolutionCanvas current;

    public string screenRatio;
    public int indexScreen = 0;
	public KeyCode TestBtn;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 4:3)")]
	public float WitdhOrHeightFactor43;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 3:2)")]
	public float WitdhOrHeightFactor32;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 16:10)")]
	public float WitdhOrHeightFactor1610;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 16:9)")]
	public float WitdhOrHeightFactor169;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 18:9)")]
	public float WitdhOrHeightFactor189;
	[Tooltip("Uu tien scale theo chieu rong hay chieu cao (man hinh 19:9)")]
	public float WitdhOrHeightFactor199;
	// Use this for initialization
	void Awake(){
        current = this;
		setWindowAspectRatio ();
	}

	void setWindowAspectRatio(){
		float _screenRatio = MainCam.aspect;
		float ratio = MainCam.aspect;
		string _screenRatio_ = _screenRatio.ToString ("F2");
		screenRatio = _screenRatio_.Substring (0, 4);
		
		if(0 < ratio && ratio <= 0.49)
        {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor199;
			indexScreen = 9;
		}
		else if( 0.49f < ratio && ratio <= 0.5f)
        {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor189;
			indexScreen = 5;
		}
		else if(0.5f < ratio && ratio <= 0.56f)
        {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor169;
			indexScreen = 4;
		}
		else if(0.56f < ratio && ratio <= 0.63)
        {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor1610;
			indexScreen = 3;
		}
		else if(0.63 < ratio && ratio <= 0.67)
        {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor32;
			indexScreen = 2;
		}
		else if(0.67 < ratio && ratio <= 0.75) {
			GetComponent<CanvasScaler>().matchWidthOrHeight = WitdhOrHeightFactor43;
			indexScreen = 1;
		}
		Debug.Log ("Screen" + screenRatio);
	}

}
