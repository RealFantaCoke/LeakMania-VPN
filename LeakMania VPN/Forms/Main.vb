Imports System.Net

Public Class Main

    Private SI As New SoftwareInfos

    Private title As String = SI.getName + " - v" + SI.getVersion

    Public UserProfile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
    Public AppData As String = UserProfile & "\AppData\Roaming"
    Public Bin As String = AppData & "\LeakManiaVPN"
    Public ConfigDir As String = Bin & "\CONFIG"
    Public HTMLDir As String = Bin & "\HTML"
    Public OpenVPNDir As String = Bin & "\OpenVPN"
    Public DirArray() As String = { _
        Bin,
        ConfigDir,
        HTMLDir,
        OpenVPNDir
    }

    Public ServerIPList() As String = { _
        "51.255.118.64"
    }

    Public ua As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.75 Safari/537.36"

    Public PSI As New ProcessStartInfo

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Visible = False
        WebBrowserHelper.FixBrowserVersion()
        Me.Text = title
        Setup()
        OpenVPNConsoleEncoding = System.Text.Encoding.GetEncoding(866)
        PSI.CreateNoWindow = True
        PSI.UseShellExecute = False
        PSI.FileName = OpenVPNDir & "\openvpn.exe"
        GUI.Navigate("file:///" + IO.Path.GetFullPath(HTMLDir & "/home.html"))
        Do Until GUI.ReadyState = WebBrowserReadyState.Complete
            Application.DoEvents()
        Loop
        Me.Visible = True
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        For Each Process In Diagnostics.Process.GetProcesses()
            If Process.ProcessName = "openvpn" Then
                Process.Kill()
                Process.WaitForExit()
            End If
        Next
        End
    End Sub

    Private OpenVPNConsoleProcess As Process
    Private OpenVPNConsoleEncoding As System.Text.Encoding

    Private Sub GUI_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles GUI.DocumentCompleted
        Dim element As HtmlElement = GUI.Document.GetElementById("action")
        Try
            Select Case element.InnerHtml.ToLower
                Case "connectdisconnect"
                    element = GUI.Document.GetElementById("ConnectDisconnect")
                    Select Case element.InnerHtml.ToLower
                        Case "connect"
                            element = GUI.Document.GetElementById("ServerList")
                            Connect(element.InnerText.ToUpper)
                        Case "disconnect"
                            Disconnect()
                    End Select
                Case "checkconnection"
                    Select Case CheckConnection()
                        Case True
                            MsgBox("You're connected !", MsgBoxStyle.Information)
                        Case False
                            MsgBox("You aren't connected !", MsgBoxStyle.Exclamation)
                    End Select
                Case "installdriver"
                    Process.Start(OpenVPNDir & "\tap-windows.exe")
                Case "geoip"
                    GeoIP.Locate()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Sub Connect(Server As String)
        Dim args As String = "--config " & ConfigDir & "\LeakMania-" & Server.Replace(" ", "") & ".ovpn"
        PSI.Arguments = args
        PSI.RedirectStandardInput = True
        PSI.RedirectStandardError = True
        PSI.StandardErrorEncoding = OpenVPNConsoleEncoding
        PSI.RedirectStandardOutput = True
        PSI.StandardOutputEncoding = OpenVPNConsoleEncoding
        Try
            OpenVPNConsoleProcess = Process.Start(PSI)
            AddHandler OpenVPNConsoleProcess.OutputDataReceived, AddressOf OutputDataReceived
            AddHandler OpenVPNConsoleProcess.ErrorDataReceived, AddressOf OutputDataReceived
            OpenVPNConsoleProcess.BeginOutputReadLine()
            OpenVPNConsoleProcess.BeginErrorReadLine()
        Catch ex As Exception
            MsgBox(ErrorToString, MsgBoxStyle.Critical)
        End Try
        Dim processes As New List(Of String)
        For Each Process In Diagnostics.Process.GetProcesses()
            processes.Add(Process.ProcessName)
        Next
        If Not processes.Contains("openvpn") Then
            MsgBox("An error occured !", MsgBoxStyle.Exclamation)
        Else
            ExecJS({"setText('ConnectDisconnect', 'Disconnect');"})
        End If
    End Sub

    Public Sub Disconnect()
        For Each Process In Diagnostics.Process.GetProcesses()
            If Process.ProcessName = "openvpn" Then
                Process.Kill()
                Process.WaitForExit()
            End If
        Next
        OpenVPNConsole.Text = "Disconnected."
        ExecJS({"setText('ConnectDisconnect', 'Connect');"})
    End Sub

    Private Sub OutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        Me.Invoke(CType(AddressOf Me.ThreadProcSetter, Action(Of Object)), e.Data)
    End Sub

    Private Sub ThreadProcSetter(ByVal text As Object)
        OpenVPNConsole.AppendText(DirectCast(text, String) & ControlChars.NewLine)
        OpenVPNConsole.SelectionStart = OpenVPNConsole.TextLength
        OpenVPNConsole.ScrollToCaret()
    End Sub

    Private Sub InstallDriverButton_Click(sender As Object, e As EventArgs)
        Process.Start(OpenVPNDir & "\tap-windows.exe")
    End Sub

    Public Function CheckConnection() As Boolean
        Try
            Dim wb As New WebBrowser()
            wb.Navigate("http://api.leakmania.com/vpn/verify.php")
            Do Until wb.ReadyState = WebBrowserReadyState.Complete
                Application.DoEvents()
            Loop
            For Each IP As String In ServerIPList
                If wb.DocumentText.Contains(IP) Then
                    Return True
                End If
            Next
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Sub ExecJS(JSCode As Object)
        GUI.Document.InvokeScript("eval", JSCode)
    End Sub

    Public Sub Setup()
        For Each d As String In DirArray
            If System.IO.Directory.Exists(d) = False Then
                System.IO.Directory.CreateDirectory(d)
            End If
        Next
        If IO.File.Exists(OpenVPNDir & "\libcrypto-1_1-x64.dll") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\libcrypto-1_1-x64.dll", My.Resources.libcrypto_1_1_x64)
        End If
        If IO.File.Exists(OpenVPNDir & "\liblzo2-2.dll") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\liblzo2-2.dll", My.Resources.liblzo2_2)
        End If
        If IO.File.Exists(OpenVPNDir & "\libpkcs11-helper-1.dll") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\libpkcs11-helper-1.dll", My.Resources.libpkcs11_helper_1)
        End If
        If IO.File.Exists(OpenVPNDir & "\libssl-1_1-x64.dll") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\libssl-1_1-x64.dll", My.Resources.libssl_1_1_x64)
        End If
        If IO.File.Exists(OpenVPNDir & "\openssl.exe") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\openssl.exe", My.Resources.openssl)
        End If
        If IO.File.Exists(OpenVPNDir & "\openvpn.exe") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\openvpn.exe", My.Resources.openvpn)
        End If
        If IO.File.Exists(OpenVPNDir & "\openvpnserv.exe") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\openvpnserv.exe", My.Resources.openvpnserv)
        End If
        If IO.File.Exists(OpenVPNDir & "\openvpnserv2.exe") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\openvpnserv2.exe", My.Resources.openvpnserv2)
        End If
        If IO.File.Exists(OpenVPNDir & "\tap-windows.exe") = False Then
            IO.File.WriteAllBytes(OpenVPNDir & "\tap-windows.exe", My.Resources.tap_windows)
        End If
        If IO.File.Exists(ConfigDir & "\LeakMania-NL.ovpn") = False Then
            IO.File.WriteAllBytes(ConfigDir & "\LeakMania-NL.ovpn", My.Resources.LeakMania_NL)
        End If
        If IO.File.Exists(HTMLDir & "\down-arrow.png") = False Then
            My.Resources.down_arrow.Save(HTMLDir & "\down-arrow.png")
        End If
        If IO.File.Exists(HTMLDir & "\home.html") = False Then
            IO.File.WriteAllText(HTMLDir & "\home.html", My.Resources.home)
        Else
            IO.File.Delete(HTMLDir & "\home.html")
            IO.File.WriteAllText(HTMLDir & "\home.html", My.Resources.home)
        End If
    End Sub

End Class
