Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Media

Namespace DXSample.ViewModel
    <POCOViewModel> _
    Public Class MainViewModel
        Private privateProgress As Integer
        Public Overridable Property Progress() As Integer
            Get
                Return privateProgress
            End Get
            Protected Set(ByVal value As Integer)
                privateProgress = value
            End Set
        End Property
        Private privateColor As Brush
        Public Overridable Property Color() As Brush
            Get
                Return privateColor
            End Get
            Protected Set(ByVal value As Brush)
                privateColor = value
            End Set
        End Property
        Private privateIsProgress As Boolean
        Public Overridable Property IsProgress() As Boolean
            Get
                Return privateIsProgress
            End Get
            Protected Set(ByVal value As Boolean)
                privateIsProgress = value
            End Set
        End Property
        Protected ReadOnly Property DispatcherService() As IDispatcherService
            Get
                Return Me.GetService(Of IDispatcherService)()
            End Get
        End Property
        Public Sub Calculate()
            IsProgress = True
            Task.Factory.StartNew(AddressOf CalcCore).ContinueWith(Sub(x) DispatcherService.BeginInvoke(Sub() IsProgress = False))
        End Sub
        Protected Sub OnIsProgressChanged()
            Me.RaiseCanExecuteChanged(Sub(x) x.Calculate())
        End Sub

        Public Function CanCalculate() As Boolean
            Return Not IsProgress
        End Function
        Private Sub CalcCore()
            For i As Integer = 1 To 100
                DispatcherService.BeginInvoke(Sub()
                    Progress = i
                    Color = New SolidColorBrush(GetColor(i))
                End Sub)
                Thread.Sleep(TimeSpan.FromSeconds(0.1))
            Next i
        End Sub
        Private Function GetColor(ByVal coef As Integer) As Color
            Return New Color() With {.R = CByte(coef * 15), .G = CByte(coef * 7), .B = CByte(coef * 8), .A = 255}
        End Function
    End Class
End Namespace
