#include "pch.h"
#include "box2dWorld.h"
#include <thread>

float hertz = 60.0f;
int velocityIterations = 6;
int positionIterations = 2;
float PIXEL_TO_METER = 3.0f;
int screenHeight, screenWidth;

const b2Vec2 Box2DWorld::ConvertScreenToWorld(b2Vec2 screenPoint)
{
	return { screenPoint.x / PIXEL_TO_METER, (-screenPoint.y + screenHeight) / PIXEL_TO_METER };
}
const b2Vec2 Box2DWorld::ConvertWorldToScreen(b2Vec2 worldPoint)
{
	return { worldPoint.x * PIXEL_TO_METER, -((worldPoint.y * PIXEL_TO_METER) - screenHeight) };
}
/// <summary>
/// key参数仅为了标识一个body，并非指针
/// </summary>
/// <param name="intptr"></param>
/// <param name="targetPos"></param>
/// <param name="key"></param>
/// <returns></returns>
b2Body* Box2DWorld::CreateBody(HWND intptr, uintptr_t key, b2Vec2 targetPos) {
	RECT rect;
	GetWindowRect(intptr, &rect);
	b2BodyDef bodyDef;
	bodyDef.type = b2_dynamicBody;
	bodyDef.position = ConvertScreenToWorld(targetPos);
	b2PolygonShape shape;
	shape.SetAsBox((rect.right - rect.left) / 2.0f / PIXEL_TO_METER, (rect.bottom - rect.top) / 2.0f / PIXEL_TO_METER);
	b2FixtureDef fixtureDef;
	fixtureDef.shape = &shape;
	fixtureDef.restitution = 0.5f;
	fixtureDef.restitutionThreshold = 80.0f;
	fixtureDef.density = 1.0f;
	b2Body* body = world->CreateBody(&bodyDef);
	body->CreateFixture(&fixtureDef);
	body->GetUserData().pointer = key;
	body->SetTransform({ rect.left / PIXEL_TO_METER, rect.top / PIXEL_TO_METER }, 0);
	return body;
}
b2Body* Box2DWorld::GetBody(int target) {

	for (b2Body* body = world->GetBodyList(); body; body = body->GetNext())
	{
		if (body->GetUserData().pointer == target)
			return body;
	}
	return NULL;
}
bool Box2DWorld::AddImpulse(int target, b2Vec2 impulse) {
	b2Body* body = GetBody(target);
	if (body != NULL) {
		body->ApplyLinearImpulseToCenter(impulse, true);
		return true;
	}
	return false;
}
bool Box2DWorld::SetBodyPos(int target, b2Vec2 pos) {
	b2Body* body = GetBody(target);
	if (body != NULL) {
		body->SetTransform(ConvertScreenToWorld(pos), body->GetAngle());
		body->SetAwake(true);
		return true;
	}
	return false;
}
bool Box2DWorld::RotateBody(int target, float angle) {
	b2Body* body = GetBody(target);
	if (body != NULL) {
		body->SetTransform(body->GetPosition(), angle * b2_pi / 180);
		body->SetAwake(true);
		return true;
	}
	return false;
}
RECT Box2DWorld::GetBodyRectangle(intptr_t target) {
	b2Body* body = GetBody(target);
	RECT rect{ -1,-1,-1,-1 };
	GetClientRect((HWND)body->GetUserData().pointer, &rect);
	return rect;
}
bool Box2DWorld::SetBodyRectangle(intptr_t target, RECT rect) {
	b2Body* body = GetBody(target);
	if (body != NULL) {
		b2PolygonShape* shape = (b2PolygonShape*)body->GetFixtureList()->GetShape();
		shape->SetAsBox((rect.right - rect.left) / 2.0f / PIXEL_TO_METER, (rect.bottom - rect.top) / 2.0f / PIXEL_TO_METER);
		return true;
	}
	return false;
}

Box2DWorld::Box2DWorld() {
	//HDC hdc = GetDC(NULL);
	//屏幕整体尺寸
	//screenWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
	//screenHeight = GetDeviceCaps(hdc, DESKTOPVERTRES);
	//屏幕整体尺寸
	//screenWidth = GetDeviceCaps(hdc, HORZRES);
	//screenHeight = GetDeviceCaps(hdc, VERTRES);
	//屏幕整体尺寸
	screenWidth = GetSystemMetrics(SM_CXSCREEN);
	screenHeight = GetSystemMetrics(SM_CYSCREEN);
	// 不包含任务栏的高度
	//screenWidth = GetSystemMetrics(SM_CXFULLSCREEN);
	//screenHeight = GetSystemMetrics(SM_CYFULLSCREEN);
	//ReleaseDC(NULL, hdc);
	paused = false;
}
void Box2DWorld::Start() {
	//Create World
	world = new b2World(b2Vec2(0.0f, -9.8f));
	world->SetAllowSleeping(true);
	//Create Wall
	b2Vec2 offset(0, 0);
	b2BodyDef wallDef;
	b2Body* wallBody;
	wallBody = world->CreateBody(&wallDef);
	b2EdgeShape wallShape;
	wallShape.SetTwoSided(ConvertScreenToWorld({ 0 + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER) }),
		ConvertScreenToWorld({ screenWidth + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER) }));
	wallBody->CreateFixture(&wallShape, 0);//下
	wallShape.SetTwoSided(ConvertScreenToWorld({ 0 + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER) }),
		ConvertScreenToWorld({ 0 + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER) }));
	wallBody->CreateFixture(&wallShape, 0);//左
	wallShape.SetTwoSided(ConvertScreenToWorld({ 0 + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER) }),
		ConvertScreenToWorld({ screenWidth + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER) }));
	wallBody->CreateFixture(&wallShape, 0);//上
	wallShape.SetTwoSided(ConvertScreenToWorld({ screenWidth + (offset.x * PIXEL_TO_METER), screenHeight + (offset.y * PIXEL_TO_METER) }),
		ConvertScreenToWorld({ screenWidth + (offset.x * PIXEL_TO_METER), 0 + (offset.y * PIXEL_TO_METER) }));
	wallBody->CreateFixture(&wallShape, 0);//右

	std::thread updateThread(&Box2DWorld::Update, this);
	updateThread.detach();
}
void Box2DWorld::Update() {
	while (42)
	{
		if (Box2DWorld::paused == false)
		{
			world->Step(1 / hertz, velocityIterations, positionIterations);
		}
	}
}
Box2DWorld::~Box2DWorld() {
	delete& world;
}