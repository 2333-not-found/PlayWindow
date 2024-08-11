#include <Windows.h>
#include <iostream>
#include <thread> 
#include <functional>

class Hook {
public:
	//single ton
	static Hook& Instance() {
		static Hook myHook;
		return myHook;
	}
	HHOOK hook; // handle to the hook	
	void InstallHook(HOOKPROC mouseProc, HOOKPROC keyProc); // function to install our hook
	void UninstallHook(); // function to uninstall our hook

	MSG msg; // struct with information about all messages in our queue
	int Messsages(); // function to "deal" with our messages 

	HHOOK keyboardhook;
};