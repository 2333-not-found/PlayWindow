#pragma once
#include "..\D3DRender\D3DRender.h"
#include "..\Utility\WindowManager.h"
#include "MainWindowTab1.h"
#include "MainWindowTab2.h"

// CPlayWindowDlg 对话框
class CPlayWindowDlg : public CDialogEx
{
	// 构造
public:
	CPlayWindowDlg(CWnd* pParent = nullptr);	// 标准构造函数

	// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_PLAYWINDOW_DIALOG };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


	// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
	WindowManager wm;
	D3DRender render{ AfxGetApp()->m_hInstance, 0, 0 };

	CTabCtrl TAB_ControlPanel;
	MainWindowTab1 TAB_ControlPanel1;
	MainWindowTab2 TAB_ControlPanel2;
	CDialog* TAB_ControlPanel_Sum[5];
	afx_msg void OnTcnSelchangeTab1(NMHDR* pNMHDR, LRESULT* pResult);
};
