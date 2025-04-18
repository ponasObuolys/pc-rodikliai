using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace PC_Rodikliai.Services
{
    public class HotkeyService
    {
        private const int WM_HOTKEY = 0x0312;
        private readonly Dictionary<int, Action> _hotkeyActions = new();
        private int _currentId = 0;
        private IntPtr _windowHandle;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public void Initialize(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
            ComponentDispatcher.ThreadPreprocessMessage += OnThreadPreprocessMessage;
        }

        public void Cleanup()
        {
            foreach (var id in _hotkeyActions.Keys)
            {
                UnregisterHotKey(_windowHandle, id);
            }
            _hotkeyActions.Clear();
            ComponentDispatcher.ThreadPreprocessMessage -= OnThreadPreprocessMessage;
        }

        public bool RegisterHotkey(ModifierKeys modifiers, Key key, Action action)
        {
            var id = ++_currentId;
            var vk = KeyInterop.VirtualKeyFromKey(key);
            var fsModifiers = (uint)modifiers;

            if (RegisterHotKey(_windowHandle, id, fsModifiers, (uint)vk))
            {
                _hotkeyActions[id] = action;
                return true;
            }

            return false;
        }

        private void OnThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY && _hotkeyActions.TryGetValue((int)msg.wParam, out var action))
            {
                action?.Invoke();
                handled = true;
            }
        }
    }
} 