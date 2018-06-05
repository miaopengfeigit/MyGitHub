// ClrDll.h

#pragma once
#include <opencv2\opencv.hpp>  
using namespace cv;
using namespace System;

namespace ClrDll {

	public ref class Class1
	{
		// TODO:  在此处添加此类的方法。
		/*----------------------------
		* 功能 : 将图像格式由 cv::Mat 转换为 System::Drawing::Bitmap
		*		- 不拷贝图像数据
		*----------------------------
		* 函数 : ConvertMatToBitmap
		* 访问 : public
		* 返回 : Bitmap图像指针，若转换失败，则返回的图像宽高均为1
		*
		* 参数 : cvImg		[in]	OpenCV 图像
		*/
		System::Drawing::Bitmap^ ConvertMatToBitmap(cv::Mat& cvImg)
		{
			System::Drawing::Bitmap^ bmpImg;

			//检查图像位深
			if (cvImg.depth() != CV_8U)
			{
				cout << "输入图像位深：" << cvImg.depth() << ". 只处理每通道8位深度的图像！" << endl;
				bmpImg = gcnew System::Drawing::Bitmap(1, 1, System::Drawing::Imaging::PixelFormat::Format8bppIndexed);
				return (bmpImg);
			}

			//彩色图像
			if (cvImg.channels() == 3)
			{
				bmpImg = gcnew Bitmap(
					cvImg.cols,
					cvImg.rows,
					cvImg.step,
					System::Drawing::Imaging::PixelFormat::Format24bppRgb,
					(System::IntPtr)cvImg.data);
			}
			//灰度图像
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
		* 功能 : 将图像格式由 System::Drawing::Bitmap 转换为 cv::Mat
		*		- 不拷贝图像数据
		*----------------------------
		* 函数 : ConvertBitmapToMat
		* 访问 : public
		* 返回 : 0：转换成功
		*
		* 参数 : bmpImg		[in]	.Net 图像
		* 参数 : cvImg		[out]	OpenCV 图像
		*/
		int ConvertBitmapToMat(System::Drawing::Bitmap^ bmpImg, cv::Mat& cvImg)
		{
			int retVal = 0;

			//锁定Bitmap数据
			System::Drawing::Imaging::BitmapData^ bmpData = bmpImg->LockBits(
				System::Drawing::Rectangle(0, 0, bmpImg->Width, bmpImg->Height),
				System::Drawing::Imaging::ImageLockMode::ReadWrite, bmpImg->PixelFormat);

			//若cvImg非空，则清空原有数据
			if (!cvImg.empty())
			{
				cvImg.release();
			}

			//将 bmpImg 的数据指针复制到 cvImg 中，不拷贝数据
			if (bmpImg->PixelFormat == System::Drawing::Imaging::PixelFormat::Format8bppIndexed)	// 灰度图像
			{
				cvImg = cv::Mat(bmpImg->Height, bmpImg->Width, CV_8UC1, (char*)bmpData->Scan0.ToPointer());
			}
			else if (bmpImg->PixelFormat == System::Drawing::Imaging::PixelFormat::Format24bppRgb)	// 彩色图像
			{
				cvImg = cv::Mat(bmpImg->Height, bmpImg->Width, CV_8UC3, (char*)bmpData->Scan0.ToPointer());
			}

			//解锁Bitmap数据
			bmpImg->UnlockBits(bmpData);

			return (retVal);
		}


	};
}
