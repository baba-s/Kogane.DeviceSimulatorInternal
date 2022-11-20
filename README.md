# Kogane Device Simulator Internal

Device Simulator の internal な機能にアクセスできるパッケージ

## 使用例

```cs
using Kogane;
using UnityEditor;
using UnityEngine;

public static class Example
{
    [MenuItem( "Tools/Hoge" )]
    private static void Hoge()
    {
        // Device Simulator で選択されているデバイスの名前を取得します
        Debug.Log( DeviceSimulatorInternal.GetCurrentDeviceName() );

        // Device Simulator のデバイスを設定します
        DeviceSimulatorInternal.SetDeviceIndexFromDeviceName( "HTC 10" );

        // Device Simulator の表示をリフレッシュします
        DeviceSimulatorInternal.Refresh();
    }
}
```