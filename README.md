# 성학십도 10도 소스

업로드한 SpriteMaker.cs 파일이 이미지를 처리하는 전체 파일입니다.

![image sequence](./image_seq.png)

InitTexture() 에서 이미지 초기화를 진행합니다.

기본 알고리즘 원리는 위 이미지처럼 영상을 이미지 시퀀스로 출력한 다음 겹치는 형태로 배치하였습니다.

```
colorArray = new Color[tex.width * tex.height];

srcArray = new Color[videoSeq.Length][];
```

이미지로 출력할 픽셀을 저장할 배열(colorArray), 원래 이미지 시퀀스에서 뽑아내는 픽셀값을 저장할 배열(srcArray) 두개가 있습니다.

두 개의 배열 모두 2차원 이미지 픽셀값을 1차원으로 Flatten 시켜서 저장합니다.

```
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
```

colorArray에는 텍스처에 적용할 첫번째 이미지의 픽셀값을 저장합니다.

srcArray에는 각 이미지 시퀀스의 픽셀값을 저장합니다.

```
tex.SetPixels(colorArray);
tex.Apply();
```

colorArray값으로 텍스처에 적용합니다.

private void OnTriggerStay(Collider other) 에서 collider와 collider가 만났을때 메인 collider의 (x,y,z) 좌표값을 알아냅니다.

UpdateImage()에서는 주어진 (x,y) 좌표와 z값으로 대응되는 이미지상에서의 위치와 이미지 프레임번호를 중심으로 원뿔 형태로 프레임 번호가 대응되게 됩니다.

```
if( zPixel > 0 && zPixel <videoSeq.Length)
{
    for (int x = 0; x < tex.width; x++)
    {
        for (int y = 0; y < tex.height; y++)
        {
            for (int z = 1; z < 50; z++)
            {
                if (z * z > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) && ((z - 1) * (z - 1)) < ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)))
                {
                    int pixelIndex = x + (y * tex.width);
                    colorArray[pixelIndex] = srcArray[zPixel - z][pixelIndex];
                }
            }

        }
    }
}
```

tex.width, tex.height 값 만큼 반복하면서 픽셀값을 업데이트 합니다.

```
if (z * z > ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)) && ((z - 1) * (z - 1)) < ((x + 0.5 - xPixel) * (x + 0.5 - xPixel) + (y + 0.5 - yPixel) * (y + 0.5 - yPixel)))
{
    int pixelIndex = x + (y * tex.width);
    colorArray[pixelIndex] = srcArray[zPixel - z][pixelIndex];
}
```

Loop 동안 원 그리는 알고리즘을 통해서 원 영역 안쪽은 해당 프레임의 픽셀값으로 변경합니다.



위와 같은 과정으로 이미지를 업데이트 합니다.

질문드리려고 하는 부분은 collider를 변경해서 좌표 변경시 이미지가 업데이트 될때 멈추는 현상이 발생이 됩니다.
(추가 : 150*150 크기까지는 부드럽게 되는데 이보다 커지면 collider가 움직일때 유니티 실행 자체가 멈추는 현상이 발생합니다.)

제가 생각하기에는 이미지 업데이트할 때 마다 배열 개수만큼 매번 반복하는 것 때문에 발생되는 현상일것으로 생각하고 있습니다.

아래 링크는 동작 샘플 영상입니다.

https://drive.google.com/file/d/1RHwx3srHs5kDrj9WmSpV0t5bi9HOwFWj/view?usp=sharing

잘 부탁드립니다.!! ^^

추가 설명]

좀더 부연설명을 드리자면, 저희가 현재 VR 상에서 아래 링크에서 보여드리는 "Khronos Projector"와 같이 짧은 영상을 유저가 자유롭게 콘트롤하며 들여다 보게 하고자 계획하고 있습니다.

https://www.alvarocassinelli.com/khronos-projector/

현재 저희가 준비한 영상은 약 15-20초 되는 영상이 준비되어 있는데요.
이 영상을 정사각형 형태로 잘라 사용하고 있고요. 현재 150x150 pixel로 코딩을 짰는데, 해상도를 높여 구동하려니 속도가 현저히 저하되는 현상이 발생했습니다.

C#으로 코딩하고 있는데 이를 좀더 크로노스 프로젝터의 움직임과 유사하게 하면 더 좋구요.
(물론 실제 크로노스 프로젝터는 실물 천 스크린에 영상을 투사하고 있지만요. 이를 VR 안에서 구현해보고자 하는 것입니다).
또한 속도 저하 문제도 해결할 수 있는 알고리즘을 짤 수 있을지도 도움을 받으면 좋을 듯 합니다.


