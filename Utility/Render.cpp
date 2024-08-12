#include "pch.h"
#include "framework.h"
#include "Render.h"
#include <d2d1.h>
#include <string>
#include <wincodec.h>
#include <windows.h>
#include <thread>

#pragma comment(lib, "d2d1.lib")
#pragma comment(lib, "windowscodecs.lib")

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

ID2D1Factory* pD2DFactory = NULL;
ID2D1HwndRenderTarget* pRenderTarget = NULL;
ID2D1Bitmap* pBitmap = NULL;
IWICImagingFactory* pWICFactory = NULL;
HBITMAP hBitmap = NULL;
int nFrameCount = 0;

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
        WS_OVERLAPPEDWINDOW,            // Window style

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

    bool result = ShowWindow(hwnd, nCmdShow);

    std::thread updateThread(&D2DRender::Update, this);
    updateThread.detach();
    return;
}

void D2DRender::Update()
{
    MSG msg = { };
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    return;
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

        if (SUCCEEDED(hr))
        {
            IWICBitmap* pWICBitmap = NULL;
            hr = pWICFactory->CreateBitmapFromHBITMAP(hBitmap, NULL, WICBitmapIgnoreAlpha, &pWICBitmap);

            if (SUCCEEDED(hr))
            {
                hr = pRenderTarget->CreateBitmapFromWicBitmap(pWICBitmap, NULL, &pBitmap);
                pWICBitmap->Release();
            }
        }
    }

    return hr;
}

void DiscardD2DResources()
{
    if (pBitmap)
    {
        pBitmap->Release();
        pBitmap = NULL;
    }

    if (pRenderTarget)
    {
        pRenderTarget->Release();
        pRenderTarget = NULL;
    }

    if (pD2DFactory)
    {
        pD2DFactory->Release();
        pD2DFactory = NULL;
    }

    if (pWICFactory)
    {
        pWICFactory->Release();
        pWICFactory = NULL;
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
    case WM_CREATE:
        hBitmap = (HBITMAP)LoadImage(NULL, L"D:\\Users\\2333\\Desktop\\ac-congrats.bmp", IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
        if (!hBitmap)
        {
            MessageBox(hwnd, L"Failed to load bitmap", L"Error", MB_OK);
            return -1;
        }
        break;

    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        BeginPaint(hwnd, &ps);

        HRESULT hr = CreateD2DResources(hwnd);

        if (SUCCEEDED(hr))
        {
            pRenderTarget->BeginDraw();
            pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));

            if (pBitmap)
            {
                D2D1_SIZE_F size = pBitmap->GetSize();
                pRenderTarget->DrawBitmap(pBitmap, D2D1::RectF(0, 0, size.width, size.height));
            }

            hr = pRenderTarget->EndDraw();

            if (FAILED(hr) || hr == D2DERR_RECREATE_TARGET)
            {
                DiscardD2DResources();
            }
        }

        EndPaint(hwnd, &ps);
        std::wstring windowText = L"Direct2D and WIC Example - Frame Count: " + std::to_wstring(++nFrameCount);
        SetWindowText(hwnd, windowText.c_str());
    }
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
