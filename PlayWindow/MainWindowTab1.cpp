#include "afxdialogex.h"
#include "MainWindowTab1.h"
#include "PlayWindow.h"
#include "PlayWindowDlg.h"
#include "Windows.h"
#include "winuser.h"

extern CPlayWindowDlg* pDlg;
IMPLEMENT_DYNAMIC(MainWindowTab1, CDialogEx)
MainWindowTab1::MainWindowTab1(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_DIALOG_TAB1, pParent)
{
}
MainWindowTab1::~MainWindowTab1()
{
}
void MainWindowTab1::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}
BEGIN_MESSAGE_MAP(MainWindowTab1, CDialogEx)
	ON_BN_CLICKED(BTN_AddWindow, &MainWindowTab1::OnBnClickedAddwindow)
END_MESSAGE_MAP()

void MainWindowTab1::OnBnClickedAddwindow()
{
	CString str;
	GetDlgItem(TB_WindowHandle)->GetWindowTextW(str);
	pDlg->wm.AddNewWindow((HWND)_ttoi(str));
}