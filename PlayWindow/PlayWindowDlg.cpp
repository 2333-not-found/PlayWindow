#include "afxdialogex.h"
#include "PlayWindow.h"
#include "PlayWindowDlg.h"
#include "windows.h"
#include <string>
#include <thread>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	DECLARE_MESSAGE_MAP()
};
CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}
void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}
BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()
CPlayWindowDlg::CPlayWindowDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_PLAYWINDOW_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}
void CPlayWindowDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, TAB_ControlPanel);
}
BEGIN_MESSAGE_MAP(CPlayWindowDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &CPlayWindowDlg::OnBnClickedOk)
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &CPlayWindowDlg::OnTcnSelchangeTab1)
	ON_BN_CLICKED(BTN_QuickAdd, &CPlayWindowDlg::OnBnClickedQuickadd)
END_MESSAGE_MAP()
CPlayWindowDlg* pDlg;
BOOL CPlayWindowDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	pDlg = this;

	//获取tab control位置和大小
	CRect tabRect, itemRect;
	int nX, nY, nXc, nYc;
	TAB_ControlPanel.GetClientRect(&tabRect);
	TAB_ControlPanel.GetItemRect(0, &itemRect);
	nX = itemRect.left;
	nY = itemRect.bottom + 1;
	nXc = tabRect.right - itemRect.left - 2;
	nYc = tabRect.bottom - nY - 2;

	// 添加对话框1
	TAB_ControlPanel.InsertItem(0, TEXT("对话框1"));
	TAB_ControlPanel1.Create(IDD_DIALOG_TAB1, &TAB_ControlPanel);
	TAB_ControlPanel_Sum[0] = &TAB_ControlPanel1;
	TAB_ControlPanel_Sum[0]->ShowWindow(SW_SHOW);
	//设置对话框1的显示位置
	TAB_ControlPanel_Sum[0]->SetWindowPos(&wndTop, nX, nY, nXc, nYc, SWP_SHOWWINDOW);

	//添加对话框2
	TAB_ControlPanel.InsertItem(1, TEXT("对话框2"));
	TAB_ControlPanel2.Create(IDD_DIALOG_TAB2, &TAB_ControlPanel);
	TAB_ControlPanel_Sum[1] = &TAB_ControlPanel2;
	TAB_ControlPanel_Sum[1]->ShowWindow(SW_HIDE);
	TAB_ControlPanel_Sum[1]->SetWindowPos(&wndTop, nX, nY, nXc, nYc, SWP_HIDEWINDOW);
	/*
	//添加对话框3
	TAB_ControlPanel.InsertItem(2, TEXT("对话框3"));
	m_sub3.Create(IDD_DIALOG_SUB3, &TAB_ControlPanel);
	TAB_ControlPanel_Sum[2] = &m_sub3;
	TAB_ControlPanel_Sum[2]->ShowWindow(SW_HIDE);
	TAB_ControlPanel_Sum[2]->SetWindowPos(&wndTop, nX, nY, nXc, nYc, SWP_HIDEWINDOW);
	*/
	return TRUE;
}

void CPlayWindowDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}
void CPlayWindowDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this);

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}
HCURSOR CPlayWindowDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

static void QuickAddMethod() {
	pDlg->GetDlgItem(BTN_QuickAdd)->EnableWindow(0);
	int seconds = 5;
	CString str;
	for (int i = seconds; i >= 0; i--)
	{
		std::this_thread::sleep_for(std::chrono::milliseconds(1000));
		str.Format(_T("%d"), i);
		pDlg->GetDlgItem(BTN_QuickAdd)->SetWindowText(str);
	}
	HWND hwnd = WindowsApi::GetHandleFromCursor(true);
	str.Format(_T("%d"), (int)hwnd);
	pDlg->wm.AddNewWindow(hwnd);
	pDlg->GetDlgItem(BTN_QuickAdd)->EnableWindow(1);
	pDlg->GetDlgItem(BTN_QuickAdd)->SetWindowText(str);
}
void CPlayWindowDlg::OnBnClickedQuickadd()
{
	std::thread oneThread(&QuickAddMethod);
	oneThread.detach();
}

void CPlayWindowDlg::OnBnClickedOk()
{
	CDialogEx::OnOK();
}

void CPlayWindowDlg::OnTcnSelchangeTab1(NMHDR* pNMHDR, LRESULT* pResult)
{
	*pResult = 0;
	//获取当前选择索引
	int index = TAB_ControlPanel.GetCurSel();
	//根据索引显示对应的界面
	if (index == 0) {
		TAB_ControlPanel_Sum[0]->ShowWindow(SW_SHOW);
		TAB_ControlPanel_Sum[1]->ShowWindow(SW_HIDE);
		//TAB_ControlPanel_Sum[2]->ShowWindow(SW_HIDE);
	}
	else if (index == 1) {
		TAB_ControlPanel_Sum[0]->ShowWindow(SW_HIDE);
		TAB_ControlPanel_Sum[1]->ShowWindow(SW_SHOW);
		//TAB_ControlPanel_Sum[2]->ShowWindow(SW_HIDE);
	}/*
	else if (index == 2) {
		TAB_ControlPanel_Sum[0]->ShowWindow(SW_HIDE);
		TAB_ControlPanel_Sum[1]->ShowWindow(SW_HIDE);
		TAB_ControlPanel_Sum[2]->ShowWindow(SW_SHOW);
	}*/
}