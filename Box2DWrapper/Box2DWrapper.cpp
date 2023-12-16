//#include "stdafx.h"
#include "pch.h"
#include <iostream>
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

BOX2DWRAPPER_API void __stdcall SetCallback(CPPCallback callback)
{
    int tick = 100;
    //下面的代码是对C#中委托进行调用
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