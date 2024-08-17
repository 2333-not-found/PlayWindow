#pragma once
#include <windows.h>
#include <unordered_map>
#include "WindowManager.h"
class D2DRender
{
public:
	D2DRender(HINSTANCE hInstance, int iWidth, int iHeight, int nCmdShow);
	void Update(const std::unordered_map< uintptr_t, WindowManager::WindowData > windowMap);
};