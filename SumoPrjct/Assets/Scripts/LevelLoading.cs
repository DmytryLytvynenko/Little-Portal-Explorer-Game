using UnityEngine;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] private Transform CheckPoints;
    private void Start()
    {
        ActivateCheckPointOnLoad();
        GlobalData.PlayerInstance.DeactivateDeformation();
        SetPlayerPosition();
        GlobalData.PlayerInstance.ActivateDeformation();
    }

    private void SetPlayerPosition()
    {
        if (PlayerPrefs.HasKey("CheckPointPosition"))
            Utilities.Utilities.SetPlayerPosition();
        else
            Utilities.Utilities.SetPlayerPosition(GlobalData.DefaultCheckPoint);
    }

    private void ActivateCheckPointOnLoad() 
    {
        if (!PlayerPrefs.HasKey("CheckPointName")) return;

        for (int i = 0; i < CheckPoints.childCount; i++)
        {
            Debug.Log($"Check Point: {CheckPoints.GetChild(i).name} Save:{PlayerPrefs.GetString("CheckPointName")}\n");
            if (CheckPoints.GetChild(i).name == PlayerPrefs.GetString("CheckPointName"))
            {
                CheckPoints.GetChild(i).GetComponent<CheckPoint>().ActivateCheckPointOnLoad();
            } 
        }
    }
}
