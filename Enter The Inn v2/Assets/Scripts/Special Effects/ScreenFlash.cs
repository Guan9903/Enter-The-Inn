using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    //public SpriteRenderer img;
    public Image img;
    public float time;
    public Color flashColor;

    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        //img = FindObjectOfType<Canvas>().transform.Find("Screen Flash").GetComponent<Image>();
        defaultColor = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlashScreen()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        img.color = flashColor;
        //Debug.Log(flashColor);

        yield return new WaitForSeconds(time);

        if (GetComponent<PlayerManager>().playerHealth > 0)
        {
            img.color = defaultColor;
        }
    }
}
