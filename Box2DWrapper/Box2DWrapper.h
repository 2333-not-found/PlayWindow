// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 TESTCPPDLL_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// BOX2DWRAPPER_API 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
#ifdef BOX2DWRAPPER_EXPORTS
#define BOX2DWRAPPER_API __declspec(dllexport)
#else
#define BOX2DWRAPPER_API __declspec(dllimport)
#endif
extern "C" BOX2DWRAPPER_API int __stdcall Add(int a, int b);
extern "C" BOX2DWRAPPER_API void __stdcall WriteString(wchar_t* content);
//传入一个整型指针，将其所指向的内容加1
extern "C" BOX2DWRAPPER_API void __stdcall AddInt(int* i);
//传入一个整型数组的指针以及数组长度，遍历每一个元素并且输出
extern "C" BOX2DWRAPPER_API void __stdcall AddIntArray(int* firstElement, int arraylength);
//在C++中生成一个整型数组，并且数组指针返回给C#
extern "C" BOX2DWRAPPER_API int* __stdcall GetArrayFromCPP();

//定义一个函数指针
typedef void(__stdcall* CPPCallback)(int tick);
//定义一个用于设置函数指针的方法,
//并在该函数中调用C#中传递过来的委托
extern "C" BOX2DWRAPPER_API void __stdcall SetCallback(CPPCallback _callback);

struct Vector3
{
	float X, Y, Z;
};
extern "C" BOX2DWRAPPER_API void __stdcall SendStructFromCSToCPP(Vector3 vector);
extern "C" BOX2DWRAPPER_API void __stdcall SendStruct(b2BodyDef body);
extern "C" BOX2DWRAPPER_API void __stdcall RunEngine(int _screenHeight, int _screenWidth);
extern "C" BOX2DWRAPPER_API void __stdcall AddBody(HWND intptr, b2Vec2 targetPos, b2BodyUserData * userData);