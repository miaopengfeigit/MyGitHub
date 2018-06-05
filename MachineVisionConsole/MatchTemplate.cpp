// MachineVisionConsole.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc.hpp>  
#include<vector>
using namespace cv;
using namespace std;

int main()
{
	//Mat img, templ, result;
	//img = imread("C:\\Users\\surface laptop\\Desktop\\untitled.png");
	//templ = imread("C:\\Users\\surface laptop\\Desktop\\untitled1.png");

	//int result_cols = img.cols - templ.cols + 1;
	//int result_rows = img.rows - templ.rows + 1;
	//result.create(result_cols, result_rows, CV_32FC1);

	//matchTemplate(img, templ, result, CV_TM_SQDIFF_NORMED);//这里我们使用的匹配算法是标准平方差匹配 method=CV_TM_SQDIFF_NORMED，数值越小匹配度越好
	//normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());

	//double minVal = -1;
	//double maxVal;
	//Point minLoc;
	//Point maxLoc;
	//Point matchLoc;
	//cout << "匹配度：" << minVal << endl;
	//minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());


	//cout << "匹配度：" << minVal << endl;

	//matchLoc = minLoc;

	//rectangle(img, matchLoc, Point(matchLoc.x + templ.cols, matchLoc.y + templ.rows), Scalar(0, 255, 0), 2, 8, 0);

	//imshow("img", img);

	Mat Img = imread("C:\\Users\\surface laptop\\Desktop\\2.bmp", -1);
	int depth = Img.depth();
	int channels = Img.channels();
	IplImage* p = cvCreateImage(cvSize(Img.cols, Img.rows), IPL_DEPTH_8U, channels);
	cvSetData(p, Img.data, Img.step);
	
	IplImage* pp = &IplImage(Img);
	IplImage* ppp = cvCloneImage(pp);

	//Img.isContinuous
	if(Img.isContinuous())
	{ 
		cout << "isContinuous：true" << endl;
	}
	else
	{
		cout << "isContinuous：false" << endl;
	}

	cvShowImage("Imagep", p);
	cvShowImage("Imagepp", ppp);

	//imshow("img", p);
	waitKey(0);
	return 0;
}

