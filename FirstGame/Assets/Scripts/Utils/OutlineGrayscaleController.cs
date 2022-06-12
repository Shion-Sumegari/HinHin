using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineGrayscaleController : MonoBehaviour
{
    [SerializeField] [Range(0, 255)] int alpha;
    Image currentSprite;
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSprite.material.HasProperty("_SolidOutline"))
        {
            Color color = currentSprite.material.GetColor("_SolidOutline");
            currentSprite.material.SetColor("_SolidOutline", new Color(color.r, color.b, color.g, alpha / 255f));
        }
    }
}
