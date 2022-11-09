#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.DeviceSimulation;
using UnityEngine;

namespace Kogane
{
    public static class DeviceSimulatorInternal
    {
        private static readonly Type      SIMULATOR_WINDOW_TYPE       = typeof( SimulatorWindow );
        private static readonly FieldInfo DEVICE_SIMULATOR_MAIN_FIELD = SIMULATOR_WINDOW_TYPE.GetField( "m_Main", BindingFlags.Instance | BindingFlags.NonPublic );

        private static SimulatorWindow GetSimulatorWindow()
        {
            return Resources
                    .FindObjectsOfTypeAll<SimulatorWindow>()
                    .FirstOrDefault()
                ;
        }

        private static DeviceSimulatorMain GetDeviceSimulatorMain()
        {
            var simulatorWindow     = GetSimulatorWindow();
            var deviceSimulatorMain = ( DeviceSimulatorMain )DEVICE_SIMULATOR_MAIN_FIELD.GetValue( simulatorWindow );

            return deviceSimulatorMain;
        }

        public static void SetDeviceIndexFromDeviceName( string deviceName )
        {
            var deviceSimulatorMain = GetDeviceSimulatorMain();
            var devices             = deviceSimulatorMain.devices;
            var deviceIndex         = Array.FindIndex( devices, x => x.deviceInfo.friendlyName == deviceName );

            if ( deviceIndex == -1 ) return;

            deviceSimulatorMain.deviceIndex = deviceIndex;
        }

        public static string GetCurrentDeviceName()
        {
            var deviceSimulatorMain = GetDeviceSimulatorMain();
            var currentDevice       = deviceSimulatorMain.currentDevice;
            var friendlyName        = currentDevice.deviceInfo.friendlyName;

            return friendlyName;
        }

        public static void Refresh()
        {
            var simulatorWindow          = GetSimulatorWindow();
            var editorAssembly           = typeof( Editor ).Assembly;
            var simulatorWindowType      = simulatorWindow.GetType();
            var gameViewType             = editorAssembly.GetType( "UnityEditor.GameView" );
            var playModeViewType         = editorAssembly.GetType( "UnityEditor.PlayModeView" );
            var swapMainWindowMethodInfo = playModeViewType.GetMethod( "SwapMainWindow", BindingFlags.Instance | BindingFlags.NonPublic );

            swapMainWindowMethodInfo.Invoke( simulatorWindow, new object[] { gameViewType } );

            var gameWindow = Resources
                    .FindObjectsOfTypeAll( gameViewType )
                    .FirstOrDefault()
                ;

            swapMainWindowMethodInfo.Invoke( gameWindow, new object[] { simulatorWindowType } );
        }
    }
}

#endif