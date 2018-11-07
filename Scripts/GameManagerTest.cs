using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//https://blog.csdn.net/ChinarCSDN/article/details/81058773#project-项目文件

public class GameManagerTest : MonoBehaviour {

    public GameObject[] ModelTests;
    public UIModelManager uIModelManager;
    public RawImage image;
    public int index;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ButtonNextClick()
    {
        index++;
        index = index % ModelTests.Length;
        GameObject go = Instantiate(ModelTests[index]);
        uIModelManager.PlaceModel(image, go, new Vector3(0, -0.4f, 0), new Vector3(0, 180, 0));
    }


    public void ButtonLastClick()
    {
        index--;
        if (index == -1)
        {
            index = 2;
        }
        index = index % ModelTests.Length;
        GameObject go = Instantiate(ModelTests[index]);
        uIModelManager.PlaceModel(image, go, new Vector3(0, -0.4f, 0), new Vector3(0, 180, 0));
    }

    public void ModelClick()
    {
        Debug.Log("模型被点击了。。。。");
    }
}
