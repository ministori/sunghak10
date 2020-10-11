// README:
// - 변경 사항의 가독성을 위해 기존의 주석과 불필요한 블록을 지웠습니다.
// - 변경된 부분은 숫자를 달아 표기하였습니다.
// - 변경된 기존 코드는 주석 처리를 하여 어느 부분이 어떻게 변경되었는지 알 수 있도록 하였습니다.

using UnityEngine;

// 1. SpriteMaker는 연산 작업이 많은 무거운 클래스이므로 씬 전체에 걸쳐 하나의 컴포넌트로만(singleton처럼) 사용되어야 합니다.
// 이미 그렇게 사용 중이신 것 같으나, 실수하기 쉬운 부분으로, 잘못 사용한다면 성능에 큰 영향을 끼치게 되기 때문에 적습니다.
public class SpriteMaker : MonoBehaviour
{
    public GameObject sprite;
    SpriteRenderer spriteRend;

    public Texture2D[] videoSeq;

    Texture2D tex;

    bool outCol = true;

    Color[] colorArray;
    Color[][] srcArray;

    // transform
    public Transform interactiveCollider;
    public Transform coverCollider;

    void Start()
    {
        spriteRend = sprite.GetComponent<SpriteRenderer>();

        ReadTextureSrc();

        InitImage();

    }

    private void OnDestroy()
    {
        // 2. Resources를 통해 로드한 어셋은 사용 후 메모리에서 해제해야 합니다.
        Resources.UnloadUnusedAssets();

        // 3. 생성자를 통해 생성한 텍스쳐도 사용 후 메모리에서 해제해야 합니다.
        Destroy(tex);
    }

    private void Update()
    {
        if (outCol)
        {
            //Debug.Log("outoutout");
        }

        coverCollider.transform.position = interactiveCollider.transform.position;
    }

    void ReadTextureSrc()
    {
        videoSeq = (Texture2D[])Resources.LoadAll<Texture2D>(transform.name);
        //Debug.Log(videoSeq.Length);
    }

    void InitImage()
    {
        InitTexture();

        MakeSprite();
    }

    void InitTexture()
    {
        tex = new Texture2D(videoSeq[0].width, videoSeq[0].height);

        // 4. 아래 몇 줄에 걸친 colorArray의 초기화 과정을 GetPixels() 메소드로 대체 가능합니다.
        // colorArray = new Color[tex.width * tex.height];
        colorArray = videoSeq[0].GetPixels();

        srcArray = new Color[videoSeq.Length][];

        for (int i = 0; i < videoSeq.Length; i++)
        {
            srcArray[i] = videoSeq[i].GetPixels();
        }

        // 4. 위의 이유로 삭제 가능합니다.
        // for (int x = 0; x < tex.width; x++)
        // {
        //     for (int y = 0; y < tex.height; y++)
        //     {
        //         int pixelIndex = x + (y * tex.width);
        //
        //         colorArray[pixelIndex] = srcArray[0][pixelIndex];
        //     }
        // }

        tex.SetPixels(colorArray);
        tex.Apply();

        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
    }

    void MakeSprite()
    {
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0f);

        spriteRend.sprite = newSprite;
    }

    void UpdateImage(int xPixel, int yPixel, int zPixel)
    {
        // 4. 아래 블록 전체를 수정하였습니다.
        // 4-1. 메소드를 호출하는 로직에서 zPixels이 0 이상 videoSeq.Length - 1 이하가 보장된다면, if 문을 삭제 가능합니다.
        // if( zPixel > 0 && zPixel <videoSeq.Length)
        // {
        //     for (int x = 0; x < tex.width; x++)
        //     {
        //         for (int y = 0; y < tex.height; y++)
        //         {
        //             for (int z = 1; z < 50; z++)
        //             {
        //                 if (z * z > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) && ((z - 1) * (z - 1)) < ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)))
        //                 {
        //                     int pixelIndex = x + (y * tex.width);
        //                     colorArray[pixelIndex] = srcArray[zPixel - z][pixelIndex];
        //                 }
        //             }
        //
        //         }
        //     }
        // }

        // 4-2. 이중 for 문을 단일 for 문으로 변경하여 성능 최적화가 가능합니다.
        var size = tex.width * tex.height;
        int z = 0;

        for (int i = 0; i < 30; i++)
        {
            //var x = size % tex.width;
            //var y = size / tex.width;

            var x = i % tex.width;
            var y = i / tex.width;
            
            // 4-3. z 값을 구하기 위한 for 문을 생략 가능합니다.
            z = (int)Mathf.Sqrt((x - xPixel) * (x - xPixel) + (y - yPixel) * (y - yPixel)); // z 값을 직접 구하고,
            z = Mathf.Min(z, zPixel); // zPixel을 넘지 않도록 해줍니다.
            colorArray[i] = srcArray[zPixel - z][i];

            //for (z = 1; z < 50; z++)
            //{
            //    
            //    if (z * z > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) && ((z - 1) * (z - 1)) < ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)))
            //    {
            //        int pixelIndex = x + (y * tex.width);
            //        colorArray[pixelIndex] = srcArray[zPixel - z][pixelIndex];
            //    }
            //}

            Debug.Log(x + " : " + y + " : " + z);

        }

        

        tex.SetPixels(colorArray);
        tex.Apply();
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.name == "coord")
        //{

            // 5. localScale을 사용하기 때문에 localPosition을 사용해야 합니다. 글로벌 position과 관련된 코드는 주석처리 하였습니다.
            // Vector3 planePosition = transform.position;
            Vector3 planeLocalPosition = transform.localPosition;

            // Vector3 colliderPosition = other.transform.position;
            Vector3 colliderLocalPosition = other.transform.localPosition;

            Vector3 planeScale = transform.localScale;

            // Debug.Log("o" + colliderPosition + ", " + colliderLocalPosition);

            // Vector3 colliderZeroPos = colliderPosition - (planePosition - planeScale / 2);

            Vector3 colliderLocalZeroPos = colliderLocalPosition - (planeLocalPosition - planeScale / 2);

            outCol = false;

            // int xPixel = (int)RangeMap(colliderZeroPos.x, 0.0f, planeScale.x, 0f, videoSeq[0].width);
            // int yPixel = (int)RangeMap(colliderZeroPos.y, 0.0f, planeScale.y, 0f, videoSeq[0].height);
            // int zPixel = (int)RangeMap(colliderZeroPos.z, 0.0f, planeScale.z, 0f, videoSeq.Length);

            // 6-1. toMax 값에 각각 1을 빼줘야 합니다.
            // 6-2. 아래 8번의 이유로 RangeMap 메소드를 ClampedMap 메소드로 변경하였습니다.
            // int xPixel2 = (int)RangeMap(colliderLocalZeroPos.x, 0.0f, planeScale.x, 0f, videoSeq[0].width);
            // int yPixel2 = (int)RangeMap(colliderLocalZeroPos.y, 0.0f, planeScale.y, 0f, videoSeq[0].height);
            // int zPixel2 = (int)RangeMap(colliderLocalZeroPos.z, 0.0f, planeScale.z, 0f, videoSeq.Length);
            int xPixel2 = ClampedMap(colliderLocalZeroPos.x, 0.0f, planeScale.x, 0, videoSeq[0].width - 1);
            int yPixel2 = ClampedMap(colliderLocalZeroPos.y, 0.0f, planeScale.y, 0, videoSeq[0].height - 1);
            int zPixel2 = ClampedMap(colliderLocalZeroPos.z, 0.0f, planeScale.z, 0, videoSeq.Length - 1);

            // 7. 8번에서 clamping을 하였으므로 범위에 대한 조건을 검사할 필요가 없습니다.
            // if(zPixel2 >= 0 && zPixel2 < videoSeq.Length)
            // {
            UpdateImage(xPixel2, yPixel2, zPixel2);
            // }

        //}

    }

    private void OnTriggerExit(Collider other)
    {
        outCol = true;
    }

    // 8. RangeMap 메소드를 아래와 같은 이유로 ClampedMap드 메소드로 변경하였습니다.
    // public static float RangeMap(float value, float fromMin, float fromMax, float toMin, float toMax)
    // {
    //     float fromAbs = value - fromMin;
    //     float fromMaxAbs = fromMax - fromMin;
    //
    //     float normal = fromAbs / fromMaxAbs;
    //
    //     float toMaxAbs = toMax - toMin;
    //     float toAbs = toMaxAbs * normal;
    //
    //     float to = toAbs + toMin;
    //
    //     return to;
    // }

    // 8-1. 픽셀은 int 형식으므로, int 형식으로 리턴합니다.
    // 8-2. 같은 이유로 toMin과 toMax도 int 형식으로 변경하였습니다.
    // 8-3. 리턴 값이 toMin과 toMax 범위를 넘어가지 않도록 하였습니다.
    private static int ClampedMap(float value, float fromMin, float fromMax, int toMin, int toMax)
    {
        return Mathf.Clamp((int)(toMin + (value - fromMin) / (fromMax - fromMin) * (toMax - toMin)), toMin, toMax);
    }
}