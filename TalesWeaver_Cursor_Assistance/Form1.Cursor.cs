using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TalesWeaver_Cursor_Assistance
{
    public partial class Form1
    {
        #region [1. 커서 오버레이 필드 및 상태 변수]
        /// <summary>마우스 커서 오버레이를 표시하는 GDI 투명 폼</summary>
        private GdiForm mCursorForm;
        /// <summary>현재 렌더링 중인 마우스 커서 비트맵 캐시</summary>
        private static Bitmap mBitmap_Mouse;

        /// <summary>현재 적용된 커서의 Base64 이미지 문자열</summary>
        private string CursorBase64 = PRESET_DEFAULT;
        /// <summary>커서 크기 배율 값 (렌더링 시 x2 픽셀 적용)</summary>
        private int CursorSize;
        /// <summary>커서 불투명도 값 (0 ~ 255)</summary>
        private byte CursorOpacity;
        /// <summary>직전 마우스 클릭(누름) 상태 캐시</summary>
        private bool _isMousePressedCached;
        #endregion

        #region [2. 커서 모듈 초기화 및 해제]
        /// <summary>
        /// 커서 오버레이 폼 및 기본 상태값을 초기화합니다.
        /// </summary>
        private void InitializeCursorModule()
        {
            this.mCursorForm = new GdiForm();
            this.CursorSize = 40;
            this.CursorOpacity = byte.MaxValue;
        }

        /// <summary>
        /// Form1_Load 시 커서 관련 UI를 초기화하고 커서 이미지를 적용합니다.
        /// </summary>
        private void InitCursorUiOnLoad()
        {
            if (this.textBox_Mouse_Size != null) this.textBox_Mouse_Size.Text = this.CursorSize.ToString();
            if (this.Mouse_Opacity != null) this.Mouse_Opacity.Value = this.CursorOpacity;
            if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.Text = this.CursorBase64;

            this._isMousePressedCached = IsAnyMouseButtonPressed();
            this.UpdatePresetSelection();
            this.SetCursor();
        }

        /// <summary>
        /// Form1_Shown 시 초기화 누락 시 커서 UI를 재동기화합니다.
        /// </summary>
        private void InitCursorUiOnShown()
        {
            if (this.textBox_Mouse_Size != null) this.textBox_Mouse_Size.Text = this.CursorSize.ToString();
            if (this.Mouse_Opacity != null) this.Mouse_Opacity.Value = this.CursorOpacity;
            if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.Text = this.CursorBase64;
            this.UpdatePresetSelection();
            this.SetCursor();
        }

        /// <summary>
        /// 커서 오버레이 폼 및 비트맵 리소스를 정리합니다.
        /// </summary>
        private void DisposeCursorModule()
        {
            if (this.mCursorForm != null)
            {
                this.mCursorForm.Hide();
                this.mCursorForm.Dispose();
                this.mCursorForm = null;
            }
            if (Form1.mBitmap_Mouse != null)
            {
                Form1.mBitmap_Mouse.Dispose();
                Form1.mBitmap_Mouse = null;
            }
        }
        #endregion

        #region [3. 커서 오버레이 표시, 위치 추적 및 클릭 상태 동기화]
        /// <summary>
        /// 커서 오버레이를 숨깁니다.
        /// </summary>
        private void HideCursorOverlay()
        {
            if (this.mCursorForm != null && this.mCursorForm.Visible)
            {
                this.mCursorForm.Hide();
            }
        }

        /// <summary>
        /// 현재 마우스 위치에 커서 오버레이를 비활성 상태로 표시합니다.
        /// </summary>
        private void ShowCursorOverlay()
        {
            if (this.mCursorForm == null || Form1.mBitmap_Mouse == null) return;

            if (!this.mCursorForm.IsHandleCreated)
            {
                IntPtr _ = this.mCursorForm.Handle;
            }
            Point cursorPos = Control.MousePosition;
            this.mCursorForm.Location = new Point(cursorPos.X - 1, cursorPos.Y - 1);
            ShowWindow(this.mCursorForm.Handle, SW_SHOWNOACTIVATE);
            this.mCursorForm.SetBitmap(Form1.mBitmap_Mouse, this.CursorOpacity);
            this.mCursorForm.Location = new Point(cursorPos.X - 1, cursorPos.Y - 1);
            this.EnsureCursorOverlayAboveSkillOverlay();
        }

        /// <summary>
        /// 마우스 이동 좌표로 커서 오버레이 위치를 갱신합니다.
        /// </summary>
        private void UpdateCursorPosition(int x, int y)
        {
            if (this.mCursorForm != null)
            {
                this.mCursorForm.Left = x - 1;
                this.mCursorForm.Top = y - 1;
            }
        }

        /// <summary>
        /// 마우스 버튼이 현재 눌려 있는지 여부를 반환합니다.
        /// </summary>
        private static bool IsAnyMouseButtonPressed()
        {
            return Control.MouseButtons != MouseButtons.None;
        }

        /// <summary>
        /// 마우스 클릭(누름) 상태가 변경되었을 때 커서 크기를 갱신합니다(클릭 중 90%).
        /// </summary>
        private void SyncMousePressStateIfChanged()
        {
            bool isPressed = IsAnyMouseButtonPressed();
            if (isPressed != this._isMousePressedCached)
            {
                this._isMousePressedCached = isPressed;
                this.SetCursor();
            }
        }
        #endregion

        #region [4. 커서 렌더링 및 설정 적용/로드]
        /// <summary>
        /// UI에서 커서 크기 및 투명도 입력값을 검증하고 적용합니다.
        /// </summary>
        private void ApplyCursorSettingsFromUi()
        {
            int cursorSize = 0;
            if (this.textBox_Mouse_Size != null && !int.TryParse(this.textBox_Mouse_Size.Text, out cursorSize) || cursorSize == 0)
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                cursorSize = 40;
                this.textBox_Mouse_Size.Text = cursorSize.ToString();
            }
            this.CursorSize = cursorSize;

            byte cursorOpacity = 0;
            if (this.Mouse_Opacity != null && !byte.TryParse(this.Mouse_Opacity.Value.ToString(), out cursorOpacity))
            {
                MessageBox.Show("0부터 255까지의 숫자만 입력 가능합니다.");
                cursorOpacity = byte.MaxValue;
                this.Mouse_Opacity.Value = cursorOpacity;
            }
            this.CursorOpacity = cursorOpacity;

            this._isMousePressedCached = IsAnyMouseButtonPressed();
            this.SetCursor();
        }

        /// <summary>
        /// INI 키-값이 커서 설정 항목이면 파싱하여 반영합니다.
        /// </summary>
        private bool TryLoadCursorSetting(string key, string value)
        {
            if (key == "Size" && int.TryParse(value, out int parsedCursorSize))
            {
                this.CursorSize = parsedCursorSize;
                return true;
            }
            if (key == "Opacity" && byte.TryParse(value, out byte parsedCursorOpacity))
            {
                this.CursorOpacity = parsedCursorOpacity;
                return true;
            }
            if (key == "Cursor" && !string.IsNullOrEmpty(value))
            {
                this.CursorBase64 = value.Replace("\r", "").Replace("\n", "").Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 커서 관련 설정값의 유효 범위를 보정합니다.
        /// </summary>
        private void NormalizeCursorSettings()
        {
            if (this.CursorSize <= 0) this.CursorSize = 20;
            if (this.CursorOpacity == 0) this.CursorOpacity = byte.MaxValue;
            if (string.IsNullOrEmpty(this.CursorBase64)) this.CursorBase64 = PRESET_DEFAULT;
        }

        /// <summary>
        /// 현재 Base64 문자열과 크기/투명도/클릭 상태(누름 시 90%)를 바탕으로 커서 비트맵을 생성하여 오버레이 폼에 적용합니다.
        /// </summary>
        private void SetCursor()
        {
            try
            {
                string base64 = string.IsNullOrEmpty(this.CursorBase64)
                    ? PRESET_DEFAULT
                    : this.CursorBase64.Replace("\r", "").Replace("\n", "").Trim();

                byte[] buffer;
                try
                {
                    buffer = Convert.FromBase64String(base64);
                }
                catch
                {
                    base64 = PRESET_DEFAULT;
                    this.CursorBase64 = base64;
                    buffer = Convert.FromBase64String(base64);
                }

                double pressScale = this._isMousePressedCached ? 0.9 : 1.0;
                int targetSize = Math.Max(1, (int)Math.Round(this.CursorSize * 2 * pressScale));

                Bitmap nextBitmap;
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                using (Bitmap bitmap = new Bitmap(memoryStream))
                {
                    nextBitmap = new Bitmap(bitmap, new Size(targetSize, targetSize));
                }

                Bitmap previousBitmap = Form1.mBitmap_Mouse;
                Form1.mBitmap_Mouse = nextBitmap;

                if (this.mIsLoadForm && this.IsFeatureActiveNow() && this.mCursorForm != null)
                {
                    this.mCursorForm.SetBitmap(Form1.mBitmap_Mouse, this.CursorOpacity);
                    this.EnsureCursorOverlayAboveSkillOverlay();
                }

                if (previousBitmap != null && !ReferenceEquals(previousBitmap, nextBitmap))
                {
                    previousBitmap.Dispose();
                }
            }
            catch
            {
            }
        }
        #endregion

        #region [5. 커서 UI 이벤트 핸들러]
        /// <summary>
        /// 커서 투명도 트랙바 스크롤 시 커서 불투명도를 실시간으로 변경합니다.
        /// </summary>
        private void Mouse_Opacity_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (sender is TrackBar trackBar)
                {
                    this.CursorOpacity = (byte)trackBar.Value;
                    this.SetCursor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("투명도 조절 중 오류 발생: " + ex.Message);
            }
        }

        /// <summary>
        /// 입력된 Base64 이미지 유효성을 검사하고 커서를 교체합니다.
        /// </summary>
        private void ApplyCursorBase64FromUi()
        {
            try
            {
                if (this.textBox_CursorBase64 == null) return;
                string text = this.textBox_CursorBase64.Text.Replace("\r", "").Replace("\n", "").Trim();
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("Base64 코드를 입력해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text)))
                using (new Bitmap(memoryStream))
                {
                }

                this.CursorBase64 = text;
                this.textBox_CursorBase64.Text = text;
                this.UpdatePresetSelection();
                this.SetCursor();
            }
            catch (Exception ex)
            {
                MessageBox.Show("유효하지 않은 Base64 이미지 코드입니다: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// 커서 Base64 입력란에서 Enter 키 입력 시 커서를 변경합니다.
        /// </summary>
        private void textBox_CursorBase64_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.ApplyCursorBase64FromUi();
            }
        }

        /// <summary>
        /// Base64 변환 웹사이트 링크 클릭 시 기본 브라우저로 페이지를 엽니다.
        /// </summary>
        private void linkLabel_CursorUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://www.base64.sh/ko/image-to-base64/");
            }
            catch (Exception ex)
            {
                MessageBox.Show("링크를 열 수 없습니다: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// 커서 프리셋 콤보박스 선택 변경 시 프리셋 데이터를 반영하고 텍스트박스 편집 가능 여부를 제어합니다.
        /// </summary>
        private void comboBox_CursorPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.mIsLoadForm || this.comboBox_CursorPreset == null)
            {
                return;
            }

            string selected = this.comboBox_CursorPreset.SelectedItem?.ToString();
            if (selected == "기본 커서 (Default)")
            {
                this.CursorBase64 = PRESET_DEFAULT;
                if (this.textBox_CursorBase64 != null)
                {
                    this.textBox_CursorBase64.Text = PRESET_DEFAULT;
                    this.textBox_CursorBase64.ReadOnly = true;
                }
                this.SetCursor();
            }
            else if (selected == "옐로 커서 (Yellow)")
            {
                this.CursorBase64 = PRESET_YELLOW;
                if (this.textBox_CursorBase64 != null)
                {
                    this.textBox_CursorBase64.Text = PRESET_YELLOW;
                    this.textBox_CursorBase64.ReadOnly = true;
                }
                this.SetCursor();
            }
            else if (selected == "레드 커서 (Red)")
            {
                this.CursorBase64 = PRESET_RED;
                if (this.textBox_CursorBase64 != null)
                {
                    this.textBox_CursorBase64.Text = PRESET_RED;
                    this.textBox_CursorBase64.ReadOnly = true;
                }
                this.SetCursor();
            }
            else if (selected == "직접 입력 (Custom)" && this.textBox_CursorBase64 != null)
            {
                this.textBox_CursorBase64.ReadOnly = false;
            }
        }

        /// <summary>
        /// 현재 적용된 CursorBase64 값에 맞춰 프리셋 콤보박스와 텍스트박스 읽기 전용 상태를 동기화합니다.
        /// </summary>
        private void UpdatePresetSelection()
        {
            if (this.comboBox_CursorPreset == null) return;

            if (this.CursorBase64 == PRESET_DEFAULT)
            {
                this.comboBox_CursorPreset.SelectedItem = "기본 커서 (Default)";
                if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.ReadOnly = true;
            }
            else if (this.CursorBase64 == PRESET_YELLOW)
            {
                this.comboBox_CursorPreset.SelectedItem = "옐로 커서 (Yellow)";
                if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.ReadOnly = true;
            }
            else if (this.CursorBase64 == PRESET_RED)
            {
                this.comboBox_CursorPreset.SelectedItem = "레드 커서 (Red)";
                if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.ReadOnly = true;
            }
            else
            {
                this.comboBox_CursorPreset.SelectedItem = "직접 입력 (Custom)";
                if (this.textBox_CursorBase64 != null) this.textBox_CursorBase64.ReadOnly = false;
            }
        }
        #endregion
    }
}