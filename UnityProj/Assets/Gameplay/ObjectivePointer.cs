using UnityEngine;
using System.Collections;

public class ObjectivePointer : MonoBehaviour {
	public GameObject target;
	public GameObject pointer;
	public Camera activeCamera;

    public GameObject monsterPointer;
    public GameObject spiritGatePointer;
    public enum Pointers
    {
        Monster_Pointer = 0,
        SpiritGate_Pointer
    }

	private Vector2 targetPos; //Screen space
	private bool isBehind;
	private float screenHeight;
	private float screenWidth;

	// Use this for initialization
	void Start () {
		screenHeight = Screen.height;
		screenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			targetPos = activeCamera.WorldToScreenPoint(target.transform.position);
			//Debug.Log("The target pos is : " + targetPos);

			if (!isTargetOnScreen())
			{
				Vector2 pointerPos = getPointerPosition(switchCoordSpace(targetPos));
				pointerPos = unSwitchCoordSpace(pointerPos);
				
				Vector3 guiPos = new Vector3(pointerPos.x / screenWidth, pointerPos.y / screenHeight, .0f);
				pointer.transform.position = guiPos;
				if (!pointer.GetComponent<GUITexture>().enabled)
					pointer.GetComponent<GUITexture>().enabled = true;
			}
			else
			{
				pointer.GetComponent<GUITexture>().enabled = false;
			}
		}
		else if (pointer != null && pointer.GetComponent<GUITexture>().enabled)
		{
			pointer.GetComponent<GUITexture>().enabled = false;
		}
	}

	private bool isTargetOnScreen()
	{
		
		Vector3 vectFromCam = target.transform.position - activeCamera.transform.position;
		Vector3 camVect = activeCamera.transform.forward;

		//Debug.Log(Vector3.Dot(camVect, vectFromCam));
		//Debug.DrawRay(activeCamera.transform.position, camVect * 10, Color.green);
		//Debug.DrawRay(activeCamera.transform.position, vectFromCam * 10, Color.blue);

		if (Vector3.Dot(camVect, vectFromCam) > .0f)
		{
			isBehind = false;

			if (targetPos.x > .0f && targetPos.x < screenWidth && targetPos.y > .0f && targetPos.y < screenHeight)
			{
				return true;
			}
		}
		else
		{
			isBehind = true;
		}
		
		return false;
	}

	private Vector2 switchCoordSpace(Vector2 _coord)
	{
		return new Vector2(_coord.x - screenWidth * .5f, _coord.y - screenHeight * .5f);
	}

	private Vector2 unSwitchCoordSpace(Vector2 _coord)
	{
		return new Vector2(_coord.x + screenWidth * .5f, _coord.y + screenHeight * .5f);
	}

	private Vector2 getPointerPosition(Vector2 _targetPos)
	{
		Vector2 pointerPos = new Vector2();

		if (isBehind)
		{
			_targetPos.x = -_targetPos.x;
			_targetPos.y = -_targetPos.y;
		}

		//Basic equation : y = mx + b, passing throught the center of the screen so b = 0
		// y = mx
		float m = _targetPos.y / _targetPos.x;

		if (_targetPos.y > .0f)
		{
			//Pointer on top of the screen
			float x = screenHeight * .5f / m;

			if (x - pointer.GetComponent<GUITexture>().pixelInset.x > screenWidth * .5f)
			{
				//Off-screen on the right side
				pointerPos.x = screenWidth * .5f - pointer.GetComponent<GUITexture>().pixelInset.width / 2.0f;
				//pointerPos.y = screenWidth * .5f * m;
                pointerPos.y = Mathf.Clamp(screenWidth * .5f * m, .0f, screenHeight * .5f + pointer.GetComponent<GUITexture>().pixelInset.y);
			}
			else if (x + pointer.GetComponent<GUITexture>().pixelInset.x < -screenWidth * .5f)
			{
				//Off-screen on the left side
				pointerPos.x = -screenWidth * .5f + pointer.GetComponent<GUITexture>().pixelInset.width / 2.0f;
                //pointerPos.y = -screenWidth * .5f * m;
                pointerPos.y = Mathf.Clamp(-screenWidth * .5f * m, .0f, screenHeight * .5f + pointer.GetComponent<GUITexture>().pixelInset.y);
            }
			else
			{
				pointerPos.x = x;
				pointerPos.y = screenHeight * .5f - pointer.GetComponent<GUITexture>().pixelInset.height/2.0f;
			}
		}
		else
		{
			//Pointer on bottom of the screen
			float x = -screenHeight * .5f / m;

			if (x - pointer.GetComponent<GUITexture>().pixelInset.x > screenWidth * .5f)
			{
				//Off-screen on the right side
				pointerPos.x = screenWidth * .5f - pointer.GetComponent<GUITexture>().pixelInset.width / 2.0f;
                //pointerPos.y = screenWidth * .5f * m;
                pointerPos.y = Mathf.Clamp(screenWidth * .5f * m, -screenHeight * .5f - pointer.GetComponent<GUITexture>().pixelInset.y, .0f);
            }
			else if (x + pointer.GetComponent<GUITexture>().pixelInset.x < -screenWidth * .5f)
			{
				//Off-screen on the left side
				pointerPos.x = -screenWidth * .5f + pointer.GetComponent<GUITexture>().pixelInset.width / 2.0f;
                //pointerPos.y = -screenWidth * .5f * m;
                pointerPos.y = Mathf.Clamp(-screenWidth * .5f * m, -screenHeight * .5f - pointer.GetComponent<GUITexture>().pixelInset.y, .0f);
            }
			else
			{
				pointerPos.x = x;
				pointerPos.y = -screenHeight * .5f + pointer.GetComponent<GUITexture>().pixelInset.height / 2.0f;
			}
		}

		return pointerPos;
	}

    public void SetPointer(Pointers _pointer)
    {
        switch(_pointer)
        {
            case Pointers.Monster_Pointer:
                pointer = monsterPointer;
                break;
            case Pointers.SpiritGate_Pointer:
                pointer = spiritGatePointer;
                break;
        }
    }
}
