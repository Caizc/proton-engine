using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MoreMountains.Tools
{
	[ExecuteInEditMode]
	/// <summary>
	/// This static class is in charge of storing the current state of the achievements, unlocking/locking them, and saving them to data files
	/// </summary>
	public static class MMAchievementManager
	{
		private static List<MMAchievement> Achievements;
		private static MMAchievement _achievement = null;

		const string _defaultFileName = "Achievements";
		const string _saveFolderName = "/MMData/MMAchievements/";
		private static string _savePath;
		private static string _saveFileName;
		private static string _listID;

		/// <summary>
		/// You'll need to call this method to initialize the manager
		/// </summary>
		public static void LoadAchievementList()
		{
			Achievements = new List<MMAchievement> ();

			// the Achievement List scriptable object must be in a Resources folder inside your project, like so : Resources/Achievements/PUT_SCRIPTABLE_OBJECT_HERE
			MMAchievementList achievementList = (MMAchievementList) Resources.Load("Achievements/AchievementList");

			if (achievementList == null)
			{
				return;
			}

			// we store the ID for save purposes
			_listID = achievementList.AchievementsListID;

			foreach (MMAchievement achievement in achievementList.Achievements)
			{
				Achievements.Add (achievement.Copy());
			}
		}

		/// <summary>
		/// Unlocks the specified achievement (if found).
		/// </summary>
		/// <param name="achievementID">Achievement I.</param>
		public static void UnlockAchievement(string achievementID)
		{
			_achievement = AchievementManagerContains(achievementID);
			if (_achievement != null)
			{
				_achievement.UnlockAchievement();
			}
		}

		/// <summary>
		/// Locks the specified achievement (if found).
		/// </summary>
		/// <param name="achievementID">Achievement ID.</param>
		public static void LockAchievement(string achievementID)
		{
			_achievement = AchievementManagerContains(achievementID);
			if (_achievement != null)
			{
				_achievement.LockAchievement();
			}
		}

		/// <summary>
		/// Adds progress to the specified achievement (if found).
		/// </summary>
		/// <param name="achievementID">Achievement ID.</param>
		/// <param name="newProgress">New progress.</param>
		public static void AddProgress(string achievementID, int newProgress)
		{
			_achievement = AchievementManagerContains(achievementID);
			if (_achievement != null)
			{
				_achievement.AddProgress(newProgress);
			}
		}

		/// <summary>
		/// Sets the progress of the specified achievement (if found) to the specified progress.
		/// </summary>
		/// <param name="achievementID">Achievement ID.</param>
		/// <param name="newProgress">New progress.</param>
		public static void SetProgress(string achievementID, int newProgress)
		{
			_achievement = AchievementManagerContains(achievementID);
			if (_achievement != null)
			{
				_achievement.SetProgress(newProgress);
			}
		}		

		/// <summary>
		/// Determines if the achievement manager contains an achievement of the specified ID. Returns it if found, otherwise returns null
		/// </summary>
		/// <returns>The achievement corresponding to the searched ID if found, otherwise null.</returns>
		/// <param name="searchedID">Searched I.</param>
		private static MMAchievement AchievementManagerContains(string searchedID)
		{
			if (Achievements.Count == 0)
			{
				return null;
			}
			foreach(MMAchievement achievement in Achievements)
			{
				if (achievement.AchievementID == searchedID)
				{
					return achievement;					
				}
			}
			return null;
		}

		/// <summary>
		/// Removes saved data and resets all achievements from a list
		/// </summary>
		/// <param name="listID">The ID of the achievement list to reset.</param>
		public static void ResetAchievements(string listID)
		{
			if (Achievements != null)
			{
				foreach(MMAchievement achievement in Achievements)
				{
					achievement.ProgressCurrent = 0;
					achievement.UnlockedStatus = false;
				}	
			}

			DeterminePath (listID);
			MMDebug.DebugLogTime ("Removing " + _saveFileName);
			File.Delete(_saveFileName);
			MMDebug.DebugLogTime ("Removing " + _saveFileName+".meta");
			File.Delete(_saveFileName+".meta");
			MMDebug.DebugLogTime ("Achievements Reset");
		}

		public static void ResetAllAchievements()
		{
			LoadAchievementList ();
			ResetAchievements (_listID);
		}

		// SAVE ------------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Loads the saved achievements file and updates the array with its content.
		/// </summary>
		public static void LoadSavedAchievements()
		{
			DeterminePath ();

			// if the MMSaves directory or the save file doesn't exist, there's nothing to load, we do nothing and exit
			if (!Directory.Exists(_savePath) || !File.Exists(_saveFileName))
			{
				//MMDebug.DebugLogTime("Nothing to load at "+_saveFileName);
				return;
			}
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Open(_saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			SerializedMMAchievementManager serializedMMAchievementManager = (SerializedMMAchievementManager)formatter.Deserialize(saveFile);
			saveFile.Close();

			ExtractSerializedMMAchievementManager(serializedMMAchievementManager);
		}

		/// <summary>
		/// Saves the achievements current status to a file on disk
		/// </summary>
		public static void SaveAchievements()
		{
			DeterminePath ();

			if (!Directory.Exists(_savePath))
			{
				Directory.CreateDirectory(_savePath);
			}
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Create(_saveFileName);

			SerializedMMAchievementManager serializedMMAchievementManager = new SerializedMMAchievementManager();
			FillSerializedMMAchievementManager(serializedMMAchievementManager);

			formatter.Serialize(saveFile, serializedMMAchievementManager);

			saveFile.Close();
		}

		/// <summary>
		/// Determines the path the achievements save file should be saved to.
		/// </summary>
		private static void DeterminePath(string specifiedFileName = "")
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) 
			{
				_savePath = Application.persistentDataPath + _saveFolderName;
			} 
			else 
			{
				_savePath = Application.dataPath + _saveFolderName;
			}

			string tempFileName = (_listID != "") ? _listID : _defaultFileName;
			if (specifiedFileName != "")
			{
				tempFileName = specifiedFileName;
			}

			_saveFileName = _savePath + tempFileName + ".achievements.binary";
		}

		/// <summary>
		/// Serializes the contents of the achievements array to a serialized, ready to save object
		/// </summary>
		/// <param name="serializedInventory">Serialized inventory.</param>
		public static void FillSerializedMMAchievementManager(SerializedMMAchievementManager serializedAchievements)
		{
			serializedAchievements.Achievements = new SerializedMMAchievement[Achievements.Count];

			for (int i = 0; i < Achievements.Count(); i++)
			{
				SerializedMMAchievement newAchievement = new SerializedMMAchievement (Achievements[i].AchievementID, Achievements[i].UnlockedStatus, Achievements[i].ProgressCurrent);
				serializedAchievements.Achievements [i] = newAchievement;
				//MMDebug.DebugLogTime ("Save : " + serializedAchievements.Achievements [i].AchievementID+", status : "+serializedAchievements.Achievements [i].UnlockedStatus);
			}
		}

		/// <summary>
		/// Extracts the serialized achievements into our achievements array if the achievements ID match.
		/// </summary>
		/// <param name="serializedAchievements">Serialized achievements.</param>
		public static void ExtractSerializedMMAchievementManager(SerializedMMAchievementManager serializedAchievements)
		{
			for (int i = 0; i < Achievements.Count(); i++)
			{
				for (int j=0; j<serializedAchievements.Achievements.Length; j++)
				{
					if (Achievements[i].AchievementID == serializedAchievements.Achievements[j].AchievementID)
					{
						Achievements [i].UnlockedStatus = serializedAchievements.Achievements [j].UnlockedStatus;
						Achievements [i].ProgressCurrent = serializedAchievements.Achievements [j].ProgressCurrent;
						//MMDebug.DebugLogTime ("Load : " + serializedAchievements.Achievements [j].AchievementID+", status : "+serializedAchievements.Achievements [j].UnlockedStatus);
					}
				}
			}
		}
	}
}