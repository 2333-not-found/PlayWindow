#include "afxdialogex.h"
#include "MainWindowTab2.h"
#include "PlayWindow.h"
#include "PlayWindowDlg.h"

extern CPlayWindowDlg* pDlg;
IMPLEMENT_DYNAMIC(MainWindowTab2, CDialogEx)
MainWindowTab2::MainWindowTab2(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_DIALOG_TAB2, pParent)
{/*
	CSpinButtonCtrl* spin_x= (CSpinButtonCtrl*)GetDlgItem(SPIN_SetPos_X);
	CSpinButtonCtrl* spin_y= (CSpinButtonCtrl*)GetDlgItem(SPIN_SetPos_Y);
	spin_x->SetRange32(-2147483647, 2147483647);
	spin_y->SetRange32(-2147483647, 2147483647);
	*/
}
MainWindowTab2::~MainWindowTab2()
{
}
void MainWindowTab2::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}
BEGIN_MESSAGE_MAP(MainWindowTab2, CDialogEx)
	ON_BN_CLICKED(BTN_SetPos, &MainWindowTab2::OnBnClickedSetpos)
	ON_BN_CLICKED(BTN_AddImpulse, &MainWindowTab2::OnBnClickedAddimpulse)
	ON_BN_CLICKED(BTN_SetRotation, &MainWindowTab2::OnBnClickedSetrotation)
END_MESSAGE_MAP()

void MainWindowTab2::OnBnClickedSetpos()
{
	CString str, _x, _y;
	GetDlgItem(EDIT_SetPos_WindowHandle)->GetWindowTextW(str);
	GetDlgItem(EDIT_SetPos_X)->GetWindowTextW(_x);
	GetDlgItem(EDIT_SetPos_Y)->GetWindowTextW(_y);
	pDlg->wm.myWorld->SetBodyPos(_ttoi(str), { (float)_ttoi(_x),(float)_ttoi(_y) });;
}

void MainWindowTab2::OnBnClickedAddimpulse()
{
	CString str, _x, _y;
	GetDlgItem(EDIT_AddImpulse_WindowHandle)->GetWindowTextW(str);
	GetDlgItem(EDIT_AddImpulse_X)->GetWindowTextW(_x);
	GetDlgItem(EDIT_AddImpulse_Y)->GetWindowTextW(_y);
	pDlg->wm.myWorld->AddImpulse(_ttoi(str), { (float)_ttoi(_x),(float)_ttoi(_y) });;
}

void MainWindowTab2::OnBnClickedSetrotation()
{
	CString str, rad;
	GetDlgItem(EDIT_SetRotation_WindowHandle)->GetWindowTextW(str);
	GetDlgItem(EDIT_SetRotation_Radian)->GetWindowTextW(rad);
	pDlg->wm.myWorld->SetBodyRotation(_ttoi(str), (float)_ttoi(rad));;
}