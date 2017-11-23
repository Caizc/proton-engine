using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MoreMountains.Tools
{	
	public static class SaveManager 
	{
		private const string _baseFolderName = "/MMData/";
		private const string _defaultFolderName = "SaveManager";

		/*protected virtual void Awake()
		{
			
			_saveFileName = _savePath+gameObject.name+".achievements.binary";
		}*/

		static string DetermineSavePath(string folderName = _defaultFolderName)
		{
			string savePath;
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				savePath = Application.persistentDataPath + _baseFolderName;
			} 
			else 
			{
				savePath = Application.dataPath + _baseFolderName;
			}
			savePath = savePath + folderName + "/";
			return savePath;
		}

		static string DetermineSaveFileName(string fileName)
		{
			return fileName+".binary";
		}

		public static void Save(object saveObject, string fileName, string foldername = _defaultFolderName)
		{
			string savePath = DetermineSavePath(foldername);
			string saveFileName = DetermineSaveFileName(fileName);

			if (!Directory.Exists(savePath))
			{
				//MMDebug.DebugLogTime("save : directory doesn't exist, let's create it");
				Directory.CreateDirectory(savePath);
			}
	        BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Create(saveFileName);
			formatter.Serialize(saveFile, saveObject);

	        saveFile.Close();
		}

		public static object Load(object saveObject, string fileName, string foldername = _defaultFolderName)
		{
			string savePath = DetermineSavePath(foldername);
			string saveFileName = savePath + DetermineSaveFileName(fileName);

			object returnObject;

			// if the MMSaves directory or the save file doesn't exist, there's nothing to load, we do nothing and exit
			if (!Directory.Exists(savePath) || !File.Exists(saveFileName))
			{
				//MMDebug.DebugLogTime("nothing to load at "+_saveFileName);
				return null;
			}
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Open(saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			returnObject = formatter.Deserialize(saveFile);
	        saveFile.Close();

			return returnObject;
		}
	}
}