﻿using UnityEngine;
using UnityEngine.UI;

public class UICooldown : MonoBehaviour
{
    public Image mask; // Hình ảnh dùng làm thanh cooldown
    public Image cool;
    private float originalSize; // Kích thước ban đầu của thanh cooldown

    // Singleton instance
    public static UICooldown instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        originalSize = cool.rectTransform.rect.width;
    }

    public void SetValue(float value)

    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        mask.rectTransform.anchoredPosition = new Vector2((originalSize * (value - 1)) / 2, mask.rectTransform.anchoredPosition.y);
    }

    public void Show()
    {
        cool.gameObject.SetActive(true);
    }

    public void Hide()
    {
        cool.gameObject.SetActive(false);
    }
}
