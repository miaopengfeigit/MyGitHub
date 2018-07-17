#include "stdafx.h"

#define DLL_API extern "C" _declspec(dllexport)     
#include <opencv2\opencv.hpp>  
#include <iostream>
#include <sstream>
#include <string>
using namespace std;
using namespace cv;

VideoCapture capture;
Mat image, imageROI;

template<typename T> String toString(const T& t) 
{
	ostringstream oss;  //创建一个格式化输出流
	oss << t;             //把值传递如流中
	return oss.str();
}

DLL_API IplImage _stdcall ReadImage(char* fileName)
{
    image = imread(fileName, IMREAD_UNCHANGED);
	return IplImage(image);
}

DLL_API bool _stdcall OpenCamera(int id)
{
	if (capture.open(id))
	{
		capture.set(CV_CAP_PROP_FRAME_WIDTH, capture.get(CV_CAP_PROP_FRAME_WIDTH));
		capture.set(CV_CAP_PROP_FRAME_HEIGHT, capture.get(CV_CAP_PROP_FRAME_HEIGHT));
		return true;
	}
	else
	{ 
		return false;
	}
}

DLL_API void _stdcall ReleaseCamera()
{
	 capture.release();
}

DLL_API double _stdcall GetFps()
{
	return capture.get(CV_CAP_PROP_FPS);
}

DLL_API IplImage  _stdcall ReadFrame()
{
	capture >> image;
	return IplImage(image);
}

DLL_API IplImage  _stdcall ReadROI(int x, int y, int width , int height)
{
	imageROI = image(Rect(x, y, width, height)).clone();
	return IplImage(imageROI);
}

DLL_API IplImage  _stdcall MatchTemplate()
{
	Mat result;
	capture >> image;

	int result_cols = image.cols - imageROI.cols + 1;
	int result_rows = image.rows - imageROI.rows + 1;
	result.create(result_cols, result_rows, CV_32FC1);

	matchTemplate(image, imageROI, result, CV_TM_SQDIFF_NORMED);//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
	normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());

	double minVal = -1;
	double maxVal;
	Point minLoc;
	Point maxLoc;
	Point matchLoc;
	//cout << "匹配度：" << minVal << endl;
	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());


	//cout << "匹配度：" << minVal << endl;
	if (1 > 0.8)
	{
		matchLoc = minLoc;
		rectangle(image, matchLoc, Point(matchLoc.x + imageROI.cols, matchLoc.y + imageROI.rows), Scalar(0, 255, 0), 1, 8, 0);
		
		putText(image, toString(maxVal), Point(matchLoc.x + imageROI.cols, matchLoc.y + imageROI.rows), FONT_HERSHEY_SCRIPT_COMPLEX, 1, Scalar::all(255));
	}
	
	return IplImage(image);
}


