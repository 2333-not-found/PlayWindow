#include "pch.h"
#include "WindowManager.h"
#include <windows.h>
#include <thread>

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

		return hBitmap;

		/*
		// 将位图选入内存设备上下文
		HGDIOBJ hOldBitmap = SelectObject(hdcMem, hBitmap);

		// 将窗口内容复制到内存设备上下文
		if (!BitBlt(hdcMem, 0, 0, rect.right - rect.left, rect.bottom - rect.top, hdcWindow, 0, 0, SRCCOPY)) {
			std::cerr << "BitBlt failed" << std::endl;
			SelectObject(hdcMem, hOldBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return false;
		}

		// 保存位图为文件
		BITMAPINFOHEADER bmi = { 0 };
		bmi.biSize = sizeof(BITMAPINFOHEADER);
		bmi.biWidth = rect.right - rect.left;
		bmi.biHeight = rect.bottom - rect.top;
		bmi.biPlanes = 1;
		bmi.biBitCount = 32;
		bmi.biCompression = BI_RGB;

		DWORD dwBmpSize = ((rect.right - rect.left) * 32 + 31) / 32 * 4 * (rect.bottom - rect.top);
		HANDLE hDIB = GlobalAlloc(GHND, dwBmpSize);
		char* lpbitmap = (char*)GlobalLock(hDIB);

		GetDIBits(hdcWindow, hBitmap, 0, (UINT)(rect.bottom - rect.top), lpbitmap, (BITMAPINFO*)&bmi, DIB_RGB_COLORS);

		// 保存位图到文件
		FILE* fp;
		fopen_s(&fp, filename.c_str(), "wb");
		if (!fp) {
			std::cerr << "Failed to open file for writing" << std::endl;
			GlobalUnlock(hDIB);
			GlobalFree(hDIB);
			SelectObject(hdcMem, hOldBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hdcMem);
			ReleaseDC(hwnd, hdcWindow);
			return false;
		}

		BITMAPFILEHEADER bmfHeader;
		bmfHeader.bfType = 0x4D42; // "BM"
		bmfHeader.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + dwBmpSize;
		bmfHeader.bfReserved1 = 0;
		bmfHeader.bfReserved2 = 0;
		bmfHeader.bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

		fwrite(&bmfHeader, sizeof(BITMAPFILEHEADER), 1, fp);
		fwrite(&bmi, sizeof(BITMAPINFOHEADER), 1, fp);
		fwrite(lpbitmap, dwBmpSize, 1, fp);

		fclose(fp);

		GlobalUnlock(hDIB);
		GlobalFree(hDIB);

		// 清理资源
		SelectObject(hdcMem, hOldBitmap);
		DeleteObject(hBitmap);
		DeleteDC(hdcMem);
		ReleaseDC(hwnd, hdcWindow);

		return true;
		*/

		DeleteObject(hBitmap);
		DeleteDC(hdcMem);
		ReleaseDC(hwnd, hdcWindow);
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

LRESULT WINAPI MyMouseCallback(int nCode, WPARAM wParam, LPARAM lParam); //callback declaration
LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam);
WindowManager::WindowManager()
{
	myWorld = new Box2DWorld();

	Hook::Instance().InstallHook(MyMouseCallback, MyKeyBoardCallback);
	std::thread updateThread(&WindowManager::Update, this);
	updateThread.detach();
}

void WindowManager::Update() {
	while (42) {
		if (myWorld->paused == false)
		{
			for (auto& pair : windowQueue) {
				myWorld->paused = true;
				HWND hwnd = (HWND)pair.first;
				//RECT rect = pair.second.rect;
				RECT rect;
				GetWindowRect(hwnd, &rect);
				b2Vec2 pos = myWorld->GetBody(pair.first)->GetPosition();
				pos = Box2DWorld::ConvertWorldToScreen(pos);
				pos.x -= (rect.right - rect.left) / 2;
				pos.y -= (rect.bottom - rect.top) / 2;
				SetWindowPos(hwnd, 0, pos.x, pos.y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
				myWorld->paused = false;
				std::cout << IsDraging(hwnd) << std::endl;
			}
		}
	}
}

bool WindowManager::AddNewWindow(HWND handle) {
	// 尝试截图
	if (WindowsApi::CaptureWindow(handle) == NULL)
		return false;
	SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
	myWorld->paused = true;
	Box2DWorld::UserData data{};
	data.intPtr_p = (uintptr_t)handle;
	WindowManager::windowQueue[(uintptr_t)handle] = data;
	myWorld->CreateBody(handle, (uintptr_t)handle, { 0,0 });
	myWorld->paused = false;
}
bool isDragging = false;
inline bool WindowManager::IsDraging(HWND handle) {
	return (GetParent(WindowsApi::GetHandleFromCursor(true)) == NULL && GetForegroundWindow() == handle && isDragging);
}
MSLLHOOKSTRUCT* pMouseStruct = nullptr;
KBDLLHOOKSTRUCT* pKeyStruct = nullptr;
static LRESULT WINAPI MyMouseCallback(int nCode, WPARAM wParam, LPARAM lParam) {

	pMouseStruct = (MSLLHOOKSTRUCT*)lParam; // WH_MOUSE_LL struct
	/*
	nCode, this parameters will determine how to process a message
	This callback in this case only have information when it is 0 (HC_ACTION): wParam and lParam contain info

	wParam is about WINDOWS MESSAGE, in this case MOUSE messages.
	lParam is information contained in the structure MSLLHOOKSTRUCT
	*/

	if (nCode == 0) { // we have information in wParam/lParam ? If yes, let's check it:
		if ((MSLLHOOKSTRUCT*)lParam != NULL) { // Mouse struct contain information?		
			//if (pDlg != nullptr) {
			//	CString str;
			//	str.Format(_T("Mouse Coordinates: x = %i | y = %i \n"), pMouseStruct->pt.x, pMouseStruct->pt.y);
			//	//pDlg->GetDlgItem(IDC_EDIT_tb)->SetWindowText(str);
			//}
		}
		switch (wParam) {
		case WM_LBUTTONUP: {
			isDragging = false;
			printf_s("LEFT CLICK UP\n");
		}break;
		case WM_LBUTTONDOWN: {
			isDragging = true;
			printf_s("LEFT CLICK DOWN\n");
		}break;
		case WM_RBUTTONUP: {
			printf_s("RIGHT CLICK UP\n");
		}break;
		case WM_RBUTTONDOWN: {
			printf_s("RIGHT CLICK DOWN\n");
		}break;
		case WM_MOUSEMOVE: {
			printf_s("MOUSE MOVE\n");
		}break;
		case WM_CHAR: {
			printf_s("%c", (TCHAR)wParam);
		}break;
		}
	}
	/*
	Every time that the nCode is less than 0 we need to CallNextHookEx:
	-> Pass to the next hook
	MSDN: Calling CallNextHookEx is optional, but it is highly recommended;
	otherwise, other applications that have installed hooks will not receive hook notifications and may behave incorrectly as a result.
	*/
	return CallNextHookEx(Hook::Instance().hook, nCode, wParam, lParam);
}

static LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam)
{
	pKeyStruct = (KBDLLHOOKSTRUCT*)lParam;

	if (nCode == 0)
	{
		if (pKeyStruct)
		{
			printf_s("Virtual Key Code: %d \n", pKeyStruct->vkCode);
		}
		switch (wParam)
		{
		case WM_KEYDOWN: {
			printf_s("Sys Key\n");
		}break;
		case WM_SYSKEYDOWN: {
			printf_s("Not Sys Key\n");
		}break;
		}
	}
	return CallNextHookEx(Hook::Instance().keyboardhook, nCode, wParam, lParam);
}