#include "hook.h"
int Hook::Messsages() {
	while (msg.message != WM_QUIT) { //while we do not close our application
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)) {
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		Sleep(1);
	}
	UninstallHook(); //if we close, let's uninstall our hook
	return (int)msg.wParam; //return the messages
}
void Hook::InstallHook(HOOKPROC mouseProc, HOOKPROC keyProc) {
	/*
	SetWindowHookEx(
	WM_MOUSE_LL = mouse low level hook type,
	MyMouseCallback = our callback function that will deal with system messages about mouse
	NULL, 0);

	c++ note: we can check the return SetWindowsHookEx like this because:
	If it return NULL, a NULL value is 0 and 0 is false.
	*/
	if (!(hook = SetWindowsHookEx(WH_MOUSE_LL, mouseProc, NULL, 0))) {
		printf_s("Error: %x \n", GetLastError());
	}
	if (!(keyboardhook = SetWindowsHookEx(WH_KEYBOARD_LL, keyProc, NULL, 0))) {
		printf_s("Error: %x \n", GetLastError());
	}
}

void Hook::UninstallHook() {
	UnhookWindowsHookEx(hook);
	UnhookWindowsHookEx(keyboardhook);
}