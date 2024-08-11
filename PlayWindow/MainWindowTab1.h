#pragma once
#include "afxdialogex.h"

// MainWindowTab1 对话框

class MainWindowTab1 : public CDialogEx
{
	DECLARE_DYNAMIC(MainWindowTab1)

public:
	MainWindowTab1(CWnd* pParent = nullptr);   // 标准构造函数
	virtual ~MainWindowTab1();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_DIALOG_TAB1 };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedAddwindow();

};