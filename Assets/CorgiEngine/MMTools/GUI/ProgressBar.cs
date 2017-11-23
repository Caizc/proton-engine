using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;

/// <summary>
/// Add this bar to an object and link it to a bar (possibly the same object the script is on), and you'll be able to resize the bar object based on a current value, located between a min and max value.
/// See the HealthBar.cs script for a use case
/// </summary>
public class ProgressBar : MonoBehaviour
{
	/// the healthbar's foreground bar
	public Transform ForegroundBar;
	public string PlayerID;

	protected Vector3 _newLocalScale = Vector3.one;
	protected float _newPercent;

	public virtual void UpdateBar(float currentValue,float minValue,float maxValue)
	{
		_newPercent = MMMaths.Remap(currentValue,minValue,maxValue,0,1);
		_newLocalScale.x = _newPercent;
		if (ForegroundBar != null)
		{
			ForegroundBar.localScale = _newLocalScale;			
		}
	}
}