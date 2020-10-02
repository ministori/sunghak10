# 성학십도 10도 소스

업로드한 cs 파일이 이미지를 처리하는 전체 파일입니다.

![image sequence](./image_seq.png)

기본 알고리즘 원리는 위 이미지처럼 영상을 이미지 시퀀스로 출력한 다음 겹치는 형태로 배치하였습니다.

~~~

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

~~~

그리고 이미지로 출력할 픽셀을 저장할 배열(colorArray), 원래 이미지 시퀀스에서 뽑아내는 픽셀값을 저장할 배열(srcArray) 두개를 선언했습니다.



처음에 이미지의 가로,세로 길이값으로 1차원 배열로 Flatten 시킨후 각 픽셀좌표의 color 값을 배열에 저장합니다.


그래서 collider 와 collider가 만났을 때 한쪽 collider의 (x,y)좌표값을 받아서 2D 이미지 상의 대응되는 (x,y)좌표를 계산하고, z좌표값으로는 깊이값으로 계산을 해서 이미지 시퀀스의 프레임 번호에 대응하도록 하였습니다.

그리고 collider의 좌표가 변할 때 그에 대응되는 이미지 좌표와 프레임 번호가 대응이 될때 (x,y) 좌표를 중심으로 원뿔 형태로 프레임 번호가 대응되게 됩니다.

이미지로 형상화하는 과정에서는




