function Get-Voltage
{
[cmdletbinding()]
Param(
    [string]$RawISpacket =""
)

    if(-not $RawISpacket)
    {
        Write-Error "RawISPacket is null"
    }

    $hexArray = $RawISpacket -split " "

    $byteArray = New-Object System.Collections.ArrayList

    foreach($hex in $hexArray)
    {
        [byte]$byte = [System.Convert]::ToByte($hex,16)
        [void]$byteArray.Add($byte)
    }

    if([System.BitConverter]::IsLittleEndian)
    {
        Write-Verbose "Reversing array due to LittleEndian"
        [void]$byteArray.Reverse()
    }
    $AnalogRaw = [System.BitConverter]::ToInt16($byteArray,0)

    $volt = ($AnalogRaw/1023)*3.3 * 2

    $out = "" | Select-Object Voltage, RawValue
    $out.Voltage = $volt
    $out.RawValue = $AnalogRaw

    return $out
}