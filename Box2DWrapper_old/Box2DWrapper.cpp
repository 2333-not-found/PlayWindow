#include "pch.h"
#include "Box2DWrapper.h"
#include <iostream>
#include <string>

using namespace std;

public class MyClass
{
public:
	int  GetValue() { return _value; }
	void SetValue(int num) { _value = num; }
	string GetStrValue() { return _strValue; }
	void SetValue(string str) { _strValue = str; }
	__declspec(property(get = GetValue, put = SetValue)) int value;
	__declspec(property(get = GetStrValue, put = SetValue)) string strValue;
private:
	int _value;
	string _strValue;
	//b2World _b2World;
};

template <typename T>
class MyTempClass
{
public:
	T GetValue() { return _value; }
	void SetValue(T num) { _value = num; }
	__declspec(property(get = GetValue, put = SetValue)) T value;
private:
	T _value;
};

int Box2DWrapper::Class1::GetValue() {
	MyClass myc;
	myc.value = 10;
	myc.strValue = "hello world";
	return myc.GetValue();
}/*
string Box2DWrapper::Class1::GetStrValue() {
	MyClass myc;
	myc.value = 10;
	myc.strValue = "hello world";
	return myc.GetStrValue();
}*/
int main()
{
	MyClass myc;
	myc.value = 10;
	myc.strValue = "hello world";
	cout << myc.value << endl;
	cout << myc.strValue << endl;
	MyTempClass<int> Mytc;
	Mytc.value = 20;
	cout << Mytc.value << endl;
	MyTempClass<string> strMytc;
	strMytc.value = "qwert";
	cout << strMytc.value << endl;
	MyTempClass<double> dMytc;
	dMytc.value = 3.1415;
	cout << dMytc.value << endl;
	return 0;
}
