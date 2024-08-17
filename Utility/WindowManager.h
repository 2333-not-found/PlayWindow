#pragma once
#include "..\Utility\hook.h"
#include "box2dWorld.h"
#include <algorithm>
#include <cmath>
#include <unordered_map>
class WindowManager
{
public:
	struct WindowData
	{
		HWND handle;
		RECT rect;
		float angle;
	};
	WindowManager();
	void Update();
	Box2DWorld* myWorld;
	// 存放窗口的队列
	std::unordered_map< uintptr_t, WindowData > windowQueue;
	bool AddNewWindow(HWND handle);
	bool IsDraging(HWND handle);
};

class WindowsApi {
public:
	// 获取指定窗口句柄的屏幕截图
	static HBITMAP CaptureWindow(HWND hwnd) {
		// 获取窗口的设备上下文
		HDC hdcWindow = GetDC(hwnd);
		if (!hdcWindow) {
			std::cerr << "Failed to get window DC" << std::endl;
			return NULL;
		}

		// 获取窗口的矩形区域
		RECT rect;
		if (!GetClientRect(hwnd, &rect)) {
			std::cerr << "Failed to get client rect" << std::endl;
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// 创建一个与窗口设备上下文兼容的内存设备上下文
		HDC hdcMem = CreateCompatibleDC(hdcWindow);
		if (!hdcMem) {
			std::cerr << "Failed to create compatible DC" << std::endl;
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// 创建一个与窗口设备上下文兼容的位图
		HBITMAP hBitmap = CreateCompatibleBitmap(hdcWindow, rect.right - rect.left, rect.bottom - rect.top);
		if (!hBitmap) {
			std::cerr << "Failed to create compatible bitmap" << std::endl;
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// 将位图选入内存设备上下文
		HBITMAP hOldBitmap = (HBITMAP)SelectObject(hdcMem, hBitmap);

		// 将窗口内容复制到内存设备上下文中
		if (!BitBlt(hdcMem, 0, 0, rect.right - rect.left, rect.bottom - rect.top, hdcWindow, 0, 0, SRCCOPY)) {
			std::cerr << "BitBlt failed" << std::endl;
			SelectObject(hdcMem, hOldBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// 恢复原来的位图
		SelectObject(hdcMem, hOldBitmap);

		// 清理资源
		DeleteDC(hdcMem);
		ReleaseDC(hwnd, hdcWindow);

		return hBitmap;
	}
	static HWND GetHandleFromCursor(bool getRoot = false) {
		POINT pt;
		if (!GetCursorPos(&pt)) {
			std::cerr << "无法获取鼠标坐标" << std::endl;
			return NULL;
		}

		HWND hwnd = WindowFromPoint(pt);
		if (hwnd == NULL) {
			std::cerr << "无法获取窗口句柄" << std::endl;
			return NULL;
		}
		if (getRoot) hwnd = GetAncestor(hwnd, GA_ROOT);

		return hwnd;
	}
};