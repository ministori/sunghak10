using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
public class SpriteMaker : MonoBehaviour
{
    // sprite object
    public GameObject sprite;
    SpriteRenderer spriteRend;

    // texture array
    //public Texture2D[] layers;
    public Texture2D[] videoSeq;

    Texture2D tex;

    bool outCol = true;

    // pixel array
    Color[] colorArray;
    Color[][] srcArray;

    void Start()
    {
        spriteRend = sprite.GetComponent<SpriteRenderer>();

        ReadTextureSrc();

        InitImage();

    }

    private void Update()
    {
        if (outCol)
        {
            Debug.Log("outoutout");
        }
    }

    void ReadTextureSrc()
    {
        //for (int i = 0; i < layers.Length; i++)
        //{
        //    layers[i] = (Texture2D)Resources.Load(transform.name + "/" + transform.name + "_" + i);
        //}

        videoSeq = (Texture2D[])Resources.LoadAll<Texture2D>(transform.name);
        Debug.Log(videoSeq.Length);
    }

    void InitImage()
    {
        InitTexture();

        MakeSprite();

        //gameObject.AddComponent<BoxCollider2D>();
    }

    void InitTexture()
    {
        tex = new Texture2D(videoSeq[0].width, videoSeq[0].height);

        colorArray = new Color[tex.width * tex.height];

        srcArray = new Color[videoSeq.Length][];

        for (int i = 0; i < videoSeq.Length; i++)
        {
            srcArray[i] = videoSeq[i].GetPixels();
        }

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                int pixelIndex = x + (y * tex.width);

                colorArray[pixelIndex] = srcArray[0][pixelIndex];
            }
        }

        tex.SetPixels(colorArray);
        tex.Apply();

        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
    }
    

    void MakeSprite()
    {
        // create a sprite from that texture
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0f);

        // assign our procedural sprite to rend.sprite
        spriteRend.sprite = newSprite;
    }

    // 2d axis - get coordinate when click
    /*
    Vector2 GetImageCoordinate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition = transform.InverseTransformPoint(mousePosition);

        int xPixel = Mathf.RoundToInt(mousePosition.x * 100);

        int yPixel = Mathf.RoundToInt(mousePosition.y * 100);

        Debug.Log(xPixel + " , " + yPixel);

        return new Vector2(xPixel, yPixel);
    }
    */



    void UpdateImage(int xPixel, int yPixel, int zPixel)
    {
        //Vector2 imageCoordinate = GetImageCoordinate();

        if( zPixel > 0 && zPixel <videoSeq.Length)
        {
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    for (int z = 1; z < 50; z++)
                    {
                        if (z * z > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) && ((z - 1) * (z - 1)) < ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)))
                        //if (100 * 100 > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) )
                        {
                            int pixelIndex = x + (y * tex.width);
                            colorArray[pixelIndex] = srcArray[zPixel - z][pixelIndex];
                        }
                    }

                }
            }
        }
        

        tex.SetPixels(colorArray);
        tex.Apply();

    }

    private void OnMouseUp()
    {
        //UpdateImage();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Vector3 planePosition = transform.position;
        Vector3 colliderPosition = other.transform.position;
        Vector3 planeScale = transform.localScale;

        Vector3 colliderZeroPos = colliderPosition - (planePosition - planeScale / 2);

        Debug.Log(colliderZeroPos);

        int xPixel = (int)RangeMap(colliderZeroPos.x, 0.0f, planeScale.x, 0.0f, 1024f);
        int yPixel = (int)RangeMap(colliderZeroPos.y, 0.0f, planeScale.y, 0f, 1024f);

        Debug.Log("(" + xPixel + "," + yPixel + ")");

        UpdateImage(xPixel, yPixel);

    }
    */

    private void OnTriggerStay(Collider other)
    {
        Vector3 planePosition = transform.position;
        Vector3 planeLocalPosition = transform.localPosition;

        Vector3 colliderPosition = other.transform.position;
        Vector3 colliderLocalPosition = other.transform.localPosition;

        Vector3 planeScale = transform.localScale;

        Debug.Log("o" + colliderPosition + ", " + colliderLocalPosition);

        //Debug.Log("cp" + colliderPosition + "pp" + planePosition);
        //Debug.Log("Lcp" + colliderLocalPosition + "Lpp" + planeLocalPosition);

        Vector3 colliderZeroPos = colliderPosition - (planePosition - planeScale / 2);

        Vector3 colliderLocalZeroPos = colliderLocalPosition - (planeLocalPosition - planeScale / 2);

        //Debug.Log("z" + colliderZeroPos + ", " + colliderLocalZeroPos);

        outCol = false;

        int xPixel = (int)RangeMap(colliderZeroPos.x, 0.0f, planeScale.x, 0f, videoSeq[0].width);
        int yPixel = (int)RangeMap(colliderZeroPos.y, 0.0f, planeScale.y, 0f, videoSeq[0].height);
        int zPixel = (int)RangeMap(colliderZeroPos.z, 0.0f, planeScale.z, 0f, videoSeq.Length);

        int xPixel2 = (int)RangeMap(colliderLocalZeroPos.x, 0.0f, planeScale.x, 0f, videoSeq[0].width);
        int yPixel2 = (int)RangeMap(colliderLocalZeroPos.y, 0.0f, planeScale.y, 0f, videoSeq[0].height);
        int zPixel2 = (int)RangeMap(colliderLocalZeroPos.z, 0.0f, planeScale.z, 0f, videoSeq.Length);

        if(zPixel2 >= 0 && zPixel2 < videoSeq.Length)
        {
            //Debug.Log("map1(" + xPixel + "," + yPixel + "," + zPixel + ")");
            //Debug.Log("map2(" + xPixel2 + "," + yPixel2 + "," + zPixel2 + ")");

            UpdateImage(xPixel2, yPixel2, zPixel2);
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        outCol = true;
    }

    public static float RangeMap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromAbs = value - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }

}
