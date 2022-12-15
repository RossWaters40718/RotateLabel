Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class RotateLabel
    Private NewLocation As Point
    Private NewFont As Font
    Private NewSize As Size
    Private cWidth As Integer
    Private cHeight As Integer
    Private StringSize As SizeF
    Public Enum TextOrientationChoices
        [HorizontalNormal] = 0 '0 Degrees
        [HorizontalUpSideDown] = 1 '180 Degrees
        [VerticalDownward] = 2 '90 Degrees
        [VerticalUpward] = 3 '-90 Degrees
    End Enum
    Public Text_Orientation As TextOrientationChoices = 0
    Public Angle As Double
    <Browsable(True), Category("Appearance"), Description("Sets The Text Orientation Of The Control."), DefaultValue(0)>
    Public Property TextOrientation As TextOrientationChoices
        Get
            Return Text_Orientation
        End Get
        Set(pbNewChoice As TextOrientationChoices)
            Select Case pbNewChoice
                Case = 0
                    Angle = 0
                Case = 1
                    Angle = 180
                Case = 2
                    Angle = 90
                Case = 3
                    Angle = -90
            End Select
            Text_Orientation = pbNewChoice
            Refresh()
        End Set
    End Property
    Public My_Text As String = "RotateLabel"
    <Browsable(True), Category("Appearance"), Description("The Text Associated With The Control."), DefaultValue("RotateLabel")>
    Public Overrides Property Text As String
        Get
            Return My_Text
        End Get
        Set(pbNewChoice As String)
            My_Text = pbNewChoice
            Refresh()
        End Set
    End Property
    Public My_Font As Font = New Font("Times New Roman", 18, FontStyle.Bold Or FontStyle.Italic)
    <Browsable(True), Category("Appearance"), Description("The Font Associated With The Control.")>
    Public Overrides Property Font As Font
        Get
            Dim x As Integer = Me.Location.X
            Dim y As Integer = Me.Location.Y
            NewLocation = Me.Location
            Return My_Font
        End Get
        Set(pbNewfont As Font)
            My_Font = pbNewfont
            MyBase.Font = pbNewfont
            NewFont = pbNewfont
            Dim x As Integer = Me.Location.X
            Dim y As Integer = Me.Location.Y
            NewLocation = Me.Location
            Refresh()
        End Set
    End Property
    Public My_BorderColor As Color = Color.Black
    <Browsable(True), Category("Appearance"), Description("Sets Control Border Color.")>
    Public Property BorderColor As Color
        Get
            Return My_BorderColor
        End Get
        Set(pbNewChoice As Color)
            My_BorderColor = pbNewChoice
            Refresh()
        End Set
    End Property
    Public My_BorderWidth As Single = 1
    <Browsable(True), Category("Appearance"), Description("Selects Control Border Width."), DefaultValue(1)>
    Public Property BorderWidth As Single
        Get
            Return My_BorderWidth
        End Get
        Set(pbNewChoice As Single)
            My_BorderWidth = pbNewChoice
            Refresh()
        End Set
    End Property
    Private Sub RotateLabel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ResizeRedraw = True
    End Sub
    Private Sub RotateLabel_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        If Me.Text = vbNullString Then Exit Sub
        Me.AutoSize = False
        Try
            StringSize = e.Graphics.MeasureString(Me.Text, NewFont)
            Select Case TextOrientation
                Case = 0, 1
                    'Resize Label For Chosen FontSize And Orientation
                    If BorderStyle <> BorderStyle.None Then
                        cHeight = StringSize.Height + BorderWidth + 3
                        cWidth = StringSize.Width + BorderWidth + 3
                    Else
                        cHeight = StringSize.Height + 3
                        cWidth = StringSize.Width + 3
                    End If
                Case = 2, 3
                    'Resize Label For Chosen FontSize And Orientation
                    If BorderStyle <> BorderStyle.None Then
                        cHeight = StringSize.Width + BorderWidth + 3
                        cWidth = StringSize.Height + BorderWidth + 3
                    Else
                        cHeight = StringSize.Width + 3
                        cWidth = StringSize.Height + 3
                    End If
            End Select
            NewLocation = New Point(Location.X, Location.Y)
            Me.Location = NewLocation
            NewSize = New Size(cWidth, cHeight)
            Me.Size = NewSize
            Me.Validate()
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality
            e.Graphics.TextContrast = 6
            Dim BorderRect As New RectangleF(0, 0, Me.ClientSize.Width, Me.ClientSize.Height)
            e.Graphics.SetClip(BorderRect)
            e.Graphics.Clip = New Region(BorderRect)
            If Me.BorderStyle = BorderStyle.FixedSingle Then
                'Draw Border Around Control Using RectF
                Dim BorderPen As New Pen(BorderColor, BorderWidth)
                e.Graphics.DrawRectangle(pen:=BorderPen, BorderRect.Left, BorderRect.Top, BorderRect.Width - 1, BorderRect.Height - 1)
                BorderPen.Dispose()
            End If
            e.Graphics.RotateTransform(Angle)
            'Point To Center Of Control To Draw
            e.Graphics.TranslateTransform(Me.ClientSize.Width / 2, Me.ClientSize.Height / 2, MatrixOrder.Append)
            'Draw The Text
            'Align Text To Center Of Control
            Dim String_format As New StringFormat(StringFormatFlags.NoClip) With {
                        .Alignment = StringAlignment.Center,
                        .LineAlignment = StringAlignment.Center
                    }
            Dim MyBrush = New SolidBrush(Me.ForeColor)
            e.Graphics.DrawString(Text, NewFont, MyBrush, 0, 0, String_format)
            String_format.Dispose()
            MyBrush.Dispose()
            e.Graphics.ResetTransform()
            e.Graphics.Transform.Dispose()
            e.Graphics.Dispose()
            Me.Location = NewLocation
            Me.Size = NewSize
        Catch exception As Exception
            MsgBox(exception.Message)
        End Try
    End Sub
    Private Sub RotateLabel_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Me.Location = NewLocation
    End Sub
    Private Sub RotateLabel_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        NewLocation = New Point(Location.X, Location.Y)
        NewSize = New Size(cWidth, cHeight)
        Me.Size = NewSize
        Me.Refresh()
    End Sub
End Class
