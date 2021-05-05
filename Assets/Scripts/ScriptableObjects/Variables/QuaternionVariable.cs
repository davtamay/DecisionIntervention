using UnityEngine;

[CreateAssetMenu (fileName = "QuaternionValue", menuName = "CustomSO/Variables/Quaternion")]
public class QuaternionVariable : ScriptableObject {

	#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
	#endif
	public Quaternion Value;

	public void SetValue(Quaternion value)
	{
		Value = value;
	}

	public void SetValue(QuaternionVariable value)
	{
		Value = value.Value;
	}

	public void ApplyChange(Quaternion amount)
	{
		Value *= amount;
	}

	public void ApplyChange(QuaternionVariable amount)
	{
		Value *= amount.Value;
	}


	public static implicit operator Quaternion(QuaternionVariable reference){

		return reference.Value;
	}
}

