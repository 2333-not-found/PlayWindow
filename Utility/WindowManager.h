#pragma once
#include "box2dWorld.h"
#include "..\Utility\hook.h"
#include <unordered_map>
class WindowManager
{
public:
	WindowManager();
	void Update();
	Box2DWorld* myWorld;
	// 存放窗口的队列
	std::unordered_map< uintptr_t, Box2DWorld::UserData> windowQueue;
	bool AddNewWindow(HWND handle);
	inline bool IsDraging(HWND handle);
};