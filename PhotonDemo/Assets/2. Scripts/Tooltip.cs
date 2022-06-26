using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public Text headerField;
    public Text contentField;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();    
    }
    public void SetText(string content, string header = "")
    {
        if(string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;
    }
    // Update is called once per frame
    void Update()
    {
        if(Application.isEditor)
        {
            int headerLegnth = headerField.text.Length;
            int contentLegnth = contentField.text.Length;

            layoutElement.enabled = (headerLegnth > characterWrapLimit || contentLegnth > characterWrapLimit) ? true : false;

        }
        Vector2 position = Input.mousePosition; // 위치 마우스 따라다니기

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);


        transform.position = position;


    }
}
