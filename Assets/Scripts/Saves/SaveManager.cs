using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ebac.core.Singleton;
using System;


public class SaveManager : Singleton<SaveManager>
{

	[SerializeField] private SaveSetup _saveSetup;
	private string _path = Application.streamingAssetsPath + "/save.txt";
	public int lastLevel;
	public Action<SaveSetup> FileLoaded;

	public SaveSetup Setup
    {
		get { return _saveSetup; }
    }

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void CreateNewSave()
    {

		SaveSetup setup = new SaveSetup();
		setup.lastLevel = 2;
		setup.playerName = "Paola";
	}


    private void Start()
    {
		Invoke(nameof(Load), .1f);
    }


    #region SAVE
    [NaughtyAttributes.Button]
    private void Save()
    {
		string setupToJson = JsonUtility.ToJson(_saveSetup, true);
		Debug.Log(setupToJson);
		SaveFile(setupToJson);
	}


	public void SaveName(string text)
	{
		_saveSetup.playerName = text;
		Save();
	}


	public void SaveItems()
	{
		_saveSetup.coins = Items.ItemManager.Instance.GetItemByType(Items.ItemType.LIFE_PACK).soInt.value;
		_saveSetup.coins = Items.ItemManager.Instance.GetItemByType(Items.ItemType.COIN).soInt.value;
		Save();


	}


	public void SaveLastLevel(int level)
    {
		_saveSetup.lastLevel = level;
		SaveItems();
		Save();

	}


	#endregion

	private void SaveFile(string json)
	{
		string path = Application.streamingAssetsPath + "/save.txt";

		Debug.Log(path);
		File.WriteAllText(path, json);

	}

	[NaughtyAttributes.Button]
	private void Load()
	{
		string fileLoaded = "";

		if (File.Exists(_path))
		{
			fileLoaded = fileLoaded = File.ReadAllText(_path);
			_saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);
			lastLevel = _saveSetup.lastLevel;
			FileLoaded.Invoke(_saveSetup);
		}

		else
		{
			CreateNewSave();
			Save();
		} 


		FileLoaded.Invoke(_saveSetup);
	}



	[NaughtyAttributes.Button]
	private void SaveLevelOne()
	{
		SaveLastLevel(1);
	}

	[NaughtyAttributes.Button]
	private void SaveLevelFile()
	{
		SaveLastLevel(5);
	}

}


[System.Serializable]
public class SaveSetup
{
	public int lastLevel;
	public int coins;
	public int health;
	public string playerName;
	public string qualquer;
}




