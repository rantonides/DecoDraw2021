Imports Microsoft.Win32
Imports System.Windows.Ink
Imports System.IO
'Imports AdvancedHMIDrivers
Imports Ace.Adept.Server.Controls
Imports Ace.Adept.Server.Desktop.Connection
Imports Ace.Core.Server.Transform3D
Imports Ace.Core.Util
Imports System.Collections.ObjectModel
Imports System.Text
Imports System.Windows.Media.Animation
Imports System.Timers
Imports System.Windows
Imports System.Windows.Input
Imports System.Math
Imports System.Reflection
Imports System.Diagnostics


'Imports ClipperLib
'Imports Polygon = System.Collections.Generic.List(Of ClipperLib.IntPoint)
'Imports Polygons = System.Collections.Generic.List(Of System.Collections.Generic.List(Of ClipperLib.IntPoint))
Imports System.Windows.Controls
Class MainWindow
    Private bInvert As Boolean = True ' False for Bosco and Roxys and SweetStreet.   True for Corsos Cookies.
    Private TransformTimer As New System.Windows.Threading.DispatcherTimer()
    Private CCWDispatchTimer As New System.Windows.Threading.DispatcherTimer()
    Private CWDispatchTimer As New System.Windows.Threading.DispatcherTimer()
    'Private visionMonitorTimer As New System.Windows.Threading.DispatcherTimer()
    'Private myDispatcherTimer As New System.Windows.Threading.DispatcherTimer()
    Private backupDispatchTimer As New System.Windows.Threading.DispatcherTimer()
    Private autoDownloadTimer As New System.Windows.Threading.DispatcherTimer()
    Private Success As Single
    Private Failure As Single
    Private colRed As Color = Color.FromRgb(255, 0, 0)
    Private colGreen As Color = Color.FromRgb(0, 255, 0)
    Private colBlack As Color = Color.FromRgb(0, 0, 0)
    Public Eye2RobotCenter As Single = 12.75 * 25.4
    Dim cycleData As String
    Const ScaleFactor = 1.02
    Const MoveFactor = 1
    Public Const ctrlCLX = 2
    Public Const ctrlKuka = 3
    Public Const ctrlAdept = 4
    Public Const ctrlUR = 5
    Private iController As Integer = ctrlUR
    Public P0X(3000) As Single
    Public P0Y(3000) As Single
    Public P0Z(3000) As Single
    Public bDebug As Boolean
    Dim numPoints As Integer = 1200 ' Changed Oct 31, 2018 for Bosco  Corsos is 1200  'Was 800 RMA
    Dim heightGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 0)
    Dim StopDelayGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 1)
    Dim StartDelayGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 2)
    Dim BeltSpeedGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 3)
    Dim PatternSpeedGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 4)
    Dim NozzleGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 5)
    Dim PrechargeGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 6)
    Dim MoveDelayGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 7)
    Dim cakeWidthGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 8)
    Dim cakeLengthGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 9)
    Dim cakeShapeGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 10)
    Dim ContinuousGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 11)
    Dim upperPressureGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 12)
    Dim DipGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 13)
    Dim NotesBoxGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 14)
    Dim UnitsGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 15)


    Dim cakeXoffLGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 16)
    Dim cakeYoffLGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 17)
    Dim cakeXoffRGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 18)
    Dim cakeYoffRGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 19)

    Dim SpecialGuid As New Guid(10, 11, 12, 10, 0, 0, 0, 0, 0, 0, 20)

    Dim fScaleFactor As Single = 0.5
    Dim numRecipes As Integer = 100
    Dim BackupState As Integer = 0
    Public Const MinSafeHeight = 0.5
    Public Const SpeedScaleFactor As Single = 0.18 'this makes 100% = 18% (used mostly to slow down the moves sent to the UR)

    Dim myToolTip As New ToolTip
    'ra Dim WithEvents frmText As New frmText
    Dim undoLevel As Integer = 0
    Dim undoAvailable As Integer = 0
    Dim sCurrentFileName As String = ""

    'ra Private WithEvents inset_keypad As New frmKeypad
    Dim inset_keypad_customSpacing As Single = 5.0

    'ra Private EthernetIPforCLXComm1 As New EthernetIPforCLXCom()

    Dim strokeErase_Occured As Boolean = False

    Dim da As New DoubleAnimation

    Private Sub cvsGrid_Loaded(sender As Object, e As RoutedEventArgs) Handles cvsGrid.Loaded
        drawGrid()
    End Sub

    Private Sub drawGrid()

        Dim cellsize As Double = 25D
        Dim x1 As Double
        Dim iCounter As Integer
        Try
            For iCounter = InkCanvas.Strokes.Count - 1 To 0 Step -1
                If InkCanvas.Strokes(iCounter).DrawingAttributes.Color = colRed Then
                    InkCanvas.Strokes.RemoveAt(iCounter)
                Else
                    If InkCanvas.Strokes(iCounter).DrawingAttributes.Color = colGreen Then
                        InkCanvas.Strokes.RemoveAt(iCounter)
                    End If
                End If
            Next

        Catch
            MsgBox(Err.Description)
        Finally
        End Try
        If Right(lblXY.Content, 1) = "m" Then   'Units are in mm
            cellsize = 25.0
        Else
            cellsize = 25.4
        End If

        For iCounter = cvsGrid.Children.Count - 1 To 0 Step -1
            If (TypeOf (cvsGrid.Children(iCounter)) Is Line) Or (TypeOf (cvsGrid.Children(iCounter)) Is Ellipse) Or (TypeOf (cvsGrid.Children(iCounter)) Is TextBlock) Then
                cvsGrid.Children.RemoveAt(iCounter)
            End If
        Next

        For x1 = cellsize To InkCanvas.ActualWidth Step cellsize
            Dim line As New Line()
            line.Stroke = New SolidColorBrush(Color.FromArgb(64, 0, 0, 255))
            line.IsHitTestVisible = False
            line.X1 = x1
            line.X2 = x1
            line.Y1 = 0.0
            line.Y2 = InkCanvas.ActualHeight
            cvsGrid.Children.Add(line)
        Next x1



        Dim y1 As Double
        For y1 = cellsize To InkCanvas.ActualHeight Step cellsize
            Dim line As New Line()
            line.Stroke = New SolidColorBrush(Color.FromArgb(64, 0, 0, 255))
            line.IsHitTestVisible = False
            line.X1 = 0.0
            line.X2 = InkCanvas.ActualWidth
            line.Y1 = y1
            line.Y2 = y1
            cvsGrid.Children.Add(line)
        Next y1


        'Draw Cake
        Dim fScaleFactor As Single
        If Right(lblXY.Content, 1) = "m" Then
            fScaleFactor = 1.0
        Else
            fScaleFactor = 25.4
        End If

        If butCakeShape.Tag = "Rectangle" Then
            Dim Line1 As New Line()
            Line1.StrokeThickness = 3
            Line1.Stroke = New SolidColorBrush(Color.FromArgb(64, 45, 45, 0))
            Line1.IsHitTestVisible = False
            Line1.X1 = 25
            Line1.Y1 = 25
            Line1.X2 = paramCakeLength.Value * fScaleFactor + 25
            Line1.Y2 = 25
            cvsGrid.Children.Add(Line1)

            Dim Line2 As New Line()
            Line2.StrokeThickness = 3
            Line2.Stroke = New SolidColorBrush(Color.FromArgb(64, 45, 45, 0))
            Line2.IsHitTestVisible = False
            Line2.X1 = paramCakeLength.Value * fScaleFactor + 25
            Line2.Y1 = 25
            Line2.X2 = paramCakeLength.Value * fScaleFactor + 25
            Line2.Y2 = paramCakeWidth.Value * fScaleFactor + 25
            cvsGrid.Children.Add(Line2)

            Dim Line3 As New Line()
            Line3.StrokeThickness = 3
            Line3.Stroke = New SolidColorBrush(Color.FromArgb(64, 45, 45, 0))
            Line3.IsHitTestVisible = False
            Line3.X1 = paramCakeLength.Value * fScaleFactor + 25
            Line3.Y1 = paramCakeWidth.Value * fScaleFactor + 25
            Line3.X2 = 25
            Line3.Y2 = paramCakeWidth.Value * fScaleFactor + 25
            cvsGrid.Children.Add(Line3)

            Dim Line4 As New Line()
            Line4.StrokeThickness = 3
            Line4.Stroke = New SolidColorBrush(Color.FromArgb(64, 45, 45, 0))
            Line4.IsHitTestVisible = False
            Line4.X1 = 25
            Line4.Y1 = paramCakeWidth.Value * fScaleFactor + 25
            Line4.X2 = 25
            Line4.Y2 = 25
            cvsGrid.Children.Add(Line4)
        Else
            Dim Ellipse As New Ellipse()
            Ellipse.StrokeThickness = 3
            Ellipse.Stroke = New SolidColorBrush(Color.FromArgb(64, 45, 45, 0))
            Ellipse.IsHitTestVisible = False
            Ellipse.Width = paramCakeLength.Value * fScaleFactor
            Ellipse.Height = paramCakeWidth.Value * fScaleFactor
            Ellipse.Margin = New Thickness(25, 25, 0, 0)


            cvsGrid.Children.Add(Ellipse)
        End If
        'Add Marks at the halfway point on each axis
        Dim linecenx, lineceny As New Line()
        linecenx.Stroke = New SolidColorBrush(Color.FromArgb(64, 0, 0, 255))
        linecenx.IsHitTestVisible = False
        linecenx.X1 = 10
        linecenx.X2 = 40
        linecenx.Y1 = paramCakeWidth.Value / 2 * fScaleFactor + 25
        linecenx.Y2 = linecenx.Y1
        cvsGrid.Children.Add(linecenx)

        lineceny.Stroke = New SolidColorBrush(Color.FromArgb(64, 0, 0, 255))
        lineceny.IsHitTestVisible = False
        lineceny.X1 = paramCakeLength.Value / 2 * fScaleFactor + 25
        lineceny.X2 = lineceny.X1
        lineceny.Y1 = 10
        lineceny.Y2 = 40
        cvsGrid.Children.Add(lineceny)

        ' Convert the X, Y position to Window based pixel coordinates
        If InkCanvas.EditingMode = InkCanvasEditingMode.Select Then
            'pt = New Point
            For iCounter = 0 To InkCanvas.Strokes.Count - 1
                Dim Ellipse As New Ellipse()
                Ellipse.StrokeThickness = 0.5
                Ellipse.Stroke = New SolidColorBrush(Color.FromArgb(255, 255, 0, 0))
                Ellipse.Width = 4
                Ellipse.Height = 4
                Ellipse.Margin = New Thickness(InkCanvas.Strokes(iCounter).StylusPoints(0).X - 2, InkCanvas.Strokes(iCounter).StylusPoints(0).Y - 2, 0, 0)
                Ellipse.IsHitTestVisible = False
                cvsGrid.Children.Add(Ellipse)
                Dim TextBlock As New TextBlock
                TextBlock.FontFamily = New FontFamily("Segoe UI")
                TextBlock.Foreground = New SolidColorBrush(Color.FromArgb(255, 255, 0, 0))
                TextBlock.Text = Trim(Str(iCounter + 1))
                TextBlock.FontSize = 7
                TextBlock.Margin = New Thickness(InkCanvas.Strokes(iCounter).StylusPoints(0).X + 1.5, InkCanvas.Strokes(iCounter).StylusPoints(0).Y - 2.5, 0, 0)
                TextBlock.IsHitTestVisible = False
                cvsGrid.Children.Add(TextBlock)
                'Dim drawFont As New Font("Arial", 16)
                'Dim drawBrush As New SolidBrush(Color.Black)

                'g.DrawString(Trim(Str(iCounter + 1)), drawFont, drawBrush, pt.X, pt.Y)
            Next iCounter
        End If

        ' strokeSlider.Maximum = InkCanvas.Strokes.Count

        Select Case InkCanvas.GetSelectedStrokes.Count
            Case Is > 1
                butJoin.Visibility = Windows.Visibility.Visible
                butSmooth.Visibility = Windows.Visibility.Visible
                butReverse.Visibility = Windows.Visibility.Visible
                butOrderMinus.Visibility = Windows.Visibility.Collapsed
                butOrderPlus.Visibility = Windows.Visibility.Collapsed
                butRotateCCW.Visibility = Windows.Visibility.Visible
                butRotateCW.Visibility = Windows.Visibility.Visible
                butDuplicate.Visibility = Windows.Visibility.Collapsed
                butRandomLoop.Visibility = Windows.Visibility.Collapsed
                butFlip.Visibility = Windows.Visibility.Visible
                tbSpecial.Visibility = Visibility.Collapsed
            Case 1
                butJoin.Visibility = Windows.Visibility.Collapsed
                butSmooth.Visibility = Windows.Visibility.Visible
                butReverse.Visibility = Windows.Visibility.Visible
                butOrderMinus.Visibility = Windows.Visibility.Visible
                butOrderPlus.Visibility = Windows.Visibility.Visible
                butRotateCCW.Visibility = Windows.Visibility.Visible
                butRotateCW.Visibility = Windows.Visibility.Visible
                butDuplicate.Visibility = Windows.Visibility.Visible
                butRandomLoop.Visibility = Windows.Visibility.Visible
                butFlip.Visibility = Windows.Visibility.Visible
                tbSpecial.Visibility = Visibility.Visible
            Case 0
                butJoin.Visibility = Windows.Visibility.Collapsed
                butSmooth.Visibility = Windows.Visibility.Collapsed
                butReverse.Visibility = Windows.Visibility.Collapsed
                butOrderMinus.Visibility = Windows.Visibility.Collapsed
                butOrderPlus.Visibility = Windows.Visibility.Collapsed
                butRotateCCW.Visibility = Windows.Visibility.Visible
                butRotateCW.Visibility = Windows.Visibility.Visible
                butDuplicate.Visibility = Windows.Visibility.Collapsed
                butRandomLoop.Visibility = Windows.Visibility.Collapsed
                butFlip.Visibility = Windows.Visibility.Collapsed
                tbSpecial.Visibility = Visibility.Collapsed
        End Select


    End Sub
End Class
