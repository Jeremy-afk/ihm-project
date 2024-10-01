using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;   // Reference to TextMeshProUGUI component
    [SerializeField]
    private float speed = 1f;       // Speed of color change
    [SerializeField]
    private float saturation = 1f;  // Saturation level (0 to 1)
    [SerializeField]
    private float hueRange = 1f;    // Range of hues to use (0 to 1)
    [SerializeField]
    private float phaseShift = 0f;  // Additional phase shift for hue cycling


    void Update()
    {
        if (text == null)
        {
            return;
        }

        text.ForceMeshUpdate();  // Force update of TextMeshPro mesh data

        // Loop through each character in the text
        for (int i = 0; i < text.textInfo.characterCount; i++)
        {
            if (text.textInfo.characterInfo[i].isVisible)
            {
                // Calculate the base hue for the current character
                float baseHue = (Time.time * speed + (float)i / text.textInfo.characterCount + phaseShift) % 1.0f;

                // Adjust for negative speed (wrap hue around gracefully)
                if (baseHue < 0)
                {
                    baseHue += 1.0f;
                }

                // Map hue to the user-defined range (e.g., from 0 to hueRange)
                float hue = Mathf.Lerp(0f, hueRange, baseHue);

                // Convert HSV to RGB with the defined saturation and full brightness (value = 1)
                Color color = Color.HSVToRGB(hue, saturation, 1f);

                // Apply color to the character vertices
                var meshInfo = text.textInfo.meshInfo[text.textInfo.characterInfo[i].materialReferenceIndex];
                int vertexIndex = text.textInfo.characterInfo[i].vertexIndex;

                // Apply the color to each vertex of the character
                for (int j = 0; j < 4; j++)
                {
                    meshInfo.colors32[vertexIndex + j] = color;
                }
            }
        }

        // Update the mesh to apply the color changes
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
