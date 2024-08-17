#include "..\Utility\Render.h"
#include "WindowManager.h"
#include <windows.h>
#include <thread>

int screenWidth = 1024;
int screenHeight = 768;
D2DRender* render = NULL;
LRESULT WINAPI MyMouseCallback(int nCode, WPARAM wParam, LPARAM lParam); //callback declaration
LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam);
b2MouseJoint* m_mouseJoint = NULL;
class QueryCallback : public b2QueryCallback
{
public:
	QueryCallback(const b2Vec2& point)
	{
		m_point = point;
		m_fixture = NULL;
	}

	bool ReportFixture(b2Fixture* fixture) override
	{
		b2Body* body = fixture->GetBody();
		if (body->GetType() == b2_dynamicBody)
		{
			bool inside = fixture->TestPoint(m_point);
			if (inside)
			{
				m_fixture = fixture;

				// We are done, terminate the query.
				return false;
			}
		}

		// Continue the query.
		return true;
	}

	b2Vec2 m_point;
	b2Fixture* m_fixture;
};
WindowManager::WindowManager()
{
	//HDC hdc = GetDC(NULL);
	//屏幕整体尺寸
	//screenWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
	//screenHeight = GetDeviceCaps(hdc, DESKTOPVERTRES);
	//屏幕整体尺寸
	//screenWidth = GetDeviceCaps(hdc, HORZRES);
	//screenHeight = GetDeviceCaps(hdc, VERTRES);
	//屏幕整体尺寸
	//screenWidth = GetSystemMetrics(SM_CXSCREEN);
	//screenHeight = GetSystemMetrics(SM_CYSCREEN);
	//不包含任务栏的高度
	screenWidth = GetSystemMetrics(SM_CXFULLSCREEN);
	screenHeight = GetSystemMetrics(SM_CYFULLSCREEN);
	//ReleaseDC(NULL, hdc);
	myWorld = new Box2DWorld(screenWidth, screenHeight);
	render = new D2DRender{ GetModuleHandle(NULL), screenWidth, screenHeight, SW_MAXIMIZE };
	Hook::Instance().InstallHook(MyMouseCallback, MyKeyBoardCallback);
	std::thread updateThread(&WindowManager::Update, this);
	updateThread.detach();
}
void WindowManager::Update() {
	while (42) {
		if (myWorld->paused == false)
		{
			for (auto& pair : windowQueue) {
				// 该代码段的功能是暂停游戏世界，获取窗口的矩形区域和物理体的位置角度，
				// 然后将窗口移动到对应的位置，最后恢复游戏世界并检查窗口是否正在被拖动。
				myWorld->paused = true;
				HWND hwnd = (HWND)pair.first;
				RECT rect;
				GetWindowRect(hwnd, &rect);
				pair.second.rect = rect;
				b2Body* body = myWorld->GetBody(pair.first);
				pair.second.angle = body->GetAngle();
				b2Vec2 pos = Box2DWorld::ConvertWorldToScreen(body->GetPosition());
				pos.x -= (rect.right - rect.left) / 2.0f;
				pos.y -= (rect.bottom - rect.top) / 2.0f;
				SetWindowPos(hwnd, 0, pos.x, pos.y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
				myWorld->paused = false;
				std::cout << IsDraging(hwnd) << std::endl;
				//std::cout << pair.second.angle << std::endl;

				if (IsDraging(hwnd) == true && m_mouseJoint == NULL) {
					// 创建一个小的包围盒（AABB）
					b2AABB aabb{};
					b2Vec2 d = { 0.001f, 0.001f };
					POINT cursorPos;
					GetCursorPos(&cursorPos);
					b2Vec2 p = Box2DWorld::ConvertScreenToWorld({ (float)cursorPos.x, (float)cursorPos.y });
					aabb.lowerBound = p - d;
					aabb.upperBound = p + d;

					// 查询世界中与包围盒重叠的形状
					QueryCallback callback(p);
					myWorld->world->QueryAABB(&callback, aabb);

					if (callback.m_fixture)
					{
						// 设置鼠标关节的参数
						float frequencyHz = 5.0f; // 频率
						float dampingRatio = 0.7f; // 阻尼比

						b2Body* body = callback.m_fixture->GetBody();
						b2MouseJointDef jd;
						jd.bodyA = myWorld->wallBody; // 地面物体
						jd.bodyB = body; // 被拖动的物体
						jd.target = p; // 目标位置
						jd.maxForce = 1000.0f * body->GetMass(); // 最大力
						b2LinearStiffness(jd.stiffness, jd.damping, frequencyHz, dampingRatio, jd.bodyA, jd.bodyB); // 线性刚度

						// 创建鼠标关节
						m_mouseJoint = (b2MouseJoint*)myWorld->world->CreateJoint(&jd);
						body->SetAwake(true); // 确保物体是激活状态
					}
				}
				else if (m_mouseJoint != NULL) {
					myWorld->world->DestroyJoint(m_mouseJoint);
					m_mouseJoint = NULL;
				}
			}
			render->Update(WindowManager::windowQueue);
		}
	}
}

bool WindowManager::AddNewWindow(HWND handle) {
	// 尝试截图
	if (WindowsApi::CaptureWindow(handle) == NULL)
		return false;
	SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
	myWorld->paused = true;
	WindowManager::WindowData data{};
	data.angle = 0;
	GetWindowRect(handle, &data.rect);
	data.handle = handle;
	WindowManager::windowQueue[(uintptr_t)handle] = data;
	myWorld->CreateBody(handle, (uintptr_t)handle, { 0,0 });
	myWorld->paused = false;
}
bool isLeftMouseDown = false;
bool WindowManager::IsDraging(HWND handle) {
	if (GetParent(WindowsApi::GetHandleFromCursor(true)) == NULL) {
		if (GetForegroundWindow() == handle) {
			if (isLeftMouseDown) {
				return true;
			}
		}
	}
	return false;
}

MSLLHOOKSTRUCT* pMouseStruct = nullptr;
KBDLLHOOKSTRUCT* pKeyStruct = nullptr;
static LRESULT WINAPI MyMouseCallback(int nCode, WPARAM wParam, LPARAM lParam) {

	pMouseStruct = (MSLLHOOKSTRUCT*)lParam; // WH_MOUSE_LL struct
	/*
	nCode, this parameters will determine how to process a message
	This callback in this case only have information when it is 0 (HC_ACTION): wParam and lParam contain info

	wParam is about WINDOWS MESSAGE, in this case MOUSE messages.
	lParam is information contained in the structure MSLLHOOKSTRUCT
	*/

	if (nCode == 0) { // we have information in wParam/lParam ? If yes, let's check it:
		if ((MSLLHOOKSTRUCT*)lParam != NULL) { // Mouse struct contain information?		
			//if (pDlg != nullptr) {
			//	CString str;
			//	str.Format(_T("Mouse Coordinates: x = %i | y = %i \n"), pMouseStruct->pt.x, pMouseStruct->pt.y);
			//	//pDlg->GetDlgItem(IDC_EDIT_tb)->SetWindowText(str);
			//}
		}
		switch (wParam) {
		case WM_LBUTTONUP: {
			isLeftMouseDown = false;
			printf_s("LEFT CLICK UP\n");
		}break;
		case WM_LBUTTONDOWN: {
			isLeftMouseDown = true;
			printf_s("LEFT CLICK DOWN\n");
		}break;
		case WM_RBUTTONUP: {
			printf_s("RIGHT CLICK UP\n");
		}break;
		case WM_RBUTTONDOWN: {
			printf_s("RIGHT CLICK DOWN\n");
		}break;
		case WM_MOUSEMOVE: {
			printf_s("MOUSE MOVE\n");
			if (m_mouseJoint != NULL) {
				// 直接从 lParam 获取鼠标位置
				POINTS pts = MAKEPOINTS(lParam);
				b2Vec2 p = Box2DWorld::ConvertScreenToWorld({ (float)pts.x, (float)pts.y });
				m_mouseJoint->SetTarget(p);
			}
			break;
		}
		case WM_CHAR: {
			printf_s("%c", (TCHAR)wParam);
		}break;
		}
	}
	/*
	Every time that the nCode is less than 0 we need to CallNextHookEx:
	-> Pass to the next hook
	MSDN: Calling CallNextHookEx is optional, but it is highly recommended;
	otherwise, other applications that have installed hooks will not receive hook notifications and may behave incorrectly as a result.
	*/
	return CallNextHookEx(Hook::Instance().hook, nCode, wParam, lParam);
}

static LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam)
{
	pKeyStruct = (KBDLLHOOKSTRUCT*)lParam;

	if (nCode == 0)
	{
		if (pKeyStruct)
		{
			printf_s("Virtual Key Code: %d \n", pKeyStruct->vkCode);
		}
		switch (wParam)
		{
		case WM_KEYDOWN: {
			printf_s("Sys Key\n");
		}break;
		case WM_SYSKEYDOWN: {
			printf_s("Not Sys Key\n");
		}break;
		}
	}
	return CallNextHookEx(Hook::Instance().keyboardhook, nCode, wParam, lParam);
}

