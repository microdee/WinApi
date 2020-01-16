using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using WinApi.DwmApi.Experimental;
using WinApi.Windows;

namespace WinApi.DxUtils.D3D11.Experimental
{
    public class DwmWindowTexture : IDisposable
    {
        private DeviceContext _ctx;
        private Adapter _adapter;
        private IntPtr _targetSharedHandle;
        public Texture2D Target { get; private set; }
        public Format DxgiFormat { get; private set; }

        public DwmWindowTexture(DeviceContext context, Adapter adapter)
        {
            _ctx = context;
            _adapter = adapter;
        }

        public void Update(NativeWindow win, bool updateWindow = true)
        {
            var fmt = (uint)Format.Unknown;
            IntPtr sharedHandle = IntPtr.Zero;
            ulong updateId = 0;
            var getres = DwmApiMethodsExperimental.DwmDxGetWindowSharedSurface(
                win.Handle, _adapter.Description.Luid, 0,
                ref fmt, ref sharedHandle, ref updateId
            );
            DxgiFormat = (Format)fmt;

            if(getres > 0)
                Debugger.Break();
            if(sharedHandle == IntPtr.Zero) return;

            if (updateWindow)
            {
                var rect = win.GetWindowRect();
                var updateRes = DwmApiMethodsExperimental.DwmDxUpdateWindowSharedSurface(
                    win.Handle, updateId,
                    ref rect
                );
            }

            if (Target == null)
            {
                Target = _ctx.Device.OpenSharedResource<Texture2D>(sharedHandle);
            }
            else if (_targetSharedHandle != sharedHandle)
            {
                Target.Dispose();
                Target = _ctx.Device.OpenSharedResource<Texture2D>(sharedHandle);
            }

            _targetSharedHandle = sharedHandle;
        }

        public void Dispose()
        {
            Target.Dispose();
        }
    }
}
