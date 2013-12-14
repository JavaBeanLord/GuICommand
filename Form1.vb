Public Class Form1
    Private WithEvents MyProcess As Process
    Private Delegate Sub AppendOutputTextDelegate(ByVal text As String)

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Timer1.Enabled = True

        Me.AcceptButton = ExecuteButton
        MyProcess = New Process
        With MyProcess.StartInfo
            .FileName = "CMD.EXE"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardInput = True
            .RedirectStandardOutput = True
            .RedirectStandardError = True
        End With
        MyProcess.Start()

        MyProcess.BeginErrorReadLine()
        MyProcess.BeginOutputReadLine()
        AppendOutputText("Process Started at: " & MyProcess.StartTime.ToString)
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MyProcess.StandardInput.WriteLine("EXIT") 'send an EXIT command to the Command Prompt
        MyProcess.StandardInput.Flush()
        MyProcess.Close()
    End Sub

    Private Sub MyProcess_ErrorDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.ErrorDataReceived
        AppendOutputText(vbCrLf & "Error: " & e.Data)
    End Sub

    Private Sub MyProcess_OutputDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.OutputDataReceived
        AppendOutputText(vbCrLf & e.Data)
    End Sub

    Private Sub ExecuteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExecuteButton.Click
        If InputTextBox.Text = Nothing Then
            MyProcess.StandardInput.WriteLine("chkdsk")
            MyProcess.StandardInput.Flush()
            InputTextBox.Text = ""
            My.Settings.exme = ("chkdsk")
        Else
            MyProcess.StandardInput.WriteLine(InputTextBox.Text)
            MyProcess.StandardInput.Flush()
            My.Settings.exme = InputTextBox.Text
            InputTextBox.Text = ""
        End If

    End Sub
    ''had to add catch
    Private Sub AppendOutputText(ByVal text As String)
        Try
            If OutputTextBox.InvokeRequired Then
                Dim myDelegate As New AppendOutputTextDelegate(AddressOf AppendOutputText)
                Me.Invoke(myDelegate, text)
            Else
                OutputTextBox.AppendText(text)
            End If
        Catch ex As Exception
        End Try

    End Sub


    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If InputTextBox.Text = Nothing Then
            PictureBox1.BackColor = Color.LawnGreen
            Label1.BackColor = Color.LawnGreen
            Label1.Text = ("Ready To Scan HardDisk!")
        Else
            PictureBox1.BackColor = Color.CornflowerBlue
            Label1.BackColor = Color.CornflowerBlue
            Label1.Text = ("Ready To: ") & InputTextBox.Text.ToString

        End If


        If My.Settings.exme = Nothing Then
            PictureBox2.BackColor = Color.Yellow
            Label2.BackColor = Color.Yellow
            Label2.Text = ("Last Execute Not Found!")
        Else
            PictureBox2.BackColor = Color.CornflowerBlue
            Label2.BackColor = Color.CornflowerBlue
            Label2.Text = ("Last Execute: ") & My.Settings.exme
        End If

        If InputTextBox.Text = Nothing Then
            Label3.BackColor = Color.Red
            PictureBox3.BackColor = Color.Red
            Label3.Text = ("Execute Will Run CHKDSK!")
        Else
            Label3.BackColor = Color.LawnGreen
            PictureBox3.BackColor = Color.LawnGreen
            Label3.Text = ("Execute Will Run Input!")
        End If


    End Sub
End Class
