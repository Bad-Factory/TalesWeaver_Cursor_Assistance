using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TalesWeaver_Cursor_Assistance
{
    public partial class Form1
    {
        #region [1. 스킬 범위 오버레이 필드 및 상태 변수]
        /// <summary>스킬 범위 오버레이를 표시하는 GDI 투명 폼</summary>
        private GdiForm mSkillForm;
        /// <summary>현재 렌더링 중인 스킬 범위 비트맵 캐시</summary>
        private static Bitmap mBitmap_Skill;

        /// <summary>자기 중심 범위(마름모) 활성화 여부</summary>
        private bool SkillEnable_UserRange;
        /// <summary>직선 범위(타원, 1.7배 보정) 활성화 여부</summary>
        private bool SkillEnable_StraightRange;
        /// <summary>지정 범위(마우스 추적 타원) 활성화 여부</summary>
        private bool SkillEnable_CustomRange;
        /// <summary>사용자 지정 스킬 범위 색상 (null이면 모드별 기본색 사용)</summary>
        private Color? Skill_CustomColor = null;
        /// <summary>직선 범위 내 사거리 외곽선 원 색상</summary>
        private Color Skill_AttackRangeColor = Color.White;
        /// <summary>범위 연마 효과(+20) 적용 여부</summary>
        private bool Skill_RangeExtension;
        /// <summary>불시엔 아이템 효과(+7) 적용 여부</summary>
        private bool Skill_RangeExtensionItem;
        /// <summary>사용자 입력 기준 스킬 사거리 값 (30 ~ 60)</summary>
        private int Skill_Range;
        /// <summary>직선 범위 내 겹치는 외곽선 원(사거리 오버레이) 값 (직선 범위 값 이하)</summary>
        private int Skill_AttackRange = DEFAULT_SKILL_ATTACK_RANGE;
        /// <summary>계산된 스킬 범위 오버레이 가로 픽셀 크기</summary>
        private int Skill_RangeWidth;
        /// <summary>계산된 스킬 범위 오버레이 세로 픽셀 크기</summary>
        private int Skill_RangeHeight;
        /// <summary>스킬 범위 오버레이 불투명도 값 (0 ~ 255)</summary>
        private byte Skill_RangeOpacity;
        /// <summary>스킬 범위 세로 최소 픽셀 제한</summary>
        private int Skill_Range_Min;
        /// <summary>스킬 범위 세로 최대 픽셀 제한</summary>
        private int Skill_Range_Max;
        #endregion

        #region [2. 스킬 범위 모듈 초기화 및 해제]
        /// <summary>
        /// 스킬 범위 오버레이 폼 및 기본 상태값을 초기화합니다.
        /// </summary>
        private void InitializeSkillModule()
        {
            this.mSkillForm = new GdiForm();
            this.SkillEnable_UserRange = false;
            this.SkillEnable_StraightRange = false;
            this.SkillEnable_CustomRange = false;
            this.Skill_CustomColor = null;
            this.Skill_AttackRangeColor = Color.White;
            this.Skill_Range = 44;
            this.Skill_AttackRange = DEFAULT_SKILL_ATTACK_RANGE;
            this.Skill_RangeHeight = this.Skill_Range * 10;
            this.Skill_RangeWidth = this.Skill_RangeHeight * 2;
            this.Skill_RangeOpacity = 100;
            this.Skill_Range_Min = 300;
            this.Skill_Range_Max = 1280;
        }

        /// <summary>
        /// Form1_Load 시 스킬 범위 관련 UI 컨트롤 상태를 동기화합니다.
        /// </summary>
        private void InitSkillUiOnLoad()
        {
            if (this.textBox_Skill_Range != null) this.textBox_Skill_Range.Text = this.Skill_Range.ToString();
            if (this.textBox_Skill_AttackRange != null) this.textBox_Skill_AttackRange.Text = this.Skill_AttackRange.ToString();
            if (this.checkBox_Skill_RangeExtension != null) this.checkBox_Skill_RangeExtension.Checked = this.Skill_RangeExtension;
            if (this.checkBox_Skill_RangeExtensionItem != null) this.checkBox_Skill_RangeExtensionItem.Checked = this.Skill_RangeExtensionItem;
            if (this.radioButton_Skill_UserRange != null) this.radioButton_Skill_UserRange.Checked = this.SkillEnable_UserRange;
            if (this.radioButton_Skill_StraightRange != null) this.radioButton_Skill_StraightRange.Checked = this.SkillEnable_StraightRange;
            if (this.radioButton_Skill_CustomRange != null) this.radioButton_Skill_CustomRange.Checked = this.SkillEnable_CustomRange;
            if (this.radioButton_Skill_None != null && !this.SkillEnable_UserRange && !this.SkillEnable_StraightRange && !this.SkillEnable_CustomRange)
            {
                this.radioButton_Skill_None.Checked = true;
            }
            if (this.Skill_Range_Opacity != null) this.Skill_Range_Opacity.Value = this.Skill_RangeOpacity;
            this.SyncAttackRangeControlState();
            this.SyncSkillColorButtonUi();
            this.SyncAttackRangeColorButtonUi();
        }

        /// <summary>
        /// Form1_Shown 시 초기화 누락 시 스킬 범위 UI를 재동기화합니다.
        /// </summary>
        private void InitSkillUiOnShown()
        {
            if (this.textBox_Skill_Range != null) this.textBox_Skill_Range.Text = this.Skill_Range.ToString();
            if (this.textBox_Skill_AttackRange != null) this.textBox_Skill_AttackRange.Text = this.Skill_AttackRange.ToString();
            this.SyncAttackRangeControlState();
            this.SyncSkillColorButtonUi();
            this.SyncAttackRangeColorButtonUi();
        }

        /// <summary>
        /// 스킬 범위 오버레이 폼 및 비트맵 리소스를 정리합니다.
        /// </summary>
        private void DisposeSkillModule()
        {
            if (this.mSkillForm != null)
            {
                this.mSkillForm.Hide();
                this.mSkillForm.Dispose();
                this.mSkillForm = null;
            }
            if (Form1.mBitmap_Skill != null)
            {
                Form1.mBitmap_Skill.Dispose();
                Form1.mBitmap_Skill = null;
            }
        }
        #endregion

        #region [3. 스킬 범위 오버레이 위치 계산, 표시 및 추적]
        /// <summary>
        /// 기준 사각형(TalesWeaver 클라이언트 영역 또는 주 디스플레이)의 중심 좌표를 반환합니다.
        /// </summary>
        private bool TryGetSkillReferenceBounds(out Rectangle referenceRect)
        {
            IntPtr targetHwnd = this.GetTargetProcessWindowHandle();
            if (targetHwnd != IntPtr.Zero && GetClientRect(targetHwnd, out RECT clientRect))
            {
                int clientWidth = clientRect.Right - clientRect.Left;
                int clientHeight = clientRect.Bottom - clientRect.Top;
                if (clientWidth > 0 && clientHeight > 0)
                {
                    Point topLeft = new Point(clientRect.Left, clientRect.Top);
                    if (ClientToScreen(targetHwnd, ref topLeft))
                    {
                        referenceRect = new Rectangle(topLeft.X, topLeft.Y, clientWidth, clientHeight);
                        return true;
                    }
                }
            }

            referenceRect = Screen.PrimaryScreen.Bounds;
            return false;
        }

        /// <summary>
        /// 스킬 범위 오버레이를 숨깁니다.
        /// </summary>
        private void HideSkillOverlay()
        {
            if (this.mSkillForm != null && this.mSkillForm.Visible)
            {
                this.mSkillForm.Hide();
            }
        }

        /// <summary>
        /// 스킬 범위 모드가 활성화된 경우 오버레이를 표시합니다.
        /// </summary>
        private void ShowSkillOverlayIfEnabled()
        {
            bool isSkillEnabled = this.SkillEnable_UserRange || this.SkillEnable_StraightRange || this.SkillEnable_CustomRange;
            if (isSkillEnabled && this.mSkillForm != null && Form1.mBitmap_Skill != null)
            {
                this.PositionAndShowSkillForm();
            }
        }

        /// <summary>
        /// 지정 범위 모드 활성화 시 마우스 좌표를 따라 스킬 오버레이 위치를 갱신합니다.
        /// </summary>
        private void UpdateCustomSkillPosition(int x, int y)
        {
            if (this.SkillEnable_CustomRange && this.mSkillForm != null && Form1.mBitmap_Skill != null)
            {
                int width = Form1.mBitmap_Skill.Width;
                int height = Form1.mBitmap_Skill.Height;
                this.mSkillForm.Left = x - width / 2;
                this.mSkillForm.Top = y - height / 2;
            }
        }

        /// <summary>
        /// TalesWeaver 창 크기(클라이언트 영역) 또는 커서 추적 기준에 맞춰 스킬 오버레이 좌표를 계산하고 비활성 상태로 표시합니다.
        /// </summary>
        private void PositionAndShowSkillForm()
        {
            if (this.mSkillForm == null || Form1.mBitmap_Skill == null) return;

            int width = Form1.mBitmap_Skill.Width;
            int height = Form1.mBitmap_Skill.Height;

            this.TryGetSkillReferenceBounds(out Rectangle refBounds);
            int left = refBounds.Left + (refBounds.Width - width) / 2;
            int top = refBounds.Top + (refBounds.Height - height) / 2 - 30;

            if (this.SkillEnable_CustomRange)
            {
                Point cursorPos = Control.MousePosition;
                left = cursorPos.X - width / 2;
                top = cursorPos.Y - height / 2;
            }

            if (!this.mSkillForm.IsHandleCreated)
            {
                IntPtr _ = this.mSkillForm.Handle;
            }
            ShowWindow(this.mSkillForm.Handle, SW_SHOWNOACTIVATE);
            this.mSkillForm.Location = new Point(left, top);
            this.mSkillForm.SetBitmap(Form1.mBitmap_Skill, this.Skill_RangeOpacity);
            this.mSkillForm.Location = new Point(left, top);
        }
        #endregion

        #region [4. 스킬 범위 색상/컨트롤 동기화, 렌더링 및 설정 적용/로드]
        /// <summary>
        /// 직선 범위 모드 활성화 여부에 따라 사거리 입력 및 사거리 원 색상 컨트롤의 사용 가능 상태를 동기화합니다.
        /// </summary>
        private void SyncAttackRangeControlState()
        {
            bool enabled = this.SkillEnable_StraightRange;
            if (this.textBox_Skill_AttackRange != null)
            {
                this.textBox_Skill_AttackRange.Enabled = enabled;
            }
            if (this.label_Skill_AttackRange != null)
            {
                this.label_Skill_AttackRange.Enabled = enabled;
            }
            if (this.button_Skill_AttackRangeColor != null)
            {
                this.button_Skill_AttackRangeColor.Enabled = enabled;
            }
        }

        /// <summary>
        /// 버튼 배경색 대비 가독성 높은 전경색(검정 또는 흰색)을 반환합니다.
        /// </summary>
        private static Color GetContrastingForeColor(Color backgroundColor)
        {
            int brightness = (backgroundColor.R * 299 + backgroundColor.G * 587 + backgroundColor.B * 114) / 1000;
            return brightness >= 128 ? Color.Black : Color.White;
        }

        /// <summary>
        /// 현재 선택된 스킬 범위 색상을 반환합니다(사용자 지정 색상이 있으면 우선 적용).
        /// </summary>
        private Color GetActiveSkillRangeColor()
        {
            if (this.Skill_CustomColor.HasValue)
            {
                return this.Skill_CustomColor.Value;
            }
            if (this.SkillEnable_StraightRange)
            {
                return Color.Orange;
            }
            if (this.SkillEnable_CustomRange)
            {
                return Color.Red;
            }
            return Color.Cyan;
        }

        /// <summary>
        /// 스킬 범위 색상 버튼의 배경색과 전경색을 현재 적용된 색상에 맞춰 동기화합니다.
        /// </summary>
        private void SyncSkillColorButtonUi()
        {
            if (this.button_Skill_RangeColor == null) return;

            Color c = this.GetActiveSkillRangeColor();
            this.button_Skill_RangeColor.UseVisualStyleBackColor = false;
            this.button_Skill_RangeColor.BackColor = c;
            this.button_Skill_RangeColor.ForeColor = GetContrastingForeColor(c);
        }

        /// <summary>
        /// 사거리 외곽선 원 색상 버튼의 배경색과 전경색을 현재 적용된 색상에 맞춰 동기화합니다.
        /// </summary>
        private void SyncAttackRangeColorButtonUi()
        {
            if (this.button_Skill_AttackRangeColor == null) return;

            Color c = this.Skill_AttackRangeColor;
            this.button_Skill_AttackRangeColor.UseVisualStyleBackColor = false;
            this.button_Skill_AttackRangeColor.BackColor = c;
            this.button_Skill_AttackRangeColor.ForeColor = GetContrastingForeColor(c);
        }

        /// <summary>
        /// UI에서 스킬 사거리 및 직선 사거리 오버레이 입력값을 검증하고 도형을 다시 그립니다.
        /// </summary>
        private void ApplySkillSettingsFromUi()
        {
            int skillRange = 0;
            if (this.textBox_Skill_Range != null)
            {
                if (!int.TryParse(this.textBox_Skill_Range.Text, out skillRange) || skillRange < 30 || skillRange > 60)
                {
                    if (skillRange < 30)
                    {
                        MessageBox.Show("30 ~ 60 숫자만 입력 가능합니다.");
                        skillRange = 30;
                    }
                    else if (skillRange > 60)
                    {
                        MessageBox.Show("30 ~ 60 숫자만 입력 가능합니다.");
                        skillRange = 60;
                    }
                    this.textBox_Skill_Range.Text = skillRange.ToString();
                }
            }
            else
            {
                skillRange = 44;
            }
            this.Skill_Range = skillRange;

            int attackRange = DEFAULT_SKILL_ATTACK_RANGE;
            if (this.textBox_Skill_AttackRange != null)
            {
                if (!int.TryParse(this.textBox_Skill_AttackRange.Text, out attackRange) || attackRange < 0)
                {
                    MessageBox.Show("사거리는 0 이상의 숫자만 입력 가능합니다.");
                    attackRange = Math.Min(DEFAULT_SKILL_ATTACK_RANGE, this.Skill_Range);
                }
                if (attackRange > this.Skill_Range)
                {
                    MessageBox.Show($"사거리는 직선 범위({this.Skill_Range})보다 클 수 없습니다.");
                    attackRange = this.Skill_Range;
                }
                this.textBox_Skill_AttackRange.Text = attackRange.ToString();
            }
            else if (attackRange > this.Skill_Range)
            {
                attackRange = this.Skill_Range;
            }
            this.Skill_AttackRange = attackRange;

            this.SetSkillShape();
        }

        /// <summary>
        /// INI 키-값이 스킬 범위 설정 항목이면 파싱하여 반영합니다.
        /// </summary>
        private bool TryLoadSkillSetting(string key, string value)
        {
            if (key == "SkillEnable_UserRange" && bool.TryParse(value, out bool parsedUserRange))
            {
                this.SkillEnable_UserRange = parsedUserRange;
                return true;
            }
            if (key == "SkillEnable_StraightRange" && bool.TryParse(value, out bool parsedStraightRange))
            {
                this.SkillEnable_StraightRange = parsedStraightRange;
                return true;
            }
            if (key == "SkillEnable_CustomRange" && bool.TryParse(value, out bool parsedCustomRange))
            {
                this.SkillEnable_CustomRange = parsedCustomRange;
                return true;
            }
            if (key == "Skill_RangeColor" && !string.IsNullOrEmpty(value) && !string.Equals(value, "Default", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Color parsedColor = ColorTranslator.FromHtml(value);
                    if (!parsedColor.IsEmpty)
                    {
                        this.Skill_CustomColor = parsedColor;
                    }
                }
                catch
                {
                }
                return true;
            }
            if (key == "Skill_AttackRangeColor" && !string.IsNullOrEmpty(value))
            {
                try
                {
                    Color parsedAttackColor = ColorTranslator.FromHtml(value);
                    if (!parsedAttackColor.IsEmpty)
                    {
                        this.Skill_AttackRangeColor = parsedAttackColor;
                    }
                }
                catch
                {
                }
                return true;
            }
            if (key == "Skill_RangeExtension" && bool.TryParse(value, out bool parsedRangeExtension))
            {
                this.Skill_RangeExtension = parsedRangeExtension;
                if (this.checkBox_Skill_RangeExtension != null) this.checkBox_Skill_RangeExtension.Checked = parsedRangeExtension;
                return true;
            }
            if (key == "Skill_RangeExtensionItem" && bool.TryParse(value, out bool parsedRangeExtensionItem))
            {
                this.Skill_RangeExtensionItem = parsedRangeExtensionItem;
                if (this.checkBox_Skill_RangeExtensionItem != null) this.checkBox_Skill_RangeExtensionItem.Checked = parsedRangeExtensionItem;
                return true;
            }
            if (key == "Skill_Range" && int.TryParse(value, out int parsedSkillRange))
            {
                this.Skill_Range = parsedSkillRange;
                return true;
            }
            if (key == "Skill_AttackRange" && int.TryParse(value, out int parsedAttackRange))
            {
                this.Skill_AttackRange = parsedAttackRange;
                return true;
            }
            if (key == "Skill_RangeOpacity" && byte.TryParse(value, out byte parsedSkillOpacity))
            {
                this.Skill_RangeOpacity = parsedSkillOpacity;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 스킬 범위 관련 설정값의 유효 범위를 보정하고 기본 치수를 계산합니다.
        /// </summary>
        private void NormalizeSkillSettings()
        {
            if (this.Skill_AttackRangeColor.IsEmpty) this.Skill_AttackRangeColor = Color.White;
            if (this.Skill_Range <= 0) this.Skill_Range = 44;
            if (this.Skill_AttackRange < 0) this.Skill_AttackRange = DEFAULT_SKILL_ATTACK_RANGE;
            if (this.Skill_AttackRange > this.Skill_Range) this.Skill_AttackRange = this.Skill_Range;
            if (this.Skill_RangeOpacity <= 0) this.Skill_RangeOpacity = byte.MaxValue;

            int calcHeight = Math.Max(this.Skill_Range_Min, Math.Min(this.Skill_Range_Max, this.Skill_Range * 10));
            this.Skill_RangeHeight = calcHeight;
            this.Skill_RangeWidth = calcHeight * 2;
        }

        /// <summary>
        /// 선택된 스킬 범위 모드와 연마/아이템 옵션에 맞춰 스킬 범위 도형 비트맵을 생성하고 활성 상태 시 오버레이 폼에 출력합니다.
        /// </summary>
        private void SetSkillShape()
        {
            try
            {
                this.SyncAttackRangeControlState();
                this.SyncSkillColorButtonUi();
                this.SyncAttackRangeColorButtonUi();

                bool isAnyEnabled = this.SkillEnable_UserRange || this.SkillEnable_StraightRange || this.SkillEnable_CustomRange;
                if (!isAnyEnabled)
                {
                    if (this.mSkillForm != null)
                    {
                        this.mSkillForm.Hide();
                    }
                    if (Form1.mBitmap_Skill != null)
                    {
                        Form1.mBitmap_Skill.Dispose();
                        Form1.mBitmap_Skill = null;
                    }
                    return;
                }

                Color activeColor = this.GetActiveSkillRangeColor();

                int effectiveRange = this.Skill_Range;
                bool isExtChecked = (this.checkBox_Skill_RangeExtension != null && this.checkBox_Skill_RangeExtension.Checked);
                bool isItemChecked = (this.checkBox_Skill_RangeExtensionItem != null && this.checkBox_Skill_RangeExtensionItem.Checked);

                if (this.SkillEnable_StraightRange)
                {
                    effectiveRange = (int)(effectiveRange * 1.7);
                }
                if (isExtChecked)
                {
                    effectiveRange += 20;
                }
                if (isItemChecked)
                {
                    effectiveRange += 7;
                }

                int pixelHeight = effectiveRange * 10;
                if (isExtChecked || isItemChecked && !this.SkillEnable_StraightRange)
                {
                    pixelHeight += 100;
                }

                pixelHeight = Math.Max(this.Skill_Range_Min, Math.Min(this.Skill_Range_Max, pixelHeight));

                int shapeWidth = pixelHeight * 2;
                int shapeHeight = pixelHeight;
                this.Skill_RangeWidth = shapeWidth;

                Bitmap image = new Bitmap(shapeWidth + 4, shapeHeight + 4);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(100, activeColor)))
                    using (Pen pen = new Pen(activeColor, 2f))
                    {
                        if (this.SkillEnable_UserRange)
                        {
                            PointF[] points = new PointF[]
                            {
                                new PointF((float)shapeWidth / 2f + 2f, 2f),
                                new PointF((float)(shapeWidth + 2), (float)shapeHeight / 2f + 2f),
                                new PointF((float)shapeWidth / 2f + 2f, (float)(shapeHeight + 2)),
                                new PointF(2f, (float)shapeHeight / 2f + 2f)
                            };
                            graphics.FillPolygon(solidBrush, points);
                            graphics.DrawPolygon(pen, points);
                        }
                        else if (this.SkillEnable_StraightRange || this.SkillEnable_CustomRange)
                        {
                            graphics.FillEllipse(solidBrush, 2f, 2f, (float)shapeWidth, (float)shapeHeight);
                            graphics.DrawEllipse(pen, 2f, 2f, (float)shapeWidth, (float)shapeHeight);

                            if (this.SkillEnable_StraightRange)
                            {
                                int clampedAttackRange = Math.Max(0, Math.Min(this.Skill_Range, this.Skill_AttackRange));
                                int attackEffectiveRange = (int)(clampedAttackRange * 1.7);
                                if (isExtChecked)
                                {
                                    attackEffectiveRange += 20;
                                }
                                if (isItemChecked)
                                {
                                    attackEffectiveRange += 7;
                                }

                                int attackHeight = Math.Max(
                                    this.Skill_Range_Min,
                                    Math.Min(shapeHeight, attackEffectiveRange * 10));
                                int attackWidth = Math.Min(shapeWidth, attackHeight * 2);

                                float circleLeft = 2f + (shapeWidth - attackWidth) / 2f;
                                float circleTop = 2f + (shapeHeight - attackHeight) / 2f;

                                using (Pen attackOutlinePen = new Pen(this.Skill_AttackRangeColor, 2f))
                                {
                                    graphics.DrawEllipse(attackOutlinePen, circleLeft, circleTop, (float)attackWidth, (float)attackHeight);
                                }
                            }
                        }
                    }
                }

                Bitmap previousSkillBitmap = Form1.mBitmap_Skill;
                Form1.mBitmap_Skill = image;

                if (this.mIsLoadForm && this.IsFeatureActiveNow())
                {
                    this.PositionAndShowSkillForm();
                }
                else if (this.mSkillForm != null)
                {
                    this.mSkillForm.Hide();
                }

                if (previousSkillBitmap != null && !ReferenceEquals(previousSkillBitmap, image))
                {
                    previousSkillBitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("범위 생성 실패: " + ex.Message);
            }
        }
        #endregion

        #region [5. 스킬 범위 UI 이벤트 핸들러]
        /// <summary>
        /// 스킬 범위 오버레이 색상 선택 대화상자를 열고 선택한 색상을 즉시 반영합니다.
        /// </summary>
        private void button_Skill_RangeColor_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.FullOpen = true;
                    dialog.Color = this.GetActiveSkillRangeColor();

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        this.Skill_CustomColor = dialog.Color;
                        this.SyncSkillColorButtonUi();
                        this.SetSkillShape();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("색상 변경 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// 직선 범위 내 사거리 외곽선 원 색상 선택 대화상자를 열고 선택한 색상을 즉시 반영합니다.
        /// </summary>
        private void button_Skill_AttackRangeColor_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.FullOpen = true;
                    dialog.Color = this.Skill_AttackRangeColor;

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        this.Skill_AttackRangeColor = dialog.Color;
                        this.SyncAttackRangeColorButtonUi();
                        this.SetSkillShape();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("사거리 원 색상 변경 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '[사용안함]' 라디오 버튼 선택 시 모든 스킬 범위 오버레이를 비활성화합니다.
        /// </summary>
        private void radioButton_Skill_None_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton_Skill_None != null && this.radioButton_Skill_None.Checked)
                {
                    this.SkillEnable_UserRange = false;
                    this.SkillEnable_StraightRange = false;
                    this.SkillEnable_CustomRange = false;
                    this.SyncAttackRangeControlState();
                    this.SetSkillShape();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 적용 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '[자기 중심 범위]' 라디오 버튼 상태 변경 시 오버레이를 갱신합니다.
        /// </summary>
        private void radioButton_SkillEnable_UserRange_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton_Skill_UserRange != null)
                {
                    this.SkillEnable_UserRange = this.radioButton_Skill_UserRange.Checked;
                }
                if (this.SkillEnable_UserRange)
                {
                    this.SkillEnable_StraightRange = false;
                    this.SkillEnable_CustomRange = false;
                }
                this.SyncAttackRangeControlState();

                if (this.textBox_Skill_Range != null && int.TryParse(this.textBox_Skill_Range.Text, out int range))
                {
                    int height = Math.Max(this.Skill_Range_Min, Math.Min(this.Skill_Range_Max, range * 10));
                    this.Skill_RangeHeight = height;
                    this.Skill_RangeWidth = height * 2;
                }
                this.SetSkillShape();
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 적용 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '[직선 범위]' 라디오 버튼 상태 변경 시 오버레이 및 사거리 컨트롤 상태를 갱신합니다.
        /// </summary>
        private void radioButton_Skill_StraightRange_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton_Skill_StraightRange != null)
                {
                    this.SkillEnable_StraightRange = this.radioButton_Skill_StraightRange.Checked;
                }
                if (this.SkillEnable_StraightRange)
                {
                    this.SkillEnable_UserRange = false;
                    this.SkillEnable_CustomRange = false;
                }
                this.SyncAttackRangeControlState();

                if (this.textBox_Skill_Range != null && int.TryParse(this.textBox_Skill_Range.Text, out int range))
                {
                    int height = Math.Max(this.Skill_Range_Min, Math.Min(this.Skill_Range_Max, range * 10));
                    this.Skill_RangeHeight = height;
                    this.Skill_RangeWidth = height * 2;
                }
                this.SetSkillShape();
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 적용 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '[지정 범위]' 라디오 버튼 상태 변경 시 오버레이를 갱신합니다.
        /// </summary>
        private void radioButton_Skill_CustomRange_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton_Skill_CustomRange != null)
                {
                    this.SkillEnable_CustomRange = this.radioButton_Skill_CustomRange.Checked;
                }
                if (this.SkillEnable_CustomRange)
                {
                    this.SkillEnable_UserRange = false;
                    this.SkillEnable_StraightRange = false;
                }
                this.SyncAttackRangeControlState();

                if (this.textBox_Skill_Range != null && int.TryParse(this.textBox_Skill_Range.Text, out int range))
                {
                    int height = Math.Max(this.Skill_Range_Min, Math.Min(this.Skill_Range_Max, range * 10));
                    this.Skill_RangeHeight = height;
                    this.Skill_RangeWidth = height * 2;
                }
                this.SetSkillShape();
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 적용 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '범위 연마' 체크박스 변경 시 스킬 범위를 재계산하여 오버레이를 갱신합니다.
        /// </summary>
        private void checkBox_Skill_RangeExtension_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox_Skill_RangeExtension != null)
                {
                    this.Skill_RangeExtension = this.checkBox_Skill_RangeExtension.Checked;
                }
                this.SetSkillShape();
            }
            catch (Exception ex)
            {
                MessageBox.Show("범위 확장 설정 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// '불시엔' 체크박스 변경 시 스킬 범위를 재계산하여 오버레이를 갱신합니다.
        /// </summary>
        private void checkBox_Skill_RangeExtensionItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox_Skill_RangeExtensionItem != null)
                {
                    this.Skill_RangeExtensionItem = this.checkBox_Skill_RangeExtensionItem.Checked;
                }
                this.SetSkillShape();
            }
            catch (Exception ex)
            {
                MessageBox.Show("불시엔 설정 실패: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// 스킬 범위 투명도 트랙바 스크롤 시 오버레이 불투명도를 실시간으로 조절합니다.
        /// </summary>
        private void Skill_RangeOpacity_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (sender is TrackBar trackBar)
                {
                    this.Skill_RangeOpacity = (byte)trackBar.Value;
                    this.SetSkillShape();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("투명도 조절 중 오류 발생: " + ex.Message);
            }
        }
        #endregion
    }
}