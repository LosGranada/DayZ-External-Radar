Public Class Form1
    Private Const ProcName = "DayZ"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim p As Process() = Process.GetProcessesByName(game.Text)
        GetProcessId(ProcName)

        x.Text = Read_Long(&HFAF868)
        y.Text = Read_Long(&HFAF870)
        z.Text = Read_Long(&HFAF86C)
    End Sub
End Class
