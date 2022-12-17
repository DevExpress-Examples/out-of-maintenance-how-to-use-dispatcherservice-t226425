Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports System
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Media

Namespace DXSample.ViewModel

    <POCOViewModel>
    Public Class MainViewModel

        Public Overridable Property Progress As Integer

        Public Overridable Property Color As Brush

        Public Overridable Property IsProgress As Boolean

        Protected ReadOnly Property DispatcherService As IDispatcherService
            Get
                Return GetService(Of IDispatcherService)()
            End Get
        End Property

        Public Sub Calculate()
            IsProgress = True
            Call Task.Factory.StartNew(AddressOf CalcCore).ContinueWith(Sub(x)
                DispatcherService.BeginInvoke(Sub() IsProgress = False)
            End Sub)
        End Sub

        Protected Sub OnIsProgressChanged()
            RaiseCanExecuteChanged(Sub(x) x.Calculate())
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
            Next
        End Sub

        Private Function GetColor(ByVal coef As Integer) As Color
            Return New Color() With {.R = CByte(coef * 15), .G = CByte(coef * 7), .B = CByte(coef * 8), .A = 255}
        End Function
    End Class
End Namespace
