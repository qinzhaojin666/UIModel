using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ChinarSmoothUi3DCamera : MonoBehaviour {

    public Transform pivot;
    public Vector3 pivotOffset = Vector3.zero;
    public Transform target;
    public float initialDistance = 2f;
    public float distance ;
    public float minDistance = 2f;
    public float maxDistance = 15f;
    public float zoomSpeed = 1f;
    public float xSpeed = 250.0f;
    public float ySpeed = 250.0f;
    public bool allowYTilt = false;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    private float x = 0.0f;
    private float y = 0.0f;
    private float targetX = 0f;
    private float targetY = 0f;
    public float targetDistance;
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    private float zoomVelocity = 1f;
    private RawImage slideImg;//用于滑动范围的图片
    private float dragX = 0.0f;

    private void Start()
    {
        var angles = transform.eulerAngles;
        targetX = x = angles.x;
        targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit);
        targetDistance = distance = initialDistance;
    }


    private void LateUpdate()
    {
        if (!pivot || slideImg == null) return;
       
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.0f) targetDistance -= zoomSpeed;
        else if (scroll < 0.0f)
            targetDistance += zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        //if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        //{
        //    targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        //    if (allowYTilt)
        //    {
        //        targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        //        targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
        //    }
        //}
        
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.1f);
        y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.1f) : targetY;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        if (Mathf.Abs(distance - targetDistance) >= 0.001)
        {
            distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);
        }
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position - pivotOffset;
        transform.rotation = rotation;
        transform.position = position;

    }

    public void ResetPosition()
    {
        targetDistance = distance = initialDistance;
        targetX = x = 0;
        targetY = y = 0;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position - pivotOffset;
        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetSlideImg(RawImage image)
    {
        slideImg = image;
        AddTriggersListener(slideImg.gameObject, EventTriggerType.BeginDrag, BeginDrag);
        AddTriggersListener(slideImg.gameObject, EventTriggerType.Drag, OnDrag);
    }

    public static void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    public void BeginDrag(BaseEventData eventData)
    {
        PointerEventData pointEventData = (PointerEventData)eventData;
        dragX = pointEventData.position.x;
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData pointEventData = (PointerEventData)eventData;
        float currentX = pointEventData.position.x;
        float deltaX = currentX - dragX;
        Debug.Log("deltaX:" + deltaX);
        targetX += deltaX;
        dragX = currentX;
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
