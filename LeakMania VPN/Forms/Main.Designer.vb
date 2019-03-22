<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.GUI = New System.Windows.Forms.WebBrowser()
        Me.OpenVPNConsole = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'GUI
        '
        Me.GUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GUI.Location = New System.Drawing.Point(0, 261)
        Me.GUI.MinimumSize = New System.Drawing.Size(20, 20)
        Me.GUI.Name = "GUI"
        Me.GUI.ScriptErrorsSuppressed = True
        Me.GUI.ScrollBarsEnabled = False
        Me.GUI.Size = New System.Drawing.Size(684, 100)
        Me.GUI.TabIndex = 0
        '
        'OpenVPNConsole
        '
        Me.OpenVPNConsole.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.OpenVPNConsole.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.OpenVPNConsole.Dock = System.Windows.Forms.DockStyle.Top
        Me.OpenVPNConsole.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.OpenVPNConsole.Location = New System.Drawing.Point(0, 0)
        Me.OpenVPNConsole.Name = "OpenVPNConsole"
        Me.OpenVPNConsole.ReadOnly = True
        Me.OpenVPNConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.OpenVPNConsole.Size = New System.Drawing.Size(684, 261)
        Me.OpenVPNConsole.TabIndex = 0
        Me.OpenVPNConsole.Text = ""
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 361)
        Me.Controls.Add(Me.GUI)
        Me.Controls.Add(Me.OpenVPNConsole)
        Me.Font = New System.Drawing.Font("Segoe UI Semilight", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(700, 400)
        Me.MinimumSize = New System.Drawing.Size(700, 400)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GUI As System.Windows.Forms.WebBrowser
    Friend WithEvents OpenVPNConsole As System.Windows.Forms.RichTextBox

End Class
