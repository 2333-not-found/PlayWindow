#pragma once
#include <windows.h>
class D3DRender
{
public:
	D3DRender(HINSTANCE hInstance, int iWidth, int iHeight);
	bool StartRenderer();
	void Update();
};