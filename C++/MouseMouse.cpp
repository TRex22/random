/*
	edg3
	Ernest Loveland
	Messing around with mouse moving
  */

#include <windows.h>
#include <winuser.h>
#include <math.h>

//#include <stdlib.h>

#include <iostream>
#include <fstream>

using namespace std;

const char dsp[8] = "DISPLAY", txt[10] ="MMove.exe";
bool flag = true;

int main ()
{
	
	HWND stealth;
	HDC hdc;
	hdc = CreateDC(dsp, NULL, NULL, NULL);
	SetBkMode(hdc,0);
	SetTextColor(hdc,1);
    AllocConsole();
    stealth=FindWindowA("ConsoleWindowClass",NULL);
    ShowWindow(stealth,0);

	int a = 0, b = 0;
	while (1 == 1)
	{
		Sleep(5);
		a += 1;
		b += 1;
		SetCursorPos(a,b);
		if (a > 350) { a = 0; }
		if (b > 500) { b = 0; }
	}
	return 0;
}