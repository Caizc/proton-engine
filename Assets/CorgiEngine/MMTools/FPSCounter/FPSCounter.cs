using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
/// <summary>
/// Add this class to a gameObject with a Text component and it'll feed it the number of FPS in real time.
/// </summary>
public class FPSCounter : MonoBehaviour
{
	/// <summary>
	/// The frequency at which the FPS counter should update (in seconds)
	/// </summary>
	public float UpdateInterval = 0.5f;
 
	protected float _framesAccumulated = 0f; 
	protected float _framesDrawnInTheInterval = 0f; 
	protected float _timeLeft ; 
	protected Text _text;

	/// <summary>
	/// On Start(), we get the Text component and initialize our counter
	/// </summary>
	protected virtual void Start()
	{
		if(GetComponent<Text>()==null)
	    {
	        Debug.LogWarning ("FPSCounter requires a GUIText component.");
	        return;
		}
		_text = GetComponent<Text>();
	    _timeLeft = UpdateInterval;  
	}

	/// <summary>
	/// On Update, we increment our various counters, and if we've reached our UpdateInterval, we update our FPS counter 
	/// with the number of frames displayed since the last counter update
	/// </summary>
	protected virtual void Update()
	{
		_framesDrawnInTheInterval++;
		_framesAccumulated = _framesAccumulated + Time.timeScale/Time.deltaTime;
		_timeLeft = _timeLeft - Time.deltaTime;
	 
	    if( _timeLeft <= 0.0 )
	    {
			_text.text = string.Concat("FPS: ",(_framesAccumulated/_framesDrawnInTheInterval).ToString("f2"));
	        _framesDrawnInTheInterval = 0;
			_framesAccumulated = 0f;
	        _timeLeft = UpdateInterval;
	    }
	}
}