#pragma once
#include "..\box2d\include\box2d\box2d.h"
#include "..\box2d\include\box2d\b2_Body.h"
#include "windows.h"

class Box2DWorld
{
public:
	struct UserData
	{
		int intPtr_p;//�����ڵ�IntPtr
		RECT rect;//����ĵĴ��ھ��δ�С
	};
	Box2DWorld(int iWidth, int iHeight);
	b2World* world;
	b2Body* wallBody;
	bool paused;
	static const b2Vec2 ConvertScreenToWorld(b2Vec2 screenPoint);
	static const b2Vec2 ConvertWorldToScreen(b2Vec2 worldPoint);
	b2Body* CreateBody(HWND intptr, uintptr_t key, b2Vec2 targetPos = { 100,100 });
	b2Body* GetBody(int target);
	bool AddImpulse(int target, b2Vec2 impulse);
	bool SetBodyPos(int target, b2Vec2 pos);
	bool SetBodyRotation(int target, float angle);
	RECT GetBodyRectangle(intptr_t target);
	bool SetBodyRectangle(intptr_t target, RECT rect);
	void Update();
	~Box2DWorld();
};