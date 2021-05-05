using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "CustomSO/PlayerData/Task")]
public class Task : ScriptableObject {

	public string taskDescription;
    public string localizationKeyForDescription;
    public string playerPrefs_SaveName;
	public Task_Status taskStatus;

    public void SetTaskStatus(Task_Status tS)
    {
        taskStatus = tS;
    }
    public void SetTaskToComplete() {

        taskStatus = Task_Status.COMPLETED;
    }

}
