#include "..\Utility\Render.h"
#include "WindowManager.h"
#include <windows.h>
#include <thread>

int screenWidth = 1024;
int screenHeight = 768;
D2DRender* render = NULL;
LRESULT WINAPI MyMouseCallback(int nCode, WPARAM wParam, LPARAM lParam);
LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam);

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
    //��Ļ����ߴ�
    //screenWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
    //screenHeight = GetDeviceCaps(hdc, DESKTOPVERTRES);
    //��Ļ����ߴ�
    //screenWidth = GetDeviceCaps(hdc, HORZRES);
    //screenHeight = GetDeviceCaps(hdc, VERTRES);
    //��Ļ����ߴ�
    //screenWidth = GetSystemMetrics(SM_CXSCREEN);
    //screenHeight = GetSystemMetrics(SM_CYSCREEN);
    //�������������ĸ߶�
    screenWidth = GetSystemMetrics(SM_CXFULLSCREEN);
    screenHeight = GetSystemMetrics(SM_CYFULLSCREEN);
    //ReleaseDC(NULL, hdc);
    myWorld = new Box2DWorld(screenWidth, screenHeight);
    render = new D2DRender{ GetModuleHandle(NULL), screenWidth, screenHeight, SW_MAXIMIZE };
    Hook::Instance().InstallHook(MyMouseCallback, MyKeyBoardCallback);
    std::thread updateThread(&WindowManager::Update, this);
    updateThread.detach();
}
WindowManager::~WindowManager() {
    for (auto pair : windowQueue) {
        HWND handle = (HWND)pair.first;
        //���ô���͸����
        SetWindowLong(handle, GWL_EXSTYLE, GetWindowLong(handle, GWL_EXSTYLE) | WS_EX_LAYERED);
        SetLayeredWindowAttributes(handle, 0, 255, LWA_ALPHA);
    }
    delete myWorld;
    delete render;
    Hook::Instance().UninstallHook();
}
b2MouseJoint* m_mouseJoint = NULL;
b2Vec2 m_mouseWorld = { 0,0 };
QueryCallback callback(b2Vec2{ 0,0 });
bool isLeftMouseDown = false;
void WindowManager::Update() {
    while (42) {
        if (myWorld->paused == false)
        {
            for (auto& pair : windowQueue) {
                if (!IsWindow((HWND)pair.first)) {
                    //windowQueue.erase(pair.first);
                    continue;
                }

                myWorld->paused = true;
                HWND hwnd = (HWND)pair.first;
                RECT rect;
                GetWindowRect(hwnd, &rect);

                // �жϴ��ڴ�С�Ƿ�ı�
                if (pair.second.rect.right - pair.second.rect.left != rect.right - rect.left ||
                    pair.second.rect.bottom - pair.second.rect.top != rect.bottom - rect.top) {
                    // ���ڴ�С�ı䣬���������λ�ýǶ�
                    myWorld->SetBodyRectangle((uintptr_t)hwnd, rect);
                }

                b2Body* body = myWorld->GetBody(pair.first);
                pair.second.angle = body->GetAngle();
                b2Vec2 bodyPos = Box2DWorld::ConvertWorldToScreen(body->GetPosition());

                bodyPos.x -= (rect.right - rect.left) / 2.0f;
                bodyPos.y -= (rect.bottom - rect.top) / 2.0f;

                pair.second.bodyPosX = bodyPos.x;
                pair.second.bodyPosY = bodyPos.y;

                // SetWindowPos(hwnd, 0, bodyPos.x, bodyPos.y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
                myWorld->paused = false;


                if (IsDraging(hwnd) == true)
                {
                    pair.second.isDraging = true;
                    if (m_mouseJoint == NULL)
                    {
                        // ����һ��С�İ�Χ�У�AABB��
                        b2AABB aabb{};
                        b2Vec2 d = { 0.001f, 0.001f };
                        POINT cursorPos;
                        GetCursorPos(&cursorPos);
                        m_mouseWorld = Box2DWorld::ConvertScreenToWorld({ (float)cursorPos.x, (float)cursorPos.y });
                        aabb.lowerBound = m_mouseWorld - d;
                        aabb.upperBound = m_mouseWorld + d;

                        // ��ѯ���������Χ���ص�����״
                        callback = QueryCallback(m_mouseWorld);
                        myWorld->world->QueryAABB(&callback, aabb);

                        if (callback.m_fixture != NULL)
                        {
                            // �������ؽڵĲ���
                            float frequencyHz = 5.0f; // Ƶ��
                            float dampingRatio = 0.7f; // �����

                            b2Body* body = callback.m_fixture->GetBody();
                            b2MouseJointDef jd;
                            jd.bodyA = myWorld->wallBody; // ��������
                            jd.bodyB = body; // ���϶�������
                            jd.target = m_mouseWorld; // Ŀ��λ��
                            jd.maxForce = 1000.0f * body->GetMass(); // �����
                            b2LinearStiffness(jd.stiffness, jd.damping, frequencyHz, dampingRatio, jd.bodyA, jd.bodyB); // ���Ըն�						
                            m_mouseJoint = (b2MouseJoint*)myWorld->world->CreateJoint(&jd);// �������ؽ�
                            body->SetAwake(true); // ȷ�������Ǽ���״̬
                        }
                    }
                }
                else {
                    pair.second.isDraging = false;
                    RECT area = pair.second.rect;
                    RECT realRect;
                    GetWindowRect(hwnd, &realRect);
                    POINT cursorPos;
                    GetCursorPos(&cursorPos);
                    if (cursorPos.x > realRect.left && cursorPos.x < realRect.right && cursorPos.y > realRect.top && cursorPos.y < realRect.bottom)
                    {
                        POINT p1 = cursorPos;
                        POINT centrePoint{ area.left + (area.right - area.left) / 2,  area.top + (area.bottom - area.top) / 2 };
                        POINT p2{};
                        //RotateAngle:��������1�Ե�(centerPoint)Ϊ������ת������
                        double Angle = pair.second.angle * (180 / 3.14159265358979323846);
                        double Rad = Angle * acos(-1) / 180;
                        p2.x = (p1.x - centrePoint.x) * cos(Rad) - (p1.y - centrePoint.y) * sin(Rad) + centrePoint.x;
                        p2.y = (p1.y - centrePoint.y) * cos(Rad) + (p1.x - centrePoint.x) * sin(Rad) + centrePoint.y;

                        SetWindowPos(hwnd, (HWND)-2, (int)(bodyPos.x + (p1.x - p2.x)), (int)(bodyPos.y + (p1.y - p2.y)), 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
                    }
                    else
                    {
                        SetWindowPos(hwnd, (HWND)-2, (int)bodyPos.x, (int)bodyPos.y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);

                        GetWindowRect(hwnd, &pair.second.rect);
                    }
                }
            }
            if (isLeftMouseDown == false && m_mouseJoint != NULL) {
                myWorld->world->DestroyJoint(m_mouseJoint);
                m_mouseJoint = NULL;
            }

            render->Update(WindowManager::windowQueue);
        }
    }
}

bool WindowManager::AddNewWindow(HWND handle) {
    handle = GetAncestor(handle, GA_ROOT);
    // ���Խ�ͼ
    if (WindowsApi::CaptureWindowByBitblt(handle) == NULL)
        return false;
    SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
    //���ô���͸����
    SetWindowLong(handle, GWL_EXSTYLE, GetWindowLong(handle, GWL_EXSTYLE) | WS_EX_LAYERED);
    SetLayeredWindowAttributes(handle, 0, 10, LWA_ALPHA);
    myWorld->paused = true;
    WindowManager::WindowData data{};
    data.angle = 0;
    GetWindowRect(handle, &data.rect);
    data.handle = handle;
    WindowManager::windowQueue[(uintptr_t)handle] = data;
    myWorld->CreateBody(handle, (uintptr_t)handle, { 0,0 });
    myWorld->paused = false;
}
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
        }break;
        case WM_LBUTTONDOWN: {
            isLeftMouseDown = true;
        }break;
        case WM_RBUTTONUP: {
        }break;
        case WM_RBUTTONDOWN: {
        }break;
        case WM_MOUSEMOVE: {
            if (m_mouseJoint != NULL && pMouseStruct != NULL) {
                b2Vec2 p = Box2DWorld::ConvertScreenToWorld({ (float)pMouseStruct->pt.x, (float)pMouseStruct->pt.y });
                m_mouseWorld = p;
                m_mouseJoint->SetTarget(p);
            }
            break;
        }
        case WM_CHAR: {
            //printf_s("%c", (TCHAR)wParam);
        }break;
        }
    }
    return CallNextHookEx(Hook::Instance().hook, nCode, wParam, lParam);
}

static LRESULT WINAPI MyKeyBoardCallback(int nCode, WPARAM wParam, LPARAM lParam)
{
    pKeyStruct = (KBDLLHOOKSTRUCT*)lParam;

    if (nCode == 0)
    {
        if (pKeyStruct)
        {
            //printf_s("Virtual Key Code: %d \n", pKeyStruct->vkCode);
        }
        switch (wParam)
        {
        case WM_KEYDOWN: {
        }break;
        case WM_SYSKEYDOWN: {
        }break;
        }
    }
    return CallNextHookEx(Hook::Instance().keyboardhook, nCode, wParam, lParam);
}

