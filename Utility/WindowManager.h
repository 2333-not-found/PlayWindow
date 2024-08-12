#pragma once
#include "box2dWorld.h"
#include "..\Utility\Render.h"
#include "..\Utility\hook.h"
#include <unordered_map>
class WindowManager
{
public:
	WindowManager();
	void Update();
	Box2DWorld* myWorld;
	D2DRender render = D2DRender{ GetModuleHandle(NULL), 1024, 768, SW_SHOW };
	// 存放窗口的队列
	std::unordered_map< uintptr_t, Box2DWorld::UserData> windowQueue;
	bool AddNewWindow(HWND handle);
	inline bool IsDraging(HWND handle);
};