using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TalesWeaver_Cursor_Assistance
{
    public partial class Form1
    {
        #region [1. 편의 기능(키 치환) 필드 및 상태 변수]
        /// <summary>키 치환 기능 활성화 여부</summary>
        private bool _spaceToEnterEnabled = true;
        /// <summary>숫자 키(1~5) 입력 후 Space를 Enter로 변환할 유지 시간(ms, 최대 2000)</summary>
        private int _spaceToEnterDurationMs = 200;
        /// <summary>치환 모드가 유지되는 만료 시각(64비트 단조 증가 Tick)</summary>
        private long _spaceAsEnterUntilTick = 0;
        /// <summary>치환 만료 시각 접근 동기화를 위한 잠금 객체</summary>
        private readonly object _spaceEnterLock = new object();
        /// <summary>GC 수거 방지를 위해 유지하는 키보드 후크 콜백 인스턴스</summary>
        private LowLevelKeyboardProc _keyboardProc;
        /// <summary>등록된 저수준 키보드 후크 핸들</summary>
        private IntPtr _keyboardHookId = IntPtr.Zero;
        #endregion

        #region [2. 편의 기능 모듈 초기화 및 설정 동기화]
        /// <summary>
        /// 편의 기능 모듈의 기본값을 초기화합니다.
        /// </summary>
        private void InitializeConvenienceModule()
        {
            this._spaceToEnterEnabled = true;
            this._spaceToEnterDurationMs = 200;
            this._spaceAsEnterUntilTick = 0;
        }

        /// <summary>
        /// Form1_Load 시 편의 기능 관련 UI 컨트롤을 초기화합니다.
        /// </summary>
        private void InitConvenienceUiOnLoad()
        {
            if (this.checkBox_SpaceToEnter_Enable != null)
            {
                this.checkBox_SpaceToEnter_Enable.Checked = this._spaceToEnterEnabled;
            }
            if (this.textBox_SpaceToEnter_Duration != null)
            {
                this.textBox_SpaceToEnter_Duration.Text = this._spaceToEnterDurationMs.ToString();
                this.textBox_SpaceToEnter_Duration.Enabled = this._spaceToEnterEnabled;
            }
        }

        /// <summary>
        /// UI에서 키 치환 유지 시간 및 활성화 여부를 검증하고 적용합니다.
        /// </summary>
        private void ApplyConvenienceSettingsFromUi()
        {
            int durationMs = 200;
            if (this.textBox_SpaceToEnter_Duration != null)
            {
                if (!int.TryParse(this.textBox_SpaceToEnter_Duration.Text, out durationMs) || durationMs < 0 || durationMs > MAX_SPACE_TO_ENTER_DURATION_MS)
                {
                    MessageBox.Show($"유지 시간은 0 ~ {MAX_SPACE_TO_ENTER_DURATION_MS}(ms) 사이의 숫자만 입력 가능합니다.");
                    durationMs = 200;
                    this.textBox_SpaceToEnter_Duration.Text = durationMs.ToString();
                }
            }
            this._spaceToEnterDurationMs = durationMs;

            if (this.checkBox_SpaceToEnter_Enable != null)
            {
                this._spaceToEnterEnabled = this.checkBox_SpaceToEnter_Enable.Checked;
            }
        }

        /// <summary>
        /// INI 키-값이 편의 기능 설정 항목이면 파싱하여 반영합니다.
        /// </summary>
        private bool TryLoadConvenienceSetting(string key, string value)
        {
            if (key == "SpaceToEnter_Enable" && bool.TryParse(value, out bool parsedSpaceToEnterEnable))
            {
                this._spaceToEnterEnabled = parsedSpaceToEnterEnable;
                return true;
            }
            if (key == "SpaceToEnter_DurationMs" && int.TryParse(value, out int parsedDuration) && parsedDuration >= 0)
            {
                this._spaceToEnterDurationMs = Math.Min(MAX_SPACE_TO_ENTER_DURATION_MS, parsedDuration);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 편의 기능 설정값의 유효 범위를 보정합니다.
        /// </summary>
        private void NormalizeConvenienceSettings()
        {
            if (this._spaceToEnterDurationMs < 0) this._spaceToEnterDurationMs = 200;
            if (this._spaceToEnterDurationMs > MAX_SPACE_TO_ENTER_DURATION_MS) this._spaceToEnterDurationMs = MAX_SPACE_TO_ENTER_DURATION_MS;
        }

        /// <summary>
        /// Space → Enter 치환 만료 타이머를 초기화합니다.
        /// </summary>
        private void ResetSpaceToEnterTimer()
        {
            lock (this._spaceEnterLock)
            {
                this._spaceAsEnterUntilTick = 0;
            }
        }
        #endregion

        #region [3. 편의 기능 UI 이벤트 핸들러]
        /// <summary>
        /// 'Space → Enter 치환 (1~5 키) 기능 사용' 체크박스 상태 변경 시 활성화 여부와 입력창 상태 및 타이머를 동기화합니다.
        /// </summary>
        private void checkBox_SpaceToEnter_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_SpaceToEnter_Enable == null) return;

            this._spaceToEnterEnabled = this.checkBox_SpaceToEnter_Enable.Checked;

            if (this.textBox_SpaceToEnter_Duration != null)
            {
                this.textBox_SpaceToEnter_Duration.Enabled = this._spaceToEnterEnabled;
            }

            if (!this._spaceToEnterEnabled)
            {
                this.ResetSpaceToEnterTimer();
            }
        }
        #endregion

        #region [4. 저수준 키보드 후크 (1~5 입력 후 Space -> Enter 치환)]
        /// <summary>
        /// 32비트 Environment.TickCount의 오버플로/부호 반전을 보정한 64비트 단조 증가 밀리초 값을 반환합니다.
        /// </summary>
        private static long GetSafeTickCount64()
        {
            return (long)(uint)Environment.TickCount;
        }

        /// <summary>
        /// 전역 저수준 키보드 후크(WH_KEYBOARD_LL)를 설치합니다.
        /// </summary>
        private void StartSpaceToEnterHook()
        {
            if (this._keyboardHookId != IntPtr.Zero) return;

            this._keyboardProc = KeyboardHookCallback;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                IntPtr hMod = curModule != null ? GetModuleHandle(curModule.ModuleName) : IntPtr.Zero;
                this._keyboardHookId = SetWindowsHookEx(WH_KEYBOARD_LL, this._keyboardProc, hMod, 0);
            }
        }

        /// <summary>
        /// 등록된 전역 저수준 키보드 후크를 해제합니다.
        /// </summary>
        private void StopSpaceToEnterHook()
        {
            if (this._keyboardHookId == IntPtr.Zero) return;

            UnhookWindowsHookEx(this._keyboardHookId);
            this._keyboardHookId = IntPtr.Zero;
            this._keyboardProc = null;
        }

        /// <summary>
        /// 입력된 가상 키 코드가 상단 숫자키(1~5) 또는 넘패드(1~5)인지 판별합니다.
        /// </summary>
        private static bool IsTriggerNumberKey(int vkCode)
        {
            return (vkCode >= 0x31 && vkCode <= 0x35) || (vkCode >= 0x61 && vkCode <= 0x65);
        }

        /// <summary>
        /// 저수준 키보드 후크 콜백: 기능 활성 상태에서 1~5 키 입력 후 지정 시간(최대 2000ms) 이내에 눌린 Space를 Enter로 치환합니다.
        /// </summary>
        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                if (msg == WM_KEYDOWN || msg == WM_SYSKEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (this.TryToggleSkillOverlayByHotkey((Keys)vkCode))
                    {
                        return CallNextHookEx(this._keyboardHookId, nCode, wParam, lParam);
                    }

                    if (this._spaceToEnterEnabled)
                    {
                        if (IsTriggerNumberKey(vkCode) && this.IsFeatureActiveNow())
                        {
                            int duration = Math.Max(0, Math.Min(MAX_SPACE_TO_ENTER_DURATION_MS, this._spaceToEnterDurationMs));
                            lock (this._spaceEnterLock)
                            {
                                this._spaceAsEnterUntilTick = GetSafeTickCount64() + duration;
                            }
                        }
                        else if (vkCode == VK_SPACE && this.IsFeatureActiveNow())
                        {
                            bool shouldConvert;
                            lock (this._spaceEnterLock)
                            {
                                shouldConvert = GetSafeTickCount64() <= this._spaceAsEnterUntilTick;
                            }

                            if (shouldConvert)
                            {
                                SendEnterKey();
                                return (IntPtr)1;
                            }
                        }
                    }
                }
            }

            return CallNextHookEx(this._keyboardHookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Enter 키의 KeyDown 및 KeyUp 이벤트를 순차적으로 발생시킵니다.
        /// </summary>
        private static void SendEnterKey()
        {
            keybd_event(VK_RETURN, 0, 0, UIntPtr.Zero);
            keybd_event(VK_RETURN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        #endregion
    }
}