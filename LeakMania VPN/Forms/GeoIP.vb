Imports System.Net

Public Class GeoIP

    Private SI As New SoftwareInfos

    Private title As String = "GeoIP | " + SI.getName + " - v" + SI.getVersion

    Private Sub GeoIP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = title
        WebBrowserHelper.FixBrowserVersion()
    End Sub

    Public Sub Locate()
        Dim wb As New WebBrowser()
        wb.Navigate("https://api.ipify.org/?format=html")
        'wb.Navigate("https://freegeoip.app/json")
        Do Until wb.ReadyState = WebBrowserReadyState.Complete
            Application.DoEvents()
        Loop
        Dim IP As String = GetBetween(wb.DocumentText, "<PRE>", "</PRE></BODY></HTML>")
        Clipboard.SetText(IP)
        Dim wc As New WebClient()
        Dim json As String = wc.DownloadString("https://freegeoip.app/json/" + IP)
        Dim latitude As String = GetBetween(json, ",""latitude"":", ",""longitude"":")
        Dim longitude As String = GetBetween(json, ",""longitude"":", ",""metro_code"":")
        Dim location As String = latitude + "+" + longitude
        GeoIPWebBrowser.DocumentText = _
            "<style>body {margin: 0;}</style>" + _
            "<iframe width=""100%"" height=""100%"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0"" src=""https://maps.google.com/maps?q=" + latitude + "," + longitude + "&output=embed""></iframe>"
        'GeoIPWebBrowser.Navigate("https://www.google.fr/maps/place/" + location)
        Me.Show()
    End Sub

    Public Function GetBetween(ByVal str As String, ByVal ex1 As String, ByVal ex2 As String) As String
        Dim istart As Integer = InStr(str, ex1)
        If istart > 0 Then
            Dim istop As Integer = InStr(istart, str, ex2)
            If istop > 0 Then
                Try
                    Dim value As String = str.Substring(istart + Len(ex1) - 1, istop - istart - Len(ex1))
                    Return value
                Catch ex As Exception
                    Return ""
                End Try
            End If
        End If
        Return ""
    End Function

End Class