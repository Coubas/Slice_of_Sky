using UnityEngine;
using System.Collections;

public static class AIUtils {

	public delegate float EasingMethod(float _currentTime, float _initialValue, float _targetValue, float _duration);

	public static float easingFunction(float _cursorVal, float _curTime, float _duration)
	{
		_cursorVal += (1 - _cursorVal) * (_curTime / _duration);
		return _cursorVal;
	}

	public static float QuadEaseInOut(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		if ((_currentTime /= _duration / 2) < 1)
			return _targetValue / 2 * _currentTime * _currentTime + _initialValue;

		return -_targetValue / 2 * ((--_currentTime) * (_currentTime - 2) - 1) + _initialValue;
	}

	public static float QuadEaseIn(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		return _targetValue * (_currentTime /= _duration) * _currentTime + _initialValue;
	}

	public static float QuadEaseOut(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		return -_targetValue * (_currentTime /= _duration) * (_currentTime - 2.0f) + _initialValue;
	}

	public static float ExpoEaseOut(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		return (_currentTime == _duration) ? _initialValue + _targetValue : _targetValue * (-Mathf.Pow(2, -10 * _currentTime / _duration) + 1) + _initialValue;
	}

	public static float BackEaseIn(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		return _targetValue * (_currentTime /= _duration) * _currentTime * ((1.70158f + 1.0f) * _currentTime - 1.70158f) + _initialValue;
	}

	public static float BackEaseInOut(float _currentTime, float _initialValue, float _targetValue, float _duration)
	{
		float s = 1.70158f;
		if ((_currentTime /= _duration / 2) < 1)
			return _targetValue / 2.0f * (_currentTime * _currentTime * (((s *= (1.525f)) + 1.0f) * _currentTime - s)) + _initialValue;
		return _targetValue / 2.0f * ((_currentTime -= 2) * _currentTime * (((s *= (1.525f)) + 1.0f) * _currentTime + s) + 2.0f) + _initialValue;
	}

	
	//Easing on vector
	public static Vector3 vectorEasing(Vector3 _from, Vector3 _to, float _currentTime, float _duration, EasingMethod _easingMethod)
	{
		float x = _easingMethod(_currentTime, _from.x, _to.x, _duration);
		float y = _easingMethod(_currentTime, _from.y, _to.y, _duration);
		float z = _easingMethod(_currentTime, _from.z, _to.z, _duration);

		return new Vector3(x, y, z);
	}
}
