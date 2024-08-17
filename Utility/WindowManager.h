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
	// ��Ŵ��ڵĶ���
	std::unordered_map< uintptr_t, WindowData > windowQueue;
	bool AddNewWindow(HWND handle);
	bool IsDraging(HWND handle);
};

class WindowsApi {
public:
	// ��ȡָ�����ھ������Ļ��ͼ
	static HBITMAP CaptureWindow(HWND hwnd) {
		// ��ȡ���ڵ��豸������
		HDC hdcWindow = GetDC(hwnd);
		if (!hdcWindow) {
			std::cerr << "Failed to get window DC" << std::endl;
			return NULL;
		}

		// ��ȡ���ڵľ�������
		RECT rect;
		if (!GetClientRect(hwnd, &rect)) {
			std::cerr << "Failed to get client rect" << std::endl;
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// ����һ���봰���豸�����ļ��ݵ��ڴ��豸������
		HDC hdcMem = CreateCompatibleDC(hdcWindow);
		if (!hdcMem) {
			std::cerr << "Failed to create compatible DC" << std::endl;
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// ����һ���봰���豸�����ļ��ݵ�λͼ
		HBITMAP hBitmap = CreateCompatibleBitmap(hdcWindow, rect.right - rect.left, rect.bottom - rect.top);
		if (!hBitmap) {
			std::cerr << "Failed to create compatible bitmap" << std::endl;
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// ��λͼѡ���ڴ��豸������
		HBITMAP hOldBitmap = (HBITMAP)SelectObject(hdcMem, hBitmap);

		// ���������ݸ��Ƶ��ڴ��豸��������
		if (!BitBlt(hdcMem, 0, 0, rect.right - rect.left, rect.bottom - rect.top, hdcWindow, 0, 0, SRCCOPY)) {
			std::cerr << "BitBlt failed" << std::endl;
			SelectObject(hdcMem, hOldBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return NULL;
		}

		// �ָ�ԭ����λͼ
		SelectObject(hdcMem, hOldBitmap);

		// ������Դ
		DeleteDC(hdcMem);
		ReleaseDC(hwnd, hdcWindow);

		return hBitmap;
	}
	static HWND GetHandleFromCursor(bool getRoot = false) {
		POINT pt;
		if (!GetCursorPos(&pt)) {
			std::cerr << "�޷���ȡ�������" << std::endl;
			return NULL;
		}

		HWND hwnd = WindowFromPoint(pt);
		if (hwnd == NULL) {
			std::cerr << "�޷���ȡ���ھ��" << std::endl;
			return NULL;
		}
		if (getRoot) hwnd = GetAncestor(hwnd, GA_ROOT);

		return hwnd;
	}
};