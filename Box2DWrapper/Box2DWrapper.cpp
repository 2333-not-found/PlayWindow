//#include "stdafx.h"
#include "pch.h"
#include <iostream>
#include <box2d/box2d.h>
#include "Box2DWrapper.h"
using namespace std;
BOX2DWRAPPER_API int __stdcall Add(int a, int b)
{
	return a + b;
}
BOX2DWRAPPER_API void __stdcall WriteString(wchar_t* content)
{
	wprintf(content);
	printf("\n");
}

BOX2DWRAPPER_API void __stdcall AddInt(int* i)
{
	(*i)++;
}

BOX2DWRAPPER_API void __stdcall AddIntArray(int* firstElement, int arrayLength)
{
	int* currentPointer = firstElement;
	for (int i = 0; i < arrayLength; i++)
	{
		cout << *currentPointer;
		currentPointer++;
	}
	cout << endl;
}
int* arrPtr;
BOX2DWRAPPER_API int* __stdcall GetArrayFromCPP()
{
	arrPtr = new int[10];

	for (int i = 0; i < 10; i++)
	{
		arrPtr[i] = i;
	}

	return arrPtr;
}

CPPCallback callback;
BOX2DWRAPPER_API void __stdcall SetCallback(CPPCallback _callback)
{
	int tick = 100;
	//下面的代码是对C#中委托进行调用
	callback = _callback;
	callback(tick);
}

BOX2DWRAPPER_API void __stdcall SendStructFromCSToCPP(Vector3 vector)
{
	cout << "got vector3 in cpp,x:";
	cout << vector.X;
	cout << ",Y:";
	cout << vector.Y;
	cout << ",Z:";
	cout << vector.Z;
}

BOX2DWRAPPER_API void __stdcall SendStruct(b2BodyDef body) {
	cout << body.angularVelocity;
}

const float PIXEL_TO_METER = 100;
int screenHeight;
int screenWidth;
float hertz = 60.0f;
int velocityIterations = 8;
int positionIterations = 3;
b2World* world;
static b2Vec2 ConvertScreenToWorld(b2Vec2* screenPoint)
{
	b2Vec2 converted{ screenPoint->x / PIXEL_TO_METER, (-screenPoint->y + screenHeight) / PIXEL_TO_METER };
	return converted;
}
static b2Vec2 ConvertWorldToScreen(b2Vec2* worldPoint)
{
	b2Vec2 converted{ worldPoint->x * PIXEL_TO_METER, -((worldPoint->y * PIXEL_TO_METER) - screenHeight) };
	return converted;
}

BOX2DWRAPPER_API void __stdcall RunEngine(int _screenHeight, int _screenWidth) {
	screenHeight = _screenHeight;
	screenWidth = _screenWidth;
	b2Vec2 gravity(0.0f, -9.8f);
	b2World* _world = new b2World(gravity);
	_world->SetAllowSleeping(true);
	world = _world;
	b2Vec2 offset(0, 0);
	b2BodyDef wallDef;
	b2Body* wallBody = world->CreateBody(&wallDef);
	b2EdgeShape wallShape;
	wallShape.SetTwoSided(ConvertScreenToWorld(new b2Vec2{ 0 + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER) }),
		ConvertScreenToWorld(new b2Vec2(screenWidth + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER))));
	wallBody->CreateFixture(&wallShape, 0.0f);//下
	wallShape.SetTwoSided(ConvertScreenToWorld(new b2Vec2(0 + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER))),
		ConvertScreenToWorld(new b2Vec2(0 + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER))));
	wallBody->CreateFixture(&wallShape, 0);//左
	wallShape.SetTwoSided(ConvertScreenToWorld(new b2Vec2(0 + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER))),
		ConvertScreenToWorld(new b2Vec2(screenWidth + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER))));
	wallBody->CreateFixture(&wallShape, 0);//上
	wallShape.SetTwoSided(ConvertScreenToWorld(new b2Vec2(screenWidth + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER))),
		ConvertScreenToWorld(new b2Vec2(screenWidth + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER))));
	wallBody->CreateFixture(&wallShape, 0);//右

	while (true)
	{
		world->Step(1 / hertz, velocityIterations, positionIterations);
		callback(0);
		cout << world->GetBodyList()->GetUserData().pointer << endl;//可能是C#中某些数据被GC掉了，不能注释掉这一行
	}
}
BOX2DWRAPPER_API void __stdcall AddBody(HWND intptr, b2Vec2 targetPos, b2BodyUserData* userData) {
	RECT rect;
	GetWindowRect(intptr, &rect);
	b2BodyDef* bodyDef = new b2BodyDef();
	bodyDef->type = b2BodyType::b2_dynamicBody;
	bodyDef->position = ConvertScreenToWorld(&targetPos);
	bodyDef->userData = *userData;
	b2PolygonShape* shape = new b2PolygonShape();
	b2FixtureDef* fixtureDef = new b2FixtureDef();
	fixtureDef->shape = shape;
	fixtureDef->restitution = 0.5f;
	fixtureDef->restitutionThreshold = 80.0f;
	fixtureDef->density = 1.0f;
	shape->SetAsBox((rect.right - rect.left) / 2 / PIXEL_TO_METER, (rect.bottom - rect.top) / 2 / PIXEL_TO_METER);
	b2Body* body;
	body = world->CreateBody(bodyDef);
	body->CreateFixture(fixtureDef);
	body->SetType(b2_dynamicBody);
	b2Vec2* place = new b2Vec2(rect.left / PIXEL_TO_METER, rect.top / PIXEL_TO_METER);
	body->SetTransform(*place, 0);
}