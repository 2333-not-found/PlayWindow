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
		RECT rect;// 窗口的矩形区域，用于对比变化
        float bodyPosX, bodyPosY; // 窗口左上角的坐标
		float angle;
        bool isDraging; // 是否正在拖动窗口
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
static HBITMAP CaptureWindowByBitblt(HWND hwnd) {
    // 获取根窗口的设备上下文
    HDC hdcWindow = GetDC(hwnd);
    if (!hdcWindow) {
        std::cerr << "Failed to get window DC" << std::endl;
        return NULL;
    }

    // 获取整个根窗口的矩形区域（包括标题栏和边框）
    RECT rect;
    if (!GetWindowRect(hwnd, &rect)) {
        std::cerr << "Failed to get window rect" << std::endl;
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 创建一个与根窗口设备上下文兼容的内存设备上下文
    HDC hdcMem = CreateCompatibleDC(hdcWindow);
    if (!hdcMem) {
        std::cerr << "Failed to create compatible DC" << std::endl;
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 创建一个与根窗口设备上下文兼容的位图
    HBITMAP hBitmap = CreateCompatibleBitmap(hdcWindow, rect.right - rect.left, rect.bottom - rect.top);
    if (!hBitmap) {
        std::cerr << "Failed to create compatible bitmap" << std::endl;
        DeleteDC(hdcMem);
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 将位图选入内存设备上下文
    HBITMAP hOldBitmap = (HBITMAP)SelectObject(hdcMem, hBitmap);

    // 将根窗口内容复制到内存设备上下文中
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

static HBITMAP CaptureWindowByPrintWindow(HWND hwnd) {
    // 获取根窗口的设备上下文
    HDC hdcWindow = GetDC(hwnd);
    if (!hdcWindow) {
        std::cerr << "Failed to get window DC" << std::endl;
        return NULL;
    }

    // 获取整个根窗口的矩形区域（包括标题栏和边框）
    RECT rect;
    if (!GetWindowRect(hwnd, &rect)) {
        std::cerr << "Failed to get window rect" << std::endl;
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 创建一个与根窗口设备上下文兼容的内存设备上下文
    HDC hdcMem = CreateCompatibleDC(hdcWindow);
    if (!hdcMem) {
        std::cerr << "Failed to create compatible DC" << std::endl;
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 创建一个与根窗口设备上下文兼容的位图
    HBITMAP hBitmap = CreateCompatibleBitmap(hdcWindow, rect.right - rect.left, rect.bottom - rect.top);
    if (!hBitmap) {
        std::cerr << "Failed to create compatible bitmap" << std::endl;
        DeleteDC(hdcMem);
        ReleaseDC(hwnd, hdcWindow);
        return NULL;
    }

    // 将位图选入内存设备上下文
    HBITMAP hOldBitmap = (HBITMAP)SelectObject(hdcMem, hBitmap);

    // 将根窗口内容复制到内存设备上下文中
    if (!PrintWindow(hwnd, hdcMem, 0)) { // 使用PrintWindow来捕获整个窗口，包括标题栏
        std::cerr << "PrintWindow failed" << std::endl;
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