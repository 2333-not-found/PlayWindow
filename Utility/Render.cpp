#include "Render.h"
#include <d2d1.h>
#include <string>
#include <thread>
#include <wincodec.h>
#include <windows.h>
#define M_PI 3.14159265358979323846
#define SAFE_RELEASE(p) { if(p) { (p)->Release(); (p) = NULL; } }

#pragma comment(lib, "d2d1.lib")
#pragma comment(lib, "windowscodecs.lib")

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

ID2D1Factory* pD2DFactory = NULL;
ID2D1Bitmap* pBitmap = NULL;
IWICImagingFactory* pWICFactory = NULL;
HBITMAP hBitmap = NULL;
int nFrameCount = 0;
HWND hWnd = NULL;
HRESULT CreateD2DResources(HWND hwnd);
void DiscardD2DResources();
D2DRender::D2DRender(HINSTANCE hInstance, int iWidth, int iHeight, int nCmdShow)
{
	const wchar_t CLASS_NAME[] = L"Sample Window Class";

	WNDCLASS wc = { };

	wc.lpfnWndProc = WndProc;
	wc.hInstance = hInstance;
	wc.lpszClassName = CLASS_NAME;

	RegisterClass(&wc);

	HWND hwnd = CreateWindowEx(
		0,                              // Optional window styles.
		CLASS_NAME,                     // Window class
		L"Direct2D and WIC Example",    // Window text
		WS_POPUP,						// Window style

		// Size and position
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,

		NULL,       // Parent window    
		NULL,       // Menu
		hInstance,  // Instance handle
		NULL        // Additional application data
	);

	if (hwnd == NULL)
	{
		return;
	}
	hWnd = hwnd;
	ShowWindow(hwnd, nCmdShow);

	// 设置窗口为分层窗口
	SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_LAYERED);
	// 设置窗口背景色透明
	SetLayeredWindowAttributes(hwnd, RGB(0, 0, 0), 0, LWA_COLORKEY);
	// 设置窗口置顶
	SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

	return;

	MSG msg = { };
	while (GetMessage(&msg, NULL, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
}

ID2D1HwndRenderTarget* pRenderTarget = NULL;
void D2DRender::Update(const std::unordered_map< uintptr_t, WindowManager::WindowData > windowMap)
{
	HRESULT hr = S_OK;
	hr = ::CoInitialize(NULL);
	if (FAILED(hr))
	{
		return;
	}

	PAINTSTRUCT ps;
	BeginPaint(hWnd, &ps);
	hr = CreateD2DResources(hWnd);
	if (SUCCEEDED(hr))
	{
		pRenderTarget->BeginDraw();
		pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::Black)); // 设置背景色为黑色

		IWICImagingFactory* pIWICFactory = NULL;
		hr = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pIWICFactory));
		if (SUCCEEDED(hr))
		{
			for (const auto& pair : windowMap)
			{
				const WindowManager::WindowData& windowData = pair.second;
				HBITMAP hBitmap = WindowsApi::CaptureWindowByPrintWindow(windowData.handle);
				if (!hBitmap)
				{
					MessageBox(hWnd, L"Failed to capture window bitmap", L"Error", MB_OK);
					continue;
				}

				// 将HBITMAP转换为ID2D1Bitmap
				ID2D1Bitmap* pBitmap = NULL;
				IWICBitmap* pIWICBitmap = NULL;
				hr = pIWICFactory->CreateBitmapFromHBITMAP(hBitmap, NULL, WICBitmapIgnoreAlpha, &pIWICBitmap);
				if (SUCCEEDED(hr))
				{
					hr = pRenderTarget->CreateBitmapFromWicBitmap(pIWICBitmap, NULL, &pBitmap);
					pIWICBitmap->Release();
				}

				if (pBitmap)
				{
					// 获取窗口矩形
					RECT rect;
					GetWindowRect(windowData.handle, &rect);

					// 获取位图大小
					D2D1_SIZE_F bitmapSize = pBitmap->GetSize();
					D2D1_POINT_2F center = D2D1::Point2F((rect.left + rect.right) / 2.0f, (rect.top + rect.bottom) / 2.0f);

					// 创建旋转矩阵，以位图中心为旋转中心
					D2D1::Matrix3x2F rotationMatrix = D2D1::Matrix3x2F::Rotation(pair.second.angle * (180 / M_PI), center);

					// 应用组合变换矩阵
					pRenderTarget->SetTransform(rotationMatrix);

					// 使用原始矩形的左上角和右下角坐标来绘制位图
					pRenderTarget->DrawBitmap(pBitmap, D2D1::RectF(rect.left, rect.top, rect.right, rect.bottom));

					// 重置变换矩阵
					pRenderTarget->SetTransform(D2D1::Matrix3x2F::Identity());

					pBitmap->Release();
				}

				DeleteObject(hBitmap);
			}

			pIWICFactory->Release();
		}

		hr = pRenderTarget->EndDraw();

		if (FAILED(hr) || hr == D2DERR_RECREATE_TARGET)
		{
			DiscardD2DResources();
		}
	}

	EndPaint(hWnd, &ps);
	std::wstring windowText = L"Direct2D and WIC Example - Frame Count: " + std::to_wstring(++nFrameCount);
	SetWindowText(hWnd, windowText.c_str());
}


HRESULT CreateD2DResources(HWND hwnd)
{
	HRESULT hr = S_OK;

	if (pRenderTarget == NULL)
	{
		RECT rc;
		GetClientRect(hwnd, &rc);

		D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

		hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &pD2DFactory);

		if (SUCCEEDED(hr))
		{
			hr = pD2DFactory->CreateHwndRenderTarget(
				D2D1::RenderTargetProperties(),
				D2D1::HwndRenderTargetProperties(hwnd, size),
				&pRenderTarget);
		}

		if (SUCCEEDED(hr))
		{
			hr = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pWICFactory));
		}

	}

	return hr;
}

void DiscardD2DResources()
{
	SAFE_RELEASE(pBitmap);
	SAFE_RELEASE(pRenderTarget);
	SAFE_RELEASE(pD2DFactory);
	SAFE_RELEASE(pWICFactory);
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_CREATE:
		// 设置窗口为分层窗口
		SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_LAYERED);
		// 设置窗口背景色透明
		SetLayeredWindowAttributes(hwnd, RGB(0, 0, 0), 0, LWA_COLORKEY);
		// 设置窗口置顶
		SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
		break;

	case WM_PAINT:
		break;

	case WM_DESTROY:
		DiscardD2DResources();
		DeleteObject(hBitmap);
		PostQuitMessage(0);
		break;

	default:
		return DefWindowProc(hwnd, msg, wParam, lParam);
	}
	return 0;
}
