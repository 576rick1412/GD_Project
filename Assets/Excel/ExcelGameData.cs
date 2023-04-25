using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Replace 'EntityType' to an actual type that is serializable.
[ExcelAsset]
public class ExcelGameData : ScriptableObject
{
	public List<ExcelItemDataBase> ItemDB_KR;
	public List<ExcelEnemyDataBase> EnemyInfo;
}
