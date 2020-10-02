# 성학십도 10도 소스

업로드한 cs 파일이 이미지를 처리하는 전체 파일입니다.

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

제가 생각하기에는 이미지 업데이트할 때 마다 배열 개수만큼 매번 반복하는 것 때문에 발생되는 현상일것으로 생각하고 있습니다.

잘 부탁드립니다. ^^




