Module Memory
    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As Integer
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Integer, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Private Declare Function WriteFloatMemory Lib "kernel32" Alias "WriteProcessMemory" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Single, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Private Declare Function ReadFloat Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByRef buffer As Single, ByVal size As Int32, ByRef lpNumberOfBytesRead As Int32) As Boolean
    Private Declare Function ReadProcessMemory Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Integer, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer
    Public RBuff As Long
    Public RBuff2 As Single
    Public RBuff3 As Integer

    Public Function Writememory(ByVal ProcessName As Process, ByVal Address As Integer, ByVal Value As Long, ByVal Bytes As Integer)
        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If
        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        WriteProcessMemory(processHandle, Address, Value, Bytes, Nothing)

        CloseHandle(processHandle)

    End Function

    Public Function ReadFloat(ByVal ProcessName As Process, ByVal Address As Single)

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Address, RBuff, 4, Nothing)

        CloseHandle(processHandle)

        Return RBuff

    End Function

    Public Function WriteFloat(ByVal ProcessName As Process, ByVal Address As Integer, ByVal Value As Single)

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        WriteFloatMemory(processHandle, Address, Value, 4, Nothing)

        CloseHandle(processHandle)

    End Function

    Public Function ReadLong(ByVal ProcessName As Process, ByVal Address As Integer)

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Address, RBuff, 4, Nothing)

        CloseHandle(processHandle)

        Return RBuff

    End Function

    Public Function ReadFloatPointer(ByVal ProcessName As Process, ByVal Base As Integer, ByVal Offset As Short)

        Dim fullAddress As Long

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Base, RBuff, 4, Nothing)

        fullAddress = RBuff + Offset

        ReadFloat(processHandle, fullAddress, RBuff2, 4, Nothing)

        Return RBuff2

        CloseHandle(processHandle)

    End Function

    Public Function ReadLongPointer(ByVal ProcessName As Process, ByVal Base As Integer, ByVal Offset As Short, ByVal Bytes As Integer)

        Dim fullAddress As Long

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Base, RBuff, 4, Nothing)

        fullAddress = RBuff + Offset

        ReadProcessMemory(processHandle, fullAddress, RBuff3, Bytes, Nothing)

        Return RBuff3

        CloseHandle(processHandle)

    End Function

    Public Function WriteFloatPointer(ByVal ProcessName As Process, ByVal Base As Integer, ByVal Offset As Short, ByVal Value As Single)

        Dim fullAddress As Long

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Base, RBuff, 4, Nothing)

        fullAddress = RBuff + Offset

        WriteFloatMemory(processHandle, fullAddress, Value, 4, Nothing)

        CloseHandle(processHandle)

    End Function

    Public Function WriteLongPointer(ByVal ProcessName As Process, ByVal Base As Integer, ByVal Offset As Short, ByVal Value As Long, ByVal Bytes As Integer)

        Dim fullAddress As Long

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        ReadProcessMemory(processHandle, Base, RBuff, 4, Nothing)

        fullAddress = RBuff + Offset

        WriteProcessMemory(processHandle, fullAddress, Value, Bytes, Nothing)

        CloseHandle(processHandle)

    End Function

    Public Function NOP(ByVal ProcessName As Process, ByVal Address As Integer, ByVal value As Integer)

        Dim GameLookUp As Process() = Process.GetProcessesByName(ProcessName.ProcessName)

        If GameLookUp.Length = 0 Then

            End

        End If

        Dim processHandle As IntPtr = OpenProcess(&H1F0FFF, 0, GameLookUp(0).Id)

        WriteProcessMemory(processHandle, Address, value, 1, Nothing)

        CloseHandle(processHandle)

    End Function

End Module