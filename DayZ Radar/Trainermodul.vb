Module Trainermodul
    ' Deklarationen
    Private process_id As Int32 = 0
    Private Const ACCESS_RIGHTS_ALL = &H1F0FFF

    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Int32, ByVal bInerhitHandle As Int32, ByVal dwProcessId As Int32) As Int32
    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Int32) As Int32

    Private Declare Function WPM Lib "kernel32" Alias "WriteProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Byte, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Byte
    Private Declare Function WPM Lib "kernel32" Alias "WriteProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Int32, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Int32
    Private Declare Function WPM Lib "kernel32" Alias "WriteProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Int64, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Int64

    Private Declare Function RPM Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Byte, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Byte
    Private Declare Function RPM Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Int32, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Int32
    Private Declare Function RPM Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As Int32, ByVal lpBaseAddress As Int32, ByRef lpBuffer As Int64, ByVal nSize As Int32, ByRef lpNumberOfBytesWritten As Int32) As Int64

    Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
    Declare Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal newProtect As Integer, ByRef oldProtect As Integer) As Boolean

    Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal Handle As Int32, ByVal Address As Int32, ByRef Value As Long, ByVal Size As Int32, ByRef BytesWritten As Int32) As Long
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal Handle As Int32, ByVal Address As Int32, ByRef Value As Long, ByVal Size As Int32, ByRef BytesWritten As Int32) As Long


    ' ProzessID finden
    Public Function GetProcessId(ByVal ProcName As String)
        Dim Processes() As Process = Process.GetProcesses
        Dim process_name As String
        Dim i As Byte
        For i = LBound(Processes) To UBound(Processes)
            process_name = Processes(i).ProcessName
            If process_name = ProcName Then
                process_id = Processes(i).Id
                Return process_id
            End If
        Next
    End Function

#Region "Write"
    ' 1 Byte in den Prozess schreiben
    Public Sub Write_Byte(ByVal address As Int32, ByVal value As Byte)
        Dim process_handle As Int32
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            WPM(process_handle, address, value, 1, 0)
        End If
        CloseHandle(process_handle)
    End Sub
    ' 4 Bytes in den Przess schreiben
    Public Sub Write_Long(ByVal address As Int32, ByVal value As Int32)
        Dim process_handle As Int32
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            WPM(process_handle, address, value, 4, 0)
        End If
        CloseHandle(process_handle)
    End Sub
    ' 8 Bytes in den Przess schreiben
    Public Sub Write_Float(ByVal address As Int32, ByVal value As Int64)
        Dim process_handle As Int32
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            WPM(process_handle, address, value, 8, 0)
        End If
        CloseHandle(process_handle)
    End Sub
    ' Für die CodeInjection
    Public Sub autopatcher(ByVal address As Int32, ByVal value As Byte())
        Dim i As Byte
        For i = LBound(value) To UBound(value)
            Write_Byte(address + i, value(i))
        Next
    End Sub
#End Region
#Region "Read"
    ' 1 Byte auslesen
    Public Function Read_Byte(ByVal address As Int32) As Byte
        Dim process_handle As Int32, value As Byte
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            RPM(process_handle, address, value, 1, 0)
        End If
        CloseHandle(process_handle)
        Return value
    End Function
    ' 4 Byte auslesen
    Public Function Read_Long(ByVal address As Int32) As Int32
        Dim process_handle As Int32, value As Int32
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            RPM(process_handle, address, value, 4, 0)
        End If
        CloseHandle(process_handle)
        Return value
    End Function
    ' 8 Byte auslesen
    Public Function Read_Float(ByVal address As Int32) As Int64
        Dim process_handle As Int32, value As Int64
        process_handle = OpenProcess(ACCESS_RIGHTS_ALL, False, process_id)
        If process_handle <> 0 Then
            RPM(process_handle, address, value, 8, 0)
        End If
        CloseHandle(process_handle)
        Return value
    End Function
#End Region
#Region "Memory Function"
    ' Speicher hinzufügen
    Sub AllocMem(ByVal ProcessName As String, ByVal AddressOfStart As Integer, ByVal SizeOfAllocationInBytes As Integer)
        For Each p As Process In Process.GetProcessesByName(ProcessName)
            Const MEM_COMMIT As Integer = &H1000
            Const PAGE_EXECUTE_READWRITE As Integer = &H40
            Dim pBlob As IntPtr = VirtualAllocEx(p.Handle, New IntPtr(AddressOfStart), New IntPtr(SizeOfAllocationInBytes), MEM_COMMIT, PAGE_EXECUTE_READWRITE)
            If pBlob = IntPtr.Zero Then Throw New Exception
            p.Dispose()
        Next
    End Sub
    ' Schreibschutz von Speicher entfernen
    Sub RemoveProtection(ByVal ProcessName As String, ByVal AddressOfStart As Integer, ByVal SizeToRemoveProtectionInBytes As Integer)
        For Each p As Process In Process.GetProcessesByName(ProcessName)
            Const PAGE_EXECUTE_READWRITE As Integer = &H40
            Dim oldProtect As Integer
            If Not VirtualProtectEx(p.Handle, New IntPtr(AddressOfStart), New IntPtr(SizeToRemoveProtectionInBytes), PAGE_EXECUTE_READWRITE, oldProtect) Then Throw New Exception
            p.Dispose()
        Next
    End Sub
#End Region

End Module
