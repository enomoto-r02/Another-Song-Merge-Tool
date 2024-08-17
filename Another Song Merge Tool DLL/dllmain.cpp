#include <windows.h>
#include <stdio.h>
#include <tchar.h>

extern "C" {
    __declspec (dllexport) void Init()
    {
        STARTUPINFO si;
        PROCESS_INFORMATION pi;

        ZeroMemory(&si, sizeof(si));
        si.cb = sizeof(si);
        ZeroMemory(&pi, sizeof(pi));

        // カレントディレクトリをexeを格納しているフォルダに移動（setting.ini読み込みのため）
        //SetCurrentDirectory(L"rom");

        // exeを実行
        if (CreateProcess(L"Another Song Merge Tool.exe", NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi))
        {
            // 子プロセスの終了を待つ
            WaitForSingleObject(pi.hProcess, INFINITE);

            // ハンドルをクローズ
            CloseHandle(pi.hProcess);
            CloseHandle(pi.hThread);
        }

        // カレントディレクトリを元に戻す（親ディレクトリに移動）
        //SetCurrentDirectory(L"..");
    }
}