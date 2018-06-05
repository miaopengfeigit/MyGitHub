// ClrDll.h

#pragma once
#include <opencv2\opencv.hpp>  
using namespace cv;
using namespace System;

namespace ClrDll {

	public ref class Class1
	{
		// TODO:  �ڴ˴���Ӵ���ķ�����
		/*----------------------------
		* ���� : ��ͼ���ʽ�� cv::Mat ת��Ϊ System::Drawing::Bitmap
		*		- ������ͼ������
		*----------------------------
		* ���� : ConvertMatToBitmap
		* ���� : public
		* ���� : Bitmapͼ��ָ�룬��ת��ʧ�ܣ��򷵻ص�ͼ���߾�Ϊ1
		*
		* ���� : cvImg		[in]	OpenCV ͼ��
		*/
		System::Drawing::Bitmap^ ConvertMatToBitmap(cv::Mat& cvImg)
		{
			System::Drawing::Bitmap^ bmpImg;

			//���ͼ��λ��
			if (cvImg.depth() != CV_8U)
			{
				cout << "����ͼ��λ�" << cvImg.depth() << ". ֻ����ÿͨ��8λ��ȵ�ͼ��" << endl;
				bmpImg = gcnew System::Drawing::Bitmap(1, 1, System::Drawing::Imaging::PixelFormat::Format8bppIndexed);
				return (bmpImg);
			}

			//��ɫͼ��
			if (cvImg.channels() == 3)
			{
				bmpImg = gcnew Bitmap(
					cvImg.cols,
					cvImg.rows,
					cvImg.step,
					System::Drawing::Imaging::PixelFormat::Format24bppRgb,
					(System::IntPtr)cvImg.data);
			}
			//�Ҷ�ͼ��
			else if (cvImg.channels() == 1)
			{
				bmpImg = gcnew Bitmap(
					cvImg.cols,
					cvImg.rows,
					cvImg.step,
					System::Drawing::Imaging::PixelFormat::Format8bppIndexed,
					(System::IntPtr)cvImg.data);
			}

			return (bmpImg);
		}

		/*----------------------------
		* ���� : ��ͼ���ʽ�� System::Drawing::Bitmap ת��Ϊ cv::Mat
		*		- ������ͼ������
		*----------------------------
		* ���� : ConvertBitmapToMat
		* ���� : public
		* ���� : 0��ת���ɹ�
		*
		* ���� : bmpImg		[in]	.Net ͼ��
		* ���� : cvImg		[out]	OpenCV ͼ��
		*/
		int ConvertBitmapToMat(System::Drawing::Bitmap^ bmpImg, cv::Mat& cvImg)
		{
			int retVal = 0;

			//����Bitmap����
			System::Drawing::Imaging::BitmapData^ bmpData = bmpImg->LockBits(
				System::Drawing::Rectangle(0, 0, bmpImg->Width, bmpImg->Height),
				System::Drawing::Imaging::ImageLockMode::ReadWrite, bmpImg->PixelFormat);

			//��cvImg�ǿգ������ԭ������
			if (!cvImg.empty())
			{
				cvImg.release();
			}

			//�� bmpImg ������ָ�븴�Ƶ� cvImg �У�����������
			if (bmpImg->PixelFormat == System::Drawing::Imaging::PixelFormat::Format8bppIndexed)	// �Ҷ�ͼ��
			{
				cvImg = cv::Mat(bmpImg->Height, bmpImg->Width, CV_8UC1, (char*)bmpData->Scan0.ToPointer());
			}
			else if (bmpImg->PixelFormat == System::Drawing::Imaging::PixelFormat::Format24bppRgb)	// ��ɫͼ��
			{
				cvImg = cv::Mat(bmpImg->Height, bmpImg->Width, CV_8UC3, (char*)bmpData->Scan0.ToPointer());
			}

			//����Bitmap����
			bmpImg->UnlockBits(bmpData);

			return (retVal);
		}


	};
}
