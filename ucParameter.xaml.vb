Public Class ucParameter
    Public Event ValueChanged()
    Public Event SetDefault()

    'Private WithEvents Keypad As New frmKeypad

    Private sImage As String
    Private sHelp As String
    Public sDefaultValue As String

    Public Property Image() As String
        Get
            Return sImage
        End Get
        Set(ByVal value As String)
            sImage = value
            imgParam.Source = New BitmapImage(New Uri(sImage, UriKind.Relative))
        End Set
    End Property


    Public Property Units() As String
        Get
            Return lblUnits.Content
        End Get
        Set(ByVal value As String)
            lblUnits.Content = value
        End Set
    End Property


    Public Property Help() As String
        Get
            Return sHelp
        End Get
        Set(ByVal value As String)
            sHelp = value
        End Set
    End Property


    Private snumFormat As String
    Public Property numFormat() As String
        Get
            Return snumFormat
        End Get
        Set(ByVal value As String)
            snumFormat = value
        End Set
    End Property

    Public Property Min() As Single
        Get
            Return sldValue.Minimum
        End Get
        Set(ByVal value As Single)
            sldValue.Minimum = value
        End Set
    End Property

    Public Property Max() As Single
        Get
            Return sldValue.Maximum
        End Get
        Set(ByVal value As Single)
            sldValue.Maximum = value
        End Set
    End Property

    Public Property IsDefault() As Boolean
        Get
            Return chkDefault.IsChecked
        End Get
        Set(ByVal value As Boolean)
            chkDefault.IsChecked = value
            If value Then 'If value is checked now, set to default value
                lblValue.Content = sDefaultValue
            End If
        End Set
    End Property


    Public Property Value() As Single
        Get
            Return Val(lblValue.Content)
        End Get
        Set(ByVal value As Single)
            lblValue.Content = Format(value, snumFormat)
            sldValue.Value = value
            If chkDefault.IsChecked Or HasDefault = False Then
                sDefaultValue = lblValue.Content
            End If
            RaiseEvent ValueChanged()

        End Set
    End Property


    Public Property Description() As String
        Get
            Return lblParameterName.Content
        End Get
        Set(ByVal value As String)
            lblParameterName.Content = value
        End Set
    End Property


    Public Property TickFrequency() As Single
        Get
            Return sldValue.TickFrequency
        End Get
        Set(ByVal value As Single)
            sldValue.TickFrequency = value
        End Set
    End Property



    Private Sub sldValue_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles sldValue.ValueChanged
        Value = sldValue.Value
    End Sub

    Private bHasDefault As Boolean
    Public Property HasDefault() As Boolean
        Get
            Return bHasDefault
        End Get
        Set(ByVal value As Boolean)
            bHasDefault = value
            If bHasDefault Then
                chkDefault.Visibility = Windows.Visibility.Visible
                sldValue.Width = 170
            Else
                chkDefault.Visibility = Windows.Visibility.Hidden
                sldValue.Width = 200
            End If
            lblDefault.Visibility = chkDefault.Visibility
        End Set
    End Property

    Private Sub ucParameter_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bHasDefault Then
            chkDefault.Visibility = Windows.Visibility.Visible
            sldValue.Width = 170
        Else
            chkDefault.Visibility = Windows.Visibility.Hidden
            sldValue.Width = 200
        End If
        lblDefault.Visibility = chkDefault.Visibility
    End Sub

    Private Sub ucParameter_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseUp

        'Keypad.Min = Min
        'Keypad.Max = Max
        'Keypad.OldValue = Value
        'Keypad.Decription = Description
        'Keypad.Help = sHelp

        'Keypad.ShowDialog()


    End Sub

    'Private Sub Keypad_OK(fReturnValue As Single) Handles Keypad.OK
    '    Value = fReturnValue
    'End Sub

    Private Sub chkDefault_Click(sender As Object, e As RoutedEventArgs) Handles chkDefault.Click
        If chkDefault.IsChecked Or HasDefault = False Then
            Value = sDefaultValue
            RaiseEvent SetDefault()
        Else
            'If Default Unchecked, Raise an event to set stroke properties
            RaiseEvent ValueChanged()
        End If
    End Sub
End Class
