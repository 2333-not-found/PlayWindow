#pragma once
#include "afxdialogex.h"

// MainWindowTab2 对话框

class MainWindowTab2 : public CDialogEx
{
	DECLARE_DYNAMIC(MainWindowTab2)

public:
	MainWindowTab2(CWnd* pParent = nullptr);   // 标准构造函数
	virtual ~MainWindowTab2();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_DIALOG_TAB2 };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedSetpos();

	//CPlayWindowDlg* pDlg = nullptr;
	afx_msg void OnBnClickedAddimpulse();
	afx_msg void OnBnClickedSetrotation();
};