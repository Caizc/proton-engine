using UnityEngine;
using System.Collections;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Add this class to a GameObject to be able to control its position/rotation/scale individually and periodically, allowing it to "wiggle" (or just move however you want on a periodic basis)
	/// </summary>
	public class Wiggle : MonoBehaviour 
	{		
		[Header("Position Frequency")]
		/// the minimum time (in seconds) between two position changes
		public float PositionFrequencyMin = 0;
		/// the maximum time (in seconds) between two position changes
		public float PositionFrequencyMax = 1;
		[Header("Position Amplitude")]
		/// the minimum position the object can have
		public Vector3 PositionAmplitudeMin = Vector3.zero;
		/// the maximum position the object can have
		public Vector3 PositionAmplitudeMax = Vector3.one;

		[Header("Rotation Frequency")]
		/// the minimum time (in seconds) between two rotation changes
		public float RotationFrequencyMin = 0;
		/// the maximum time (in seconds) between two rotation changes
		public float RotationFrequencyMax = 1;
		[Header("Rotation Amplitude")]
		/// the object's minimum rotation
		public Vector3 RotationAmplitudeMin = Vector3.zero;
		/// the object's maximum rotation
		public Vector3 RotationAmplitudeMax = Vector3.one;

		[Header("Scale Frequency")]
		/// the minimum time (in seconds) between two scale changes
		public float ScaleFrequencyMin = 0;
		/// the maximum time (in seconds) between two scale changes
		public float ScaleFrequencyMax = 1;
		[Header("Scale Amplitude")]
		/// the object's minimum scale
		public Vector3 ScaleAmplitudeMin = Vector3.zero;
		/// the object's maximum scale
		public Vector3 ScaleAmplitudeMax = Vector3.one;

		protected Vector3 _startPosition;
		protected Quaternion _startRotation;
		protected Vector3 _startScale;

		protected float _randomPositionFrequency = 0;
		protected Vector3 _randomPositionAmplitude = Vector3.zero;

		protected float _randomRotationFrequency = 0;
		protected Vector3 _randomRotationAmplitude = Vector3.zero;

		protected float _randomScaleFrequency = 0;
		protected Vector3 _randomScaleAmplitude = Vector3.zero;

		protected float _positionTimer = 0;
		protected float _rotationTimer = 0;
		protected float _scaleTimer = 0;

		protected float _positionT = 0;
		protected float _rotationT = 0;
		protected float _scaleT = 0;

		protected Vector3 _newPosition = Vector3.zero;
		protected Quaternion _newRotation = Quaternion.identity;
		protected Vector3 _newScale = Vector3.zero;

		/// <summary>
		/// On Start() we trigger the initialization
		/// </summary>
		protected virtual void Start()
		{
			Initialization ();
		}

		/// <summary>
		/// On init we get the start values and trigger our coroutines for each property
		/// </summary>
		protected virtual void Initialization()
		{
			_startPosition = transform.localPosition;
			_startRotation = transform.localRotation;
			_startScale = transform.localScale;
			StartCoroutine(ComputePosition ());
			StartCoroutine(ComputeRotation ());
			StartCoroutine(ComputeScale ());
		}

		/// <summary>
		/// Every frame we update our object's position
		/// </summary>
		protected virtual void Update()
		{
			UpdatePosition ();
		}

		/// <summary>
		/// Lerps each property towards the new computed target position/rotation/scale
		/// </summary>
		protected virtual void UpdatePosition()
		{
			if (_randomPositionFrequency > 0)
			{
				_positionT = (Time.time - _positionTimer) / _randomPositionFrequency;
				transform.localPosition = Vector3.Lerp (transform.localPosition, _newPosition, _positionT);				
			}

			if (_randomRotationFrequency > 0)
			{
				_rotationT = (Time.time - _rotationTimer) / _randomRotationFrequency;
				transform.localRotation = Quaternion.Lerp (transform.localRotation, _newRotation, _rotationT);
			}

			if (_randomScaleFrequency > 0)
			{
				_scaleT = (Time.time - _scaleTimer) / _randomScaleFrequency;
				transform.localScale = Vector3.Lerp (transform.localScale, _newScale, _scaleT);
			}
		}

		/// <summary>
		/// Computes a new target position
		/// </summary>
		/// <returns>The position.</returns>
		protected virtual IEnumerator ComputePosition()
		{
			while (true)
			{
				// if it's time to decide of a new position
				_randomPositionFrequency = UnityEngine.Random.Range(PositionFrequencyMin,PositionFrequencyMax);

				_randomPositionAmplitude.x = UnityEngine.Random.Range(PositionAmplitudeMin.x,PositionAmplitudeMax.x);
				_randomPositionAmplitude.y = UnityEngine.Random.Range(PositionAmplitudeMin.y,PositionAmplitudeMax.y);
				_randomPositionAmplitude.z = UnityEngine.Random.Range(PositionAmplitudeMin.z,PositionAmplitudeMax.z);

				_newPosition = _startPosition + _randomPositionAmplitude;
				_positionTimer = Time.time;

				yield return new WaitForSeconds (_randomPositionFrequency);
			}
		}

		/// <summary>
		/// Computes a new target rotation
		/// </summary>
		/// <returns>The rotation.</returns>
		protected virtual IEnumerator ComputeRotation()
		{
			while (true)
			{
				// if it's time to decide of a new position
				_randomRotationFrequency = UnityEngine.Random.Range(RotationFrequencyMin,RotationFrequencyMax);

				_randomRotationAmplitude.x = UnityEngine.Random.Range(RotationAmplitudeMin.x,RotationAmplitudeMax.x);
				_randomRotationAmplitude.y = UnityEngine.Random.Range(RotationAmplitudeMin.y,RotationAmplitudeMax.y);
				_randomRotationAmplitude.z = UnityEngine.Random.Range(RotationAmplitudeMin.z,RotationAmplitudeMax.z);

				_newRotation = Quaternion.Euler(_randomRotationAmplitude);
				_rotationTimer = Time.time;

				yield return new WaitForSeconds (_randomRotationFrequency);
			}
		}

		/// <summary>
		/// Computes a new target scale
		/// </summary>
		/// <returns>The scale.</returns>
		protected virtual IEnumerator ComputeScale()
		{
			while (true)
			{
				// if it's time to decide of a new position
				_randomScaleFrequency = UnityEngine.Random.Range(ScaleFrequencyMin,ScaleFrequencyMax);

				_randomScaleAmplitude.x = UnityEngine.Random.Range(ScaleAmplitudeMin.x,ScaleAmplitudeMax.x);
				_randomScaleAmplitude.y = UnityEngine.Random.Range(ScaleAmplitudeMin.y,ScaleAmplitudeMax.y);
				_randomScaleAmplitude.z = UnityEngine.Random.Range(ScaleAmplitudeMin.z,ScaleAmplitudeMax.z);

				_newScale = _startScale + _randomScaleAmplitude;
				_scaleTimer = Time.time;

				yield return new WaitForSeconds (_randomScaleFrequency);
			}
		}
	}
}