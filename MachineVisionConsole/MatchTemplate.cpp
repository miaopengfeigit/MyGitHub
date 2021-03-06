#include "stdafx.h"
#include <iostream>
#include<opencv2/opencv.hpp>
#include<vector>

using namespace std;
using namespace cv;

//定义了窗体名
string winName = "二值化";

//TrackBar发生改变的回调函数
void onChangeTrackBar(int pos, void* userdata);

//主函数
int main2()
{
	//trackbar名
	string trackBarName = "pos";
	//图像文件名
	string imgName = "E:\\Image\\1\\1.bmp";
	//trackbar的值
	int posTrackBar = 0;
	//trackbar的最大值
	int maxValue = 255;

	Mat img;
	//读入图像，以灰度图形式读入
	img = imread(imgName, 0);
	//新建窗口
	namedWindow(winName);
	imshow(winName, img);
	//创建trackbar，我们把img作为数据传进回调函数中
	createTrackbar(trackBarName, winName, &posTrackBar, maxValue, onChangeTrackBar, &img);
	createTrackbar("2", winName, &posTrackBar, maxValue, onChangeTrackBar, &img);
	waitKey();
	return 0;
}

// 回调函数
void onChangeTrackBar(int pos, void* usrdata)
{
	// 强制类型转换
	Mat src = *(Mat*)(usrdata);

	Mat dst;
	// 二值化
	threshold(src, dst, pos, 255, 0);

	imshow(winName, dst);
}