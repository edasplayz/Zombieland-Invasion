using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasModifier : MonoBehaviour
{
    public Image crosshairDot;
    public Color crosshairColor = Color.white;
    public float gapSize = 10f;
    public float lineWidth = 2f;

    private RectTransform[] crosshairLines = new RectTransform[4];

    private void Start()
    {
        CreateCrosshair();
    }

    private void CreateCrosshair()
    {
        // Set the crosshair color
        crosshairDot.color = crosshairColor;

        // Clear existing lines
        for (int i = 0; i < 4; i++)
        {
            if (crosshairLines[i] != null)
            {
                Destroy(crosshairLines[i].gameObject);
            }
        }

        // Create horizontal lines
        CreateLine(Vector2.up);
        CreateLine(Vector2.down);

        // Create vertical lines
        CreateLine(Vector2.left);
        CreateLine(Vector2.right);
    }

    private void CreateLine(Vector2 direction)
    {
        GameObject lineObject = new GameObject("Line");
        RectTransform line = lineObject.AddComponent<RectTransform>();
        line.SetParent(transform);
        line.anchoredPosition = Vector2.zero;
        line.sizeDelta = new Vector2(lineWidth, gapSize);
        line.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        line.localScale = new Vector3(1, 1, 1);
        line.GetComponent<Image>().color = crosshairColor;

        // Store the line in the array for later removal
        if (direction == Vector2.up)
        {
            crosshairLines[0] = line;
        }
        else if (direction == Vector2.down)
        {
            crosshairLines[1] = line;
        }
        else if (direction == Vector2.left)
        {
            crosshairLines[2] = line;
        }
        else if (direction == Vector2.right)
        {
            crosshairLines[3] = line;
        }
    }

    public void UpdateGapSize(float newGapSize)
    {
        gapSize = newGapSize;
        CreateCrosshair();
    }

    public void UpdateLineWidth(float newLineWidth)
    {
        lineWidth = newLineWidth;
        CreateCrosshair();
    }
}
