using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using RoboRyanTron.Unite2017.Variables;

[CreateAssetMenu(fileName = "PlayerDataCollection", menuName = "CustomSO/PlayerData/PlayerDataCollection")]
public class PlayerDataCollection : ScriptableObject
{

    [Header("Player Description")]
    public string playerName;

    [Header("Transform Data")]
    public Vector3 homePlayerPosition;
    public Vector3 customPlayerPosition;
    public Vector3 savedPlayerPosition;
    public Vector3Variable currentPlayerPosition;

    //public Quaternion currentPlayerRotation;
    //public Quaternion customPlayerRotation;
    //public float savedPlayerRotY;
    //public Quaternion savedPlayerRotation;
    //public QuaternionVariable currentPlayerRotation;

    public float savedPlayerRotY;
    public FloatVariable currentPlayerRotY;
    // public Quaternion currentPlayerRotation;

    [Header("Game Progress Data")]
    public InventoryList masterInventoryList;
    public TaskList masterTaskList;
    public CharacterProfileList masterCharacterProfList;
    public AreaList masterAreaList;

    [Header("Score Data")]
    public PointsList masterPlayerPoints;

    [Header("Stress Data")]
    public ResiliencyData playerResilienceHealth;
    public CopingMechanismList_SO masterCopingMechanismList;


    [Header("Meta")]
    public string persisterDescription = "PlayerData";

    [Serializable]
    public struct PlayerBasicData
    {
        public string playerName;
        public Vector3 savedPlayerPos;
        public float savedPlayerRotY;
       // public Quaternion savedPlayerRot;
        public int resilienceHealth;

    }
    public PlayerBasicData playerBasicData;


    [Serializable]
    public struct ItemData
    {
        public string itemName;
        public bool isCarrying, isRemoveFromGame;

    }
    private ItemData[] itemDataArray;



    [Serializable]
    public struct PlayerPointsData
    {
        public string gameName;
        public int points;

    }
    private PlayerPointsData[] playerPointsDataArray;


    [Serializable]
    public struct CopingMechanismData
    {
        public string copingMechName;
        public bool isCopingMechAvailable;
    }
    private CopingMechanismData[] copingMechDataArray;


    [Serializable]
    public struct TaskData
    {
        public string taskName;
        public Task_Status taskStatus;
    }
    private TaskData[] taskDataArray;

    [Serializable]
    public struct CharacterProfile_Data
    {
        public string characterName;
        public bool isTransportToAvailable;
        public Vector3 location;
 
    }
    private CharacterProfile_Data[] characterProfDataArray;
    [Serializable]
    public struct Area_Data
    {
        public string areaName;
        public bool isAvailable;
        public bool isShowing;
    }
    private Area_Data[] areaDataArray;


    public void SaveData(bool PlayerNamePosRes = true, bool Items = true, bool Points = true, bool CopingMech = true, bool Tasks = true, bool charProf = true, bool area = true)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        var json = String.Empty;

        if (PlayerNamePosRes)
        {
            #region PLAYERBASICDATA_SAVE
            playerBasicData = new PlayerBasicData
            {
                playerName = this.playerName,
                savedPlayerPos = this.savedPlayerPosition,
                savedPlayerRotY = this.savedPlayerRotY,
// savedPlayerRot = this.savedPlayerRotation,
                resilienceHealth = this.playerResilienceHealth.resilienceHealth,

            };


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_PlayerBasicData", 1));
            //   StreamWriter Writer = new StreamWriter(file);
            json = JsonUtility.ToJson(playerBasicData);
            bf.Serialize(file, json);
            file.Close();
            // Writer.Close();
            #endregion
        }
        if (Items)
        {
            #region ITEMDATE_SAVE

            itemDataArray = new ItemData[masterInventoryList.Items.Count];

            for (int i = 0; i < masterInventoryList.Items.Count; i++)
            {
                var itemData = new ItemData
                {
                    itemName = masterInventoryList.Items[i].itemName,
                    isCarrying = masterInventoryList.Items[i].isPlayerCarrying,
                    isRemoveFromGame = masterInventoryList.Items[i].isRemoveFromGame

                };
                this.itemDataArray[i] = itemData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1));
            json = JsonHelper.ToJson<ItemData>(itemDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();

            #endregion
        }
        if (Points)
        {
            #region PLATERPOINTS_SAVE

            playerPointsDataArray = new PlayerPointsData[masterPlayerPoints.Items.Count];

            for (int i = 0; i < masterPlayerPoints.Items.Count; i++)
            {
                var pointData = new PlayerPointsData
                {
                    points = masterPlayerPoints.Items[i].Value,
                    gameName = masterPlayerPoints.Items[i].poins_Description,

                };
                this.playerPointsDataArray[i] = pointData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_points", 1));
            json = JsonHelper.ToJson<PlayerPointsData>(playerPointsDataArray, true);
            bf.Serialize(file, json);


            Debug.Log(json);
            file.Close();

            #endregion
        }
        if (CopingMech)
        {
            #region COPINGMECHANISMS_SAVE

            int copingMechCount = masterCopingMechanismList.Items.Count;
            copingMechDataArray = new CopingMechanismData[copingMechCount];

            for (int i = 0; i < copingMechCount; i++)
            {
                var copingMechData = new CopingMechanismData
                {
                    copingMechName = masterCopingMechanismList.Items[i].description,
                    isCopingMechAvailable = masterCopingMechanismList.Items[i].isOn,

                };
                this.copingMechDataArray[i] = copingMechData;

            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CopingMechanisms", 1));
            json = JsonHelper.ToJson<CopingMechanismData>(copingMechDataArray, true);
            bf.Serialize(file, json);


            Debug.Log(json);
            file.Close();


            #endregion
        }
        if (Tasks)
        {
            #region TASK_SAVE

            int TaskCount = masterTaskList.Items.Count;
            taskDataArray = new TaskData[TaskCount];

            for (int i = 0; i < TaskCount; i++)
            {
                var taskData = new TaskData
                {
                    taskName = masterTaskList.Items[i].taskDescription,
                    taskStatus = masterTaskList.Items[i].taskStatus,

                };
                this.taskDataArray[i] = taskData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_Tasks", 1));
            json = JsonHelper.ToJson<TaskData>(taskDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();

            #endregion
        }
        if (charProf)
        {
            #region CharacterProfile_SAVE

            int charProfCount = masterCharacterProfList.Items.Count;
            characterProfDataArray = new CharacterProfile_Data[charProfCount];

            for (int i = 0; i < charProfCount; i++)
            {
                var charProfData = new CharacterProfile_Data
                {
                    characterName = masterCharacterProfList.Items[i].characterName,
                    isTransportToAvailable = masterCharacterProfList.Items[i].isAvailable,
                    location = masterCharacterProfList.Items[i].location
                   
                };
                this.characterProfDataArray[i] = charProfData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CharacterProfileData", 1));
            json = JsonHelper.ToJson<CharacterProfile_Data>(characterProfDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();

            #endregion
        }
        if (area)
        {
            #region AREA_SAVE

            int areaCount = masterAreaList.Items.Count;
            areaDataArray = new Area_Data[areaCount];

            for (int i = 0; i < areaCount; i++)
            {
                var areaData = new Area_Data
                {
                    areaName = masterAreaList.Items[i].areaName,
                    isAvailable = masterAreaList.Items[i].isAvailable,
                    isShowing = masterAreaList.Items[i].isShowing,
                    
                };
                this.areaDataArray[i] = areaData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_AreaData", 1));
            json = JsonHelper.ToJson<Area_Data>(areaDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();

            #endregion
        }
    }

    public void LoadALLData(bool PlayerNamePosRes = true, bool Items = true, bool Points = true, bool CopingMech = true, bool Tasks = true, bool charProf = true, bool area = true)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        var json = String.Empty;

        if (PlayerNamePosRes)
        {
            #region PLAYERBASICDATA_LOAD

            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_PlayerBasicData", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_PlayerBasicData", 1), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), playerBasicData);
                file.Close();

                
                this.savedPlayerPosition = playerBasicData.savedPlayerPos;
                this.savedPlayerRotY = playerBasicData.savedPlayerRotY;
               // this.savedPlayerRotation = playerBasicData.savedPlayerRot;
                this.playerResilienceHealth.resilienceHealth = playerBasicData.resilienceHealth;
                this.playerName = playerBasicData.playerName;
            }
            #endregion
        }
        if (Items)
        {
            #region PLAYERITEM_LOAD;
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1), FileMode.Open);
                itemDataArray = JsonHelper.FromJson<ItemData>((string)bf.Deserialize(file));
                file.Close();



                foreach (var savedItem in itemDataArray)
                {
                    foreach (var masterListItem in masterInventoryList.Items)
                    {
                        if (string.Equals(savedItem.itemName, masterListItem.itemName, StringComparison.CurrentCultureIgnoreCase))
                        {

                            masterListItem.isPlayerCarrying = savedItem.isCarrying;
                            masterListItem.isRemoveFromGame = savedItem.isRemoveFromGame;
                            break;
                            // return;
                        }
                    }

                }
            }
            #endregion
        }
        if (Points)
        {
            #region PLAYERPOINT_LOAD
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_points", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_points", 1), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), playerPointsDataArray);
                //playerPointsData = JsonHelper.FromJson<PlayerPointsData>((string)bf.Deserialize(file));
                file.Close();





                foreach (var masterPointList in masterPlayerPoints.Items)
                {
                    foreach (var savedPoints in playerPointsDataArray)
                    {
                        if (string.Equals(savedPoints.gameName, masterPointList.poins_Description, StringComparison.CurrentCultureIgnoreCase))
                        {
                            masterPointList.Value = savedPoints.points;
                            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1)))
                            {
                                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1), FileMode.Open);
                                itemDataArray = JsonHelper.FromJson<ItemData>((string)bf.Deserialize(file));
                                file.Close();

                            }

                            foreach (var savedItem in itemDataArray)
                            {
                                foreach (var masterListItem in masterInventoryList.Items)
                                {
                                    if (string.Equals(savedItem.itemName, masterListItem.itemName, StringComparison.CurrentCultureIgnoreCase))
                                    {

                                        masterListItem.isPlayerCarrying = savedItem.isCarrying;
                                        masterListItem.isRemoveFromGame = savedItem.isRemoveFromGame;
                                        break;
                                        // return;
                                    }
                                }

                            }

                            //return ;
                        }
                    }
                }

            }
            #endregion
        }
        if (CopingMech)
        {
            #region COPINGMECHANISMS_LOAD


            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CopingMechanisms", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CopingMechanisms", 1), FileMode.Open);
                copingMechDataArray = JsonHelper.FromJson<CopingMechanismData>((string)bf.Deserialize(file));
                file.Close();


                foreach (var savedCM in copingMechDataArray)
                {
                    foreach (var masterCM in masterCopingMechanismList.Items)
                    {
                        if (string.Equals(savedCM.copingMechName, masterCM.description, StringComparison.CurrentCultureIgnoreCase))
                        {

                            masterCM.description = savedCM.copingMechName;
                            masterCM.isOn = savedCM.isCopingMechAvailable;
                            break; //EXITS FIRST INNER LOOP? ++
                                   // return;// EXITS WHOLE FUNCTION EXECUTION NOT JUST ONE ARRAY
                        }
                    }
                }

            }

            #endregion
        }
        if (Tasks)
        {
            #region TASK_LOAD

            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_Tasks", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_Tasks", 1), FileMode.Open);
                taskDataArray = JsonHelper.FromJson<TaskData>((string)bf.Deserialize(file));
                file.Close();

                foreach (var savedTask in taskDataArray)
                {
                    foreach (var masterListTask in masterTaskList.Items)
                    {
                        if (string.Equals(savedTask.taskName, masterListTask.taskDescription, StringComparison.CurrentCultureIgnoreCase))
                        {

                            masterListTask.taskStatus = savedTask.taskStatus;
                            break;
                            //continue;
                        }
                    }

                }
            }
            #endregion
        }
        if (charProf)
        {
            #region CHARACTERPROFILE_LOAD

            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CharacterProfileData", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CharacterProfileData", 1), FileMode.Open);
                characterProfDataArray = JsonHelper.FromJson<CharacterProfile_Data>((string)bf.Deserialize(file));
                file.Close();

                foreach (var savedCharProfData in characterProfDataArray)
                {
                    foreach (var masterListCharProf in masterCharacterProfList.Items)
                    {
                        if (string.Equals(savedCharProfData.characterName, masterListCharProf.characterName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            masterListCharProf.isAvailable = savedCharProfData.isTransportToAvailable;
                            break;
                            //continue;
                        }
                    }

                }
            }
            #endregion
        }
        if (area)
        {
            #region AREA_LOAD

            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_AreaData", 1)))
            {
                file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_AreaData", 1), FileMode.Open);
                areaDataArray = JsonHelper.FromJson<Area_Data>((string)bf.Deserialize(file));
                file.Close();

                foreach (var savedAreaData in areaDataArray)
                {
                    foreach (var masterListAreaList in masterAreaList.Items)
                    {
                        if (string.Equals(savedAreaData.areaName, masterListAreaList.areaName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            masterListAreaList.isAvailable = savedAreaData.isAvailable;
                            masterListAreaList.isShowing = savedAreaData.isShowing;

                            break;
                            //continue;
                        }
                    }

                }
            }
            #endregion
        }

    }
    [ContextMenu("DeleteAllData")]
    public void RemovePlayerData(bool PlayerNamePosRes = true, bool Items = true, bool Points = true, bool CopingMech = true, bool Tasks = true, bool charProf = true, bool area = true)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        var json = String.Empty;

        if (PlayerNamePosRes)
        {
            #region PLAYERBASICDATA_REMOVEDATA
            playerBasicData = new PlayerBasicData
            {
                playerName = string.Empty,
                savedPlayerPos = homePlayerPosition,
                savedPlayerRotY = 0,
              //  savedPlayerRot = Quaternion.identity,
                resilienceHealth = 100,
                
            };
            playerName = String.Empty;
            savedPlayerRotY = 0;
            //savedPlayerRotation = Quaternion.identity;
            savedPlayerPosition = homePlayerPosition;
            playerResilienceHealth.resilienceHealth = 100;

            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_PlayerBasicData", 1));
            json = JsonUtility.ToJson(playerBasicData);
            bf.Serialize(file, json);
            file.Close();

            #endregion
        }
        if (Items)
        {
            #region PLAYERITEM_REMOVEDATA
            itemDataArray = new ItemData[masterInventoryList.Items.Count];

            for (int i = 0; i < masterInventoryList.Items.Count; i++)
            {
                var itemData = new ItemData
                {

                    isCarrying = false,
                isRemoveFromGame = false,

                };
                this.itemDataArray[i] = itemData;
                masterInventoryList.Items[i].isPlayerCarrying = false;
                masterInventoryList.Items[i].isRemoveFromGame = false;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_ItemStatus", 1));
            json = JsonHelper.ToJson<ItemData>(itemDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();
            #endregion
        }
        if (Points)
        {
            #region PLAYERPOINT_REMOVEDATA
            playerPointsDataArray = new PlayerPointsData[masterPlayerPoints.Items.Count];

            for (int i = 0; i < masterPlayerPoints.Items.Count; i++)
            {
                var pointData = new PlayerPointsData
                {
                    points = 0,
                  //  gameName = masterPlayerPoints.Items[i].poins_Description,

                };
                this.playerPointsDataArray[i] = pointData;
                masterPlayerPoints.Items[i].Value = 0;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_points", 1));
            json = JsonHelper.ToJson<PlayerPointsData>(playerPointsDataArray, true);
            bf.Serialize(file, json);


            Debug.Log(json);
            file.Close();
            #endregion
        }
        if (CopingMech)
        {
            #region COPINGMECHANISMS_REMOVEDATA

            int copingMechCount = masterCopingMechanismList.Items.Count;
            copingMechDataArray = new CopingMechanismData[copingMechCount];

            for (int i = 0; i < copingMechCount; i++)
            {
                var copingMechData = new CopingMechanismData
                {
                    //copingMechName = masterCopingMechanismList.Items[i].description,
                    isCopingMechAvailable = false,

                };
                this.copingMechDataArray[i] = copingMechData;
                this.masterCopingMechanismList.Items[i].isOn = false;

            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CopingMechanisms", 1));
            json = JsonHelper.ToJson<CopingMechanismData>(copingMechDataArray, true);
            bf.Serialize(file, json);


            Debug.Log(json);
            file.Close();

            #endregion
        }
        if (Tasks)
        {
            #region TASK_REMOVEDATA

            int TaskCount = masterTaskList.Items.Count;
            taskDataArray = new TaskData[TaskCount];

            for (int i = 0; i < TaskCount; i++)
            {
                masterTaskList.Items[i].taskStatus = Task_Status.NOT_IDENTIFIED;
                taskDataArray[i].taskStatus = Task_Status.NOT_IDENTIFIED;
                //   var taskData = new TaskData
                //{
                //    taskStatus = Task_Status.NOT_IDENTIFIED,
                //};
                //this.taskDataArray[i] = taskData;
            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_Tasks", 1));
            json = JsonHelper.ToJson<TaskData>(taskDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();


            #endregion
        }
        if (charProf)
        {
            #region CHARACTERPROFILE_REMOVEDATA

            int charProfCount = masterCharacterProfList.Items.Count;
            characterProfDataArray = new CharacterProfile_Data[charProfCount];

            for (int i = 0; i < charProfCount; i++)
            {
                masterCharacterProfList.Items[i].isAvailable = false;
                characterProfDataArray[i].isTransportToAvailable = false;

            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_CharacterProfileData", 1));
            json = JsonHelper.ToJson<CharacterProfile_Data>(characterProfDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();


            #endregion
        }
        if (area)
        {
            #region AREA_REMOVEDATA

            int areaCount = masterAreaList.Items.Count;
            areaDataArray = new Area_Data[areaCount];

            for (int i = 0; i < areaCount; i++)
            {
                masterAreaList.Items[i].isAvailable = false;
                masterAreaList.Items[i].isShowing = false;

                areaDataArray[i].isAvailable = false;
                areaDataArray[i].isShowing = false;
                // characterProfDataArray[i].isTransportToAvailable = false;

            }


            file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterDescription + "_AreaData", 1));
            json = JsonHelper.ToJson<Area_Data>(areaDataArray, true);
            bf.Serialize(file, json);

            Debug.Log(json);
            file.Close();


            #endregion
        }



    }
    //public void CheckAndSaveHighScore(Points pointsToCheckAgainst)
    //{
    //    if (masterPlayerPoints.currentPlayerPoints.Value > pointsToCheckAgainst.Value)
    //    {
    //        pointsToCheckAgainst.Value = masterPlayerPoints.currentPlayerPoints.Value;
        
    //        SaveData(false, false, true);

             
    //    }


    //}
}
