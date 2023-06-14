using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public GameObject canvas;
    public List<GameObject> stateList = new List<GameObject>();
    public string currentStateName;
    public void ChangeState(string stateName)
    {
        for(int i = 0; i < stateList.Count; i++)
        {
            if (stateList[i].name == stateName)
            {
                stateList[i].SetActive(true);
                currentStateName = stateName;
            }
            else
            {
                stateList[i].SetActive(false);
            }
        }   
    }
}
