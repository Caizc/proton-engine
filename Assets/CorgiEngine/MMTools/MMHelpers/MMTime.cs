using UnityEngine;
using System.Collections;
using System;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
	/// </summary>

	public class MMTime : MonoBehaviour 
	{
		/// <summary>
	    /// Takes a hh:mm:ss:SSS string and turns it into a float value expressed in seconds
	    /// </summary>
	    /// <returns>a number of seconds.</returns>
	    /// <param name="timeInStringNotation">Time in string notation to decode.</param>
		public static float TimeStringToFloat(string timeInStringNotation)
	    {
			if (timeInStringNotation.Length!=12)
			{
				throw new Exception("The time in the TimeStringToFloat method must be specified using a hh:mm:ss:SSS syntax");
			}

			string[] timeStringArray = timeInStringNotation.Split(new string[] {":"},StringSplitOptions.None);

			float startTime=0f;
			float result;
			if (float.TryParse(timeStringArray[0], out result))
			{
				startTime+=result*3600f;
			}
			if (float.TryParse(timeStringArray[1], out result))
			{
				startTime+=result*60f;
			}
			if (float.TryParse(timeStringArray[2], out result))
			{
				startTime+=result;
			}
			if (float.TryParse(timeStringArray[3], out result))
			{
				startTime+=result/1000f;
			}

			return startTime;
	    }
	}
}
