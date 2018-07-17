#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <stdlib.h>
#include <stdio.h>

using namespace cv;

/// ȫ�ֱ���

Mat src, src_gray;
Mat dst, detected_edges;

int edgeThresh = 1;
int lowThreshold;
int size = 3;
int const max_lowThreshold = 100;
int ratio = 3;
int kernel_size = 3;
char* window_name = "Edge Map";

/**
* @���� CannyThreshold
* @��飺 trackbar �����ص� - Canny��ֵ�������1:3
*/
void CannyThreshold(int, void*)
{
	/// ʹ�� 3x3�ں˽���
	blur(src_gray, detected_edges, Size(size, size));

	/// ����Canny����
	Canny(detected_edges, detected_edges, lowThreshold, lowThreshold*ratio, kernel_size);

	/// ʹ�� Canny���������Ե��Ϊ������ʾԭͼ��
	//dst = Scalar::all(0);

	//src.copyTo(dst, detected_edges);
	imshow(window_name, detected_edges);
}


/** @���� main */
int maincannny(int argc, char** argv)
{
	/// װ��ͼ��
	src = imread("C:\\1.bmp");

	if (!src.data)
	{
		return -1;
	}

	/// ������srcͬ���ͺʹ�С�ľ���(dst)
	dst.create(src.size(), src.type());

	/// ԭͼ��ת��Ϊ�Ҷ�ͼ��
	cvtColor(src, src_gray, CV_BGR2GRAY);

	/// ������ʾ����
	namedWindow(window_name, CV_WINDOW_AUTOSIZE);

	/// ����trackbar
	createTrackbar("blur:", window_name, &size, max_lowThreshold, CannyThreshold);
	createTrackbar("ratio:", window_name, &ratio, max_lowThreshold, CannyThreshold);
	createTrackbar("kernel_size:", window_name, &kernel_size, max_lowThreshold, CannyThreshold);
	createTrackbar("Min Threshold:", window_name, &lowThreshold, max_lowThreshold, CannyThreshold);
	/// ��ʾͼ��
	//CannyThreshold(0, 0);
	imshow(window_name, src_gray);
	/// �ȴ��û���Ӧ
	waitKey(0);

	return 0;
}