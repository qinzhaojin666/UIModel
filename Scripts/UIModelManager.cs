using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIModelManager : MonoBehaviour
{
    [HideInInspector]
    public int normalUpY = 20;//每次新加相机时上升的y值
    private int currentUpY = 100;
    public const int CAM_DEPTH = 2;
    public const int CAM_LAYER = 5;
    //正在使用中的camera
    public Dictionary<string, Camera> modelCameras = new Dictionary<string, Camera>();
    public Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();
    public void PlaceModel(RawImage image, GameObject model, Vector3 offset)
    {
        PlaceModel(image, model, offset, Vector3.zero, Vector3.one);
    }

    public void PlaceModel(RawImage image, GameObject model, Vector3 offset, Vector3 ratation)
    {
        PlaceModel(image, model, offset, ratation, Vector3.one);
    }


    /// <summary>
    /// 放置模型,如果一个页面要放置相同的模型却不是一个texture,请令他们的name不一样
    /// </summary>
    /// <param name="model">需要显示的模型</param>
    /// <param name="offset">模型显示到场景上的偏移量</param>
    /// <param name="rotation">模型旋转</param>
    /// <param name="scale">模型缩放值</param>
    public void PlaceModel(RawImage image, GameObject model,Vector3 offset,Vector3 rotation, Vector3 scale)
    {
        Camera camera = null;

        if (modelCameras.ContainsKey(model.name))
        {
            camera = modelCameras[model.name];
        }
        else
        {
            camera = CreateModelCamera();
            modelCameras.Add(model.name, camera);
        }

        if (models.ContainsKey(model.name))
        {
            Destroy(models[model.name]);
            models[model.name] = model;
        }
        else
        {
            models.Add(model.name,model);
        }

        Vector3 pos = camera.transform.position;
        model.transform.position = new Vector3(pos.x + offset.x, pos.y + offset.y, offset.z);
        model.transform.localScale = scale;
        model.transform.Rotate(rotation.x, rotation.y, rotation.z);
        model.transform.parent = transform;
        model.layer = CAM_LAYER;
        model.SetActive(true);
        ChinarSmoothUi3DCamera csud = camera.GetComponent<ChinarSmoothUi3DCamera>();
        csud.pivot = model.transform;
        csud.pivotOffset = offset;
        csud.ResetPosition();
        csud.SetSlideImg(image);
        if (camera.targetTexture == null)
        {
            RenderTexture texture = new RenderTexture(256, 256, 1, RenderTextureFormat.ARGB32);
            texture.name = model.name;
            camera.targetTexture = texture;
        }

        image.texture = camera.targetTexture;
    }

    private Camera CreateModelCamera()
    {
        currentUpY += normalUpY;
        GameObject CameraObj = new GameObject("ModelCamera");
        CameraObj.transform.parent = transform;
        Camera cam = CameraObj.AddComponent<Camera>();
        cam.transform.localScale = Vector3.one ;
        cam.depth = CAM_DEPTH;
        cam.gameObject.layer = CAM_LAYER;
        cam.clearFlags = CameraClearFlags.SolidColor;
        CameraObj.AddComponent<ChinarSmoothUi3DCamera>();
        cam.transform.localPosition = new Vector3(0, currentUpY, CameraObj.GetComponent<ChinarSmoothUi3DCamera>().initialDistance);
        return cam;
    }

}