using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NetCoreEx.Geometry;
using WinApi.Core;

namespace WinApi.DwmApi.Experimental
{
    public static class DwmApiMethodsExperimental
    {
        public const string LibraryName = "dwmapi";

        /// <summary>
        /// Retrieves the DirectX shared surface backing a given window.
        /// This surface can be written to in order to update the contents of the window.
        /// </summary>
        /// <param name="hwnd">An HWND specifying the window to be updated.</param>
        /// <param name="luidAdapter">The LUID of the adapter where the surface should be located.</param>
        /// <param name="hmonitorAssociation">Reserved (presumably keep it 0)</param>
        /// <param name="dwFlags">
        /// <para>
        /// This parameter can be one of the following values,
        /// or a bitwise OR combination of multiple values where appropriate.</para>
        /// <para>DWM_REDIRECTION_FLAG_WAIT (0) Causes the function to block until a vertical synchronization
        /// (VSync) has passed since the last time the function was called successfully.</para>
        /// <para>DWM_REDIRECTION_FLAG_SUPPORT_PRESENT_TO_GDI_SURFACE (0x10) Indicates that the calling
        /// application is capable of presenting to a GDI shared surface.
        /// </para>
        /// </param>
        /// <param name="pfmtWindow">
        /// <para>(ref uint) On input, the desired format of the surface. On output, the actual format of the returned surface.</para>
        /// <para>Originally DXGI_FORMAT, when used with SharpDX use SharpDX.DXGI.Format</para>
        /// </param>
        /// <param name="phDxSurface">(out IntPtr) On output, the shared handle to the surface.</param>
        /// <param name="puiUpdateId">(out long) On output, the ID of the update.</param>
        /// <returns>
        /// <para>
        /// It's not managed WinApi <see cref="HResult"/> as at time of writing some
        /// exact return values couldn't be determined.
        /// </para>
        /// <para>This function can return one of these values.</para>
        /// <para>
        /// S_OK: The call succeeded, and you should update the surface,
        /// being sure to pass the update ID to D3DKMTRender (in the PresentHistoryToken
        /// member of the D3DKMT_RENDER structure when the update is submitted, and then
        /// DwmDxUpdateWindowSharedSurface should be called with the same update ID.
        /// Note that DwmDxUpdateWindowSharedSurface should be called regardless of
        /// whether the surface was actually updated or not.
        /// </para>
        /// <para>
        /// DWM_S_GDI_REDIRECTION_SURFACE: The call succeeded, and you should
        /// update the surface by calling D3DKMTPresent, and setting
        /// PresentHistoryToken member's Model to D3DKMT_PM_REDIRECTED_BLT, and
        /// providing the update ID in the Blt member of the union. This value is
        /// only returned if DWM_REDIRECTION_FLAG_SUPPORT_PRESENT_TO_GDI_SURFACE was
        /// specified in dwFlags.
        /// </para>
        /// <para>DWM_E_ADAPTER_NOT_FOUND: The value of luidAdapter is not valid.</para>
        /// <para>DWM_E_COMPOSITIONDISABLED: DWM is not currently enabled.</para>
        /// </returns>
        /// <remarks>
        /// This API is intended for implementing a graphics driver or runtime.
        /// An application may not call this method. This documentation is only valid
        /// for Windows 7, and this API is not guaranteed to exist nor behave in a
        /// similar manner on other versions of Windows. This function is not present in
        /// any header or static-link library, and it is located at ordinal 100 in dwmapi.dll.
        /// </remarks>
        public static unsafe uint DwmDxGetWindowSharedSurface(
            IntPtr hwnd,
            long luidAdapter,
            //IntPtr hmonitorAssociation, Reserved
            ulong dwFlags,
            ref uint fmtWindow,
            ref IntPtr hDxSurface,
            ref ulong uiUpdateId
        ) {
            fixed (uint* pfmtWindow = &fmtWindow)
            fixed (IntPtr* phDxSurface = &hDxSurface)
            fixed (ulong* puiUpdateId = &uiUpdateId)
            {
                return DwmDxGetWindowSharedSurface(
                    hwnd, luidAdapter, IntPtr.Zero, dwFlags,
                    new IntPtr(pfmtWindow),
                    new IntPtr(phDxSurface),
                    new IntPtr(puiUpdateId)
                );
            }
        }

        [DllImport(LibraryName, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint DwmDxGetWindowSharedSurface(
            IntPtr hwnd,
            long luidAdapter,
            IntPtr hmonitorAssociation,
            ulong dwFlags,
            IntPtr pfmtWindow,
            IntPtr phDxSurface,
            IntPtr puiUpdateId
        );

        /// <summary>
        /// Notifies the DWM of an incoming update to a window shared surface.
        /// </summary>
        /// <param name="hwnd">An HWND specifying the window to be updated.</param>
        /// <param name="puiUpdateId">The update ID retrieved from <see cref="DwmDxGetWindowSharedSurface"/>.</param>
        /// <param name="dwFlags">Reserved (presumably keep it 0)</param>
        /// <param name="hmonitorAssociation">Reserved (presumably keep it 0)</param>
        /// <param name="prc">
        /// (out Rectangle) The rect of the window being updated, in window coordinate space.
        /// A NULL rectangle indicates that no region was updated.
        /// </param>
        /// <returns>S_OK if successful, otherwise a FAILED HRESULT.</returns>
        /// <remarks>
        /// <para>
        /// This API is intended for implementing a graphics driver or runtime.
        /// An application may not call this method. This documentation is only valid
        /// for Windows 7, and this API is not guaranteed to exist nor behave in a
        /// similar manner on other versions of Windows. This function is not present in
        /// any header or static-link library, and it is located at ordinal 101 in dwmapi.dll.
        /// </para>
        /// <para>
        /// This method should only ever be called after DwmDxGetWindowSharedSurface returns S_OK,
        /// and must be called in that scenario, regardless of whether the surface is updated or not.
        /// </para>
        /// </remarks>
        public static unsafe HResult DwmDxUpdateWindowSharedSurface(
            IntPtr hwnd,
            ulong puiUpdateId,
            //ulong dwFlags, Reserved
            //IntPtr hmonitorAssociation, Reserved
            ref Rectangle rc
        ) {
            fixed (Rectangle* prc = &rc)
            {
                return DwmDxUpdateWindowSharedSurface(
                    hwnd, puiUpdateId,
                    0, IntPtr.Zero,
                    new IntPtr(prc)
                );
            }
        }

        [DllImport(LibraryName, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern HResult DwmDxUpdateWindowSharedSurface(
            IntPtr hwnd,
            ulong puiUpdateId,
            ulong dwFlags,
            IntPtr hmonitorAssociation,
            IntPtr prc
        );
    }
}
