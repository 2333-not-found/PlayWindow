#include "WindowManager.h"
#include <d2d1.h>
#include <memory>
#include <unordered_map>
#include <wincodec.h>
#include <windows.h>
#include <wrl/client.h>

class D2DRender {
public:
    std::unordered_map<uintptr_t, WindowManager::WindowData> m_currentWindowMap;
    D2DRender(HINSTANCE hInstance, int iWidth, int iHeight, int nCmdShow);
    ~D2DRender();
    void Update(const std::unordered_map<uintptr_t, WindowManager::WindowData>& windowMap);

    //private:
    HWND m_hWnd = nullptr;
    HINSTANCE m_hInstance = nullptr;

    // Direct2D资源（使用ComPtr自动管理生命周期）
    Microsoft::WRL::ComPtr<ID2D1Factory> m_pD2DFactory;
    Microsoft::WRL::ComPtr<ID2D1HwndRenderTarget> m_pRenderTarget;
    Microsoft::WRL::ComPtr<IWICImagingFactory> m_pWICFactory;

    // 位图缓存（智能指针管理HBITMAP）
    std::unordered_map<uintptr_t,
        std::pair<Microsoft::WRL::ComPtr<ID2D1Bitmap>, std::unique_ptr<HBITMAP, decltype(&DeleteObject)>>> m_bitmapCache;

    HRESULT CreateD2DResources(HWND hwnd);
    static LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
};