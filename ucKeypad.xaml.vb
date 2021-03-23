Public Class ucKeypad

    Public Property Value As Single
    Public Property OldValue As Single
    Public Property Min As Single
    Public Property Max As Single
    Public Property DefaultVal As Single
    Public Event OK(ByVal fReturnValue As Single)

    Public Event Cancel()

    Private Sub btnClear_Click(sender As Object, e As RoutedEventArgs) Handles btnClear.Click
        txtValue.Text = ""
    End Sub

    Private Sub btn7_Click(sender As Object, e As RoutedEventArgs) Handles btn7.Click
        txtValue.Text += "7"
    End Sub

    Private Sub btn0_Click(sender As Object, e As RoutedEventArgs) Handles btn0.Click
        txtValue.Text += "0"
    End Sub

    Private Sub btn1_Click(sender As Object, e As RoutedEventArgs) Handles btn1.Click
        txtValue.Text += "1"
    End Sub

    Private Sub btn2_Click(sender As Object, e As RoutedEventArgs) Handles btn2.Click
        txtValue.Text += "2"
    End Sub

    Private Sub btn3_Click(sender As Object, e As RoutedEventArgs) Handles btn3.Click
        txtValue.Text += "3"
    End Sub

    Private Sub btn4_Click(sender As Object, e As RoutedEventArgs) Handles btn4.Click
        txtValue.Text += "4"
    End Sub

    Private Sub btn5_Click(sender As Object, e As RoutedEventArgs) Handles btn5.Click
        txtValue.Text += "5"
    End Sub

    Private Sub btn6_Click(sender As Object, e As RoutedEventArgs) Handles btn6.Click
        txtValue.Text += "6"
    End Sub

    Private Sub btn8_Click(sender As Object, e As RoutedEventArgs) Handles btn8.Click
        txtValue.Text += "8"
    End Sub

    Private Sub btn9_Click(sender As Object, e As RoutedEventArgs) Handles btn9.Click
        txtValue.Text += "9"
    End Sub

    Private Sub btnPoint_Click(sender As Object, e As RoutedEventArgs) Handles btnPoint.Click
        txtValue.Text += "."
    End Sub

    Private Sub btnNegate_Click(sender As Object, e As RoutedEventArgs) Handles btnNegate.Click
        If Left(txtValue.Text, 1) = "-" Then
            txtValue.Text = Right(txtValue.Text, Len(txtValue.Text) - 1)

        Else
            txtValue.Text = "-" & txtValue.Text
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As RoutedEventArgs) Handles btnOK.Click
        RaiseEvent OK(Val(txtValue.Text))

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        RaiseEvent Cancel()
    End Sub

    Private Sub ucKeypad_IsVisibleChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.IsVisibleChanged
        txtValue.Text = ""
    End Sub


End Class
