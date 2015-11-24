Imports System
Imports System.Windows.Forms

Public Class SystemInfo
    Dim power = SystemInformation.PowerStatus
    Private Function GetBatteryPercent()

        Dim percent As Single = power.BatteryLifePercent
        Return percent * 100
    End Function
    Private Function GetBatteryConnected()
        Dim Status As Boolean = power.powerlinestatus
        Return Status
    End Function

    Public ReadOnly Property BatteryPercent As String
        Get
            Return GetBatteryPercent()
        End Get
    End Property

    Public ReadOnly Property BatteryConnected As Boolean
        Get
            Return GetBatteryConnected()
        End Get
    End Property
End Class
