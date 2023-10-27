using System;
using UnityEngine;

namespace Utilities
{
    public class Utilities
    {
        public static void SetPlayerPosition() 
        {
            string[] currentCheckPointString = PlayerPrefs.GetString("CheckPointPosition").Split(".");
            Vector3 currentCheckPoint = new Vector3
                (
                    Convert.ToSingle(currentCheckPointString[0]),
                    Convert.ToSingle(currentCheckPointString[1]),
                    Convert.ToSingle(currentCheckPointString[2])
                );

            if (currentCheckPoint != null)
                GlobalData.PlayerInstance.transform.position = currentCheckPoint;
        }
        public static void SetPlayerPosition(Vector3 position)
        {
            GlobalData.PlayerInstance.transform.position = position;
        }
    }
}

