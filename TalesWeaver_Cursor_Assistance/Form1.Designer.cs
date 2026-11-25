namespace TalesWeaver_Cursor_Assistance
{
    // Token: 0x02000010 RID: 16
    public partial class Form1 : global::System.Windows.Forms.Form
    {
        #region [1. 폼 소멸 및 리소스 해제 기능 (Dispose)]
        // =========================================================================
        // [메소드 분류] Dispose : 폼 종료 시 메모리 및 관리 리소스(트레이 아이콘 등) 해제
        // =========================================================================
        // Token: 0x0600003A RID: 58 RVA: 0x00002112 File Offset: 0x00000312
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.notifyIcon1 != null)
                {
                    this.notifyIcon1.Dispose(); // 시스템 트레이 아이콘 관리 리소스 해제
                }
                if (this.components != null)
                {
                    this.components.Dispose(); // 컴포넌트 컨테이너 관리 리소스 해제
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region [2. UI 컴포넌트 초기화 및 배치 기능 (InitializeComponent)]
        // =========================================================================
        // [메소드 분류] InitializeComponent : 폼 UI 컨트롤 생성, 속성 지정 및 이벤트 바인딩
        // =========================================================================
        // Token: 0x0600003B RID: 59 RVA: 0x0000351C File Offset: 0x0000171C
        private void InitializeComponent()
        {
            #region [2-1. 리소스 매니저 및 기본 인스턴스 생성]
            // 리소스 매니저 초기화 (아이콘 파일 및 임베디드 리소스 로드에 사용)
            global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TalesWeaver_Cursor_Assistance.Form1));

            // [기능별 분류] 각 그룹박스, 컨트롤 및 트레이 구성 요소 인스턴스 생성
            this.groupBox_MouseHighlight = new global::System.Windows.Forms.GroupBox();
            this.label1 = new global::System.Windows.Forms.Label();
            this.textBox_Mouse_Size = new global::System.Windows.Forms.TextBox();
            this.label2 = new global::System.Windows.Forms.Label();
            this.Mouse_Opacity = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Mouse_Opacity)).BeginInit();

            this.toolTip1 = new global::System.Windows.Forms.ToolTip();

            this.groupBox_CustomCursor = new global::System.Windows.Forms.GroupBox();
            this.label_CursorDesc = new global::System.Windows.Forms.Label();
            global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
            this.comboBox_CursorPreset = new global::System.Windows.Forms.ComboBox();
            this.linkLabel_CursorUrl = new global::System.Windows.Forms.LinkLabel();
            this.textBox_CursorBase64 = new global::System.Windows.Forms.TextBox();

            this.groupBox_Skill = new global::System.Windows.Forms.GroupBox();
            this.radioButton_Skill_None = new global::System.Windows.Forms.RadioButton();
            this.radioButton_Skill_UserRange = new global::System.Windows.Forms.RadioButton();
            this.radioButton_Skill_StraightRange = new global::System.Windows.Forms.RadioButton();
            this.radioButton_Skill_CustomRange = new global::System.Windows.Forms.RadioButton();
            this.panel_SkillSeparator = new global::System.Windows.Forms.Panel();
            this.checkBox_Skill_RangeExtension = new global::System.Windows.Forms.CheckBox();
            this.checkBox_Skill_RangeExtensionItem = new global::System.Windows.Forms.CheckBox();
            this.label_Skill_Range = new global::System.Windows.Forms.Label();
            this.textBox_Skill_Range = new global::System.Windows.Forms.TextBox();
            this.label_Skill_RangeOpacity = new global::System.Windows.Forms.Label();
            this.Skill_Range_Opacity = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Skill_Range_Opacity)).BeginInit();

            this.button_ChangeCursor = new global::System.Windows.Forms.Button();
            this.button_Apply = new global::System.Windows.Forms.Button();
            this.button_MinimizeTray = new global::System.Windows.Forms.Button();
            this.notifyIcon1 = new global::System.Windows.Forms.NotifyIcon();
            this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip();
            this.toolStripMenuItemOpen = new global::System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new global::System.Windows.Forms.ToolStripMenuItem();

            this.groupBox_SpaceToEnter = new global::System.Windows.Forms.GroupBox();
            this.checkBox_SpaceToEnter_Enable = new global::System.Windows.Forms.CheckBox();
            this.textBox_SpaceToEnter_Duration = new global::System.Windows.Forms.TextBox();
            this.label_SpaceToEnter_Duration = new global::System.Windows.Forms.Label();
            #endregion

            #region [2-2. 레이아웃 일괄 업데이트 일시 정지 (SuspendLayout)]
            this.groupBox_MouseHighlight.SuspendLayout();
            this.groupBox_CustomCursor.SuspendLayout();
            this.groupBox_Skill.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox_SpaceToEnter.SuspendLayout();
            #endregion

            #region [2-3. 커서 설정 그룹 UI 배치 및 속성 설정 (groupBox_MouseHighlight)]
            this.groupBox_MouseHighlight.Controls.Add(this.label1);
            this.groupBox_MouseHighlight.Controls.Add(this.textBox_Mouse_Size);
            this.groupBox_MouseHighlight.Controls.Add(this.label2);
            this.groupBox_MouseHighlight.Controls.Add(this.Mouse_Opacity);
            this.groupBox_MouseHighlight.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.groupBox_MouseHighlight.Location = new global::System.Drawing.Point(15, 12);
            this.groupBox_MouseHighlight.Name = "groupBox_MouseHighlight";
            this.groupBox_MouseHighlight.Size = new global::System.Drawing.Size(330, 115);
            this.groupBox_MouseHighlight.TabIndex = 0;
            this.groupBox_MouseHighlight.TabStop = false;
            this.groupBox_MouseHighlight.Text = "커서 설정";
            this.groupBox_MouseHighlight.UseCompatibleTextRendering = true;

            // 마우스 크기 입력 텍스트박스 설정
            this.textBox_Mouse_Size.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.textBox_Mouse_Size.ImeMode = global::System.Windows.Forms.ImeMode.Disable;
            this.textBox_Mouse_Size.Location = new global::System.Drawing.Point(15, 32);
            this.textBox_Mouse_Size.MaxLength = 3;
            this.textBox_Mouse_Size.Name = "textBox_Mouse_Size";
            this.textBox_Mouse_Size.Size = new global::System.Drawing.Size(130, 27);
            this.textBox_Mouse_Size.TabIndex = 0;
            this.textBox_Mouse_Size.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);

            // 마우스 크기 안내 라벨 설정
            this.label1.AutoSize = true;
            this.label1.Font = new global::System.Drawing.Font("Segoe UI", 10f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label1.Location = new global::System.Drawing.Point(155, 34);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(150, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "커서 크기 (기본 40)";
            this.label1.UseCompatibleTextRendering = true;

            // 투명도 조절 트랙바 설정 (그룹박스 내부 좌표 기준)
            this.Mouse_Opacity.Location = new global::System.Drawing.Point(15, 68);
            this.Mouse_Opacity.Name = "Mouse_Opacity";
            this.Mouse_Opacity.Size = new global::System.Drawing.Size(130, 45);
            this.Mouse_Opacity.Maximum = 250;
            this.Mouse_Opacity.TickFrequency = 50;
            this.Mouse_Opacity.Scroll += new System.EventHandler(this.Mouse_Opacity_Scroll);

            // 투명도 안내 라벨 설정
            this.label2.AutoSize = true;
            this.label2.Font = new global::System.Drawing.Font("Segoe UI", 10f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label2.Location = new global::System.Drawing.Point(155, 74);
            this.label2.Name = "label2";
            this.label2.Size = new global::System.Drawing.Size(150, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "커서 투명도";
            this.label2.UseCompatibleTextRendering = true;
            #endregion

            #region [2-4. 커서 프리셋 및 Base64 입력 그룹 UI 배치 및 속성 설정 (groupBox_CustomCursor)]
            this.groupBox_CustomCursor.Controls.Add(this.label_CursorDesc);
            this.groupBox_CustomCursor.Controls.Add(label);
            this.groupBox_CustomCursor.Controls.Add(this.comboBox_CursorPreset);
            this.groupBox_CustomCursor.Controls.Add(this.linkLabel_CursorUrl);
            this.groupBox_CustomCursor.Controls.Add(this.textBox_CursorBase64);
            this.groupBox_CustomCursor.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.groupBox_CustomCursor.Location = new global::System.Drawing.Point(15, 137);
            this.groupBox_CustomCursor.Name = "groupBox_CustomCursor";
            this.groupBox_CustomCursor.Size = new global::System.Drawing.Size(330, 195);
            this.groupBox_CustomCursor.TabIndex = 1;
            this.groupBox_CustomCursor.TabStop = false;
            this.groupBox_CustomCursor.Text = "커서 선택";
            this.groupBox_CustomCursor.UseCompatibleTextRendering = true;

            // 커서 설명 라벨 설정
            this.label_CursorDesc.AutoSize = true;
            this.label_CursorDesc.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label_CursorDesc.Location = new global::System.Drawing.Point(15, 25);
            this.label_CursorDesc.Name = "label_CursorDesc";
            this.label_CursorDesc.Size = new global::System.Drawing.Size(260, 20);
            this.label_CursorDesc.TabIndex = 0;
            this.label_CursorDesc.Text = "커서 프리셋 선택 또는 Base64 직접 입력:";

            // 커서 프리셋 콤보박스 설정
            this.comboBox_CursorPreset.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CursorPreset.Font = new global::System.Drawing.Font("Segoe UI", 9.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.comboBox_CursorPreset.Items.AddRange(new object[]
            {
                "기본 커서 (Default)",
                "옐로 커서 (Yellow)",
                "레드 커서 (Red)",
                "직접 입력 (Custom)"
            });
            this.comboBox_CursorPreset.Location = new global::System.Drawing.Point(15, 50);
            this.comboBox_CursorPreset.Name = "comboBox_CursorPreset";
            this.comboBox_CursorPreset.Size = new global::System.Drawing.Size(300, 25);
            this.comboBox_CursorPreset.TabIndex = 1;
            this.comboBox_CursorPreset.SelectedIndexChanged += new System.EventHandler(this.comboBox_CursorPreset_SelectedIndexChanged);

            // Base64 확인 노트 라벨 설정
            label.AutoSize = true;
            label.Font = new global::System.Drawing.Font("Segoe UI", 8f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
            label.Location = new global::System.Drawing.Point(15, 83);
            label.Name = "label_Base64CheckNote";
            label.Size = new global::System.Drawing.Size(200, 18);
            label.TabIndex = 2;
            label.Text = "※ Base64 문자열 ※";

            // Base64 변환 링크 라벨 설정
            this.linkLabel_CursorUrl.AutoSize = true;
            this.linkLabel_CursorUrl.Font = new global::System.Drawing.Font("Segoe UI", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.linkLabel_CursorUrl.Location = new global::System.Drawing.Point(15, 101);
            this.linkLabel_CursorUrl.Name = "linkLabel_CursorUrl";
            this.linkLabel_CursorUrl.Size = new global::System.Drawing.Size(220, 18);
            this.linkLabel_CursorUrl.TabIndex = 3;
            this.linkLabel_CursorUrl.TabStop = true;
            this.linkLabel_CursorUrl.Text = "https://www.base64.sh/ko/image-to-base64/";
            this.linkLabel_CursorUrl.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_CursorUrl_LinkClicked);

            // Base64 입력 텍스트박스 설정
            this.textBox_CursorBase64.Font = new global::System.Drawing.Font("Segoe UI", 8.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.textBox_CursorBase64.Location = new global::System.Drawing.Point(15, 123);
            this.textBox_CursorBase64.Multiline = true;
            this.textBox_CursorBase64.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_CursorBase64.MaxLength = 0;
            this.textBox_CursorBase64.Name = "textBox_CursorBase64";
            this.textBox_CursorBase64.Size = new global::System.Drawing.Size(300, 55);
            this.textBox_CursorBase64.TabIndex = 4;
            #endregion

            #region [2-5. 센터 스킬 영역 오버레이 그룹 UI 배치 및 속성 설정 (groupBox_Skill)]
            this.groupBox_Skill.Controls.Add(this.radioButton_Skill_None);
            this.groupBox_Skill.Controls.Add(this.radioButton_Skill_UserRange);
            this.groupBox_Skill.Controls.Add(this.radioButton_Skill_StraightRange);
            this.groupBox_Skill.Controls.Add(this.radioButton_Skill_CustomRange);
            this.groupBox_Skill.Controls.Add(this.panel_SkillSeparator);
            this.groupBox_Skill.Controls.Add(this.checkBox_Skill_RangeExtension);
            this.groupBox_Skill.Controls.Add(this.checkBox_Skill_RangeExtensionItem);
            this.groupBox_Skill.Controls.Add(this.label_Skill_Range);
            this.groupBox_Skill.Controls.Add(this.textBox_Skill_Range);
            this.groupBox_Skill.Controls.Add(this.label_Skill_RangeOpacity);
            this.groupBox_Skill.Controls.Add(this.Skill_Range_Opacity);
            this.groupBox_Skill.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.groupBox_Skill.Location = new global::System.Drawing.Point(15, 339);
            this.groupBox_Skill.Name = "groupBox_Skill";
            this.groupBox_Skill.Size = new global::System.Drawing.Size(330, 215);
            this.groupBox_Skill.TabIndex = 2;
            this.groupBox_Skill.TabStop = false;
            this.groupBox_Skill.Text = "스킬 범위 오버레이";
            this.groupBox_Skill.UseCompatibleTextRendering = true;

            // [사용안함] 라디오 버튼 설정 (TabIndex 0)
            this.radioButton_Skill_None.AutoSize = true;
            this.radioButton_Skill_None.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.radioButton_Skill_None.Location = new global::System.Drawing.Point(15, 28);
            this.radioButton_Skill_None.Name = "radioButton_Skill_None";
            this.radioButton_Skill_None.Size = new global::System.Drawing.Size(80, 20);
            this.radioButton_Skill_None.TabIndex = 0;
            this.radioButton_Skill_None.TabStop = true;
            this.radioButton_Skill_None.Text = "사용안함";
            this.radioButton_Skill_None.UseVisualStyleBackColor = true;
            this.radioButton_Skill_None.CheckedChanged += new System.EventHandler(this.radioButton_Skill_None_CheckedChanged);

            // [자기 중심 범위] 라디오 버튼 설정 (TabIndex 1)
            this.radioButton_Skill_UserRange.AutoSize = true;
            this.radioButton_Skill_UserRange.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.radioButton_Skill_UserRange.Location = new global::System.Drawing.Point(165, 28);
            this.radioButton_Skill_UserRange.Name = "radioButton_Skill_UserRange";
            this.radioButton_Skill_UserRange.Size = new global::System.Drawing.Size(80, 20);
            this.radioButton_Skill_UserRange.TabIndex = 1;
            this.radioButton_Skill_UserRange.TabStop = true;
            this.radioButton_Skill_UserRange.Text = "자기중심";
            this.radioButton_Skill_UserRange.UseVisualStyleBackColor = true;
            this.radioButton_Skill_UserRange.CheckedChanged += new System.EventHandler(this.radioButton_SkillEnable_UserRange_CheckedChanged);

            // [직선 범위] 라디오 버튼 설정 (TabIndex 2)
            this.radioButton_Skill_StraightRange.AutoSize = true;
            this.radioButton_Skill_StraightRange.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.radioButton_Skill_StraightRange.Location = new global::System.Drawing.Point(15, 58);
            this.radioButton_Skill_StraightRange.Name = "radioButton_Skill_StraightRange";
            this.radioButton_Skill_StraightRange.Size = new global::System.Drawing.Size(110, 20);
            this.radioButton_Skill_StraightRange.TabIndex = 2;
            this.radioButton_Skill_StraightRange.TabStop = true;
            this.radioButton_Skill_StraightRange.Text = "직선범위 (길이)";
            this.radioButton_Skill_StraightRange.UseVisualStyleBackColor = true;
            this.radioButton_Skill_StraightRange.CheckedChanged += new System.EventHandler(this.radioButton_Skill_StraightRange_CheckedChanged);

            // [지정 범위] 라디오 버튼 설정 (TabIndex 3)
            this.radioButton_Skill_CustomRange.AutoSize = true;
            this.radioButton_Skill_CustomRange.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.radioButton_Skill_CustomRange.Location = new global::System.Drawing.Point(165, 58);
            this.radioButton_Skill_CustomRange.Name = "radioButton_Skill_CustomRange";
            this.radioButton_Skill_CustomRange.Size = new global::System.Drawing.Size(80, 20);
            this.radioButton_Skill_CustomRange.TabIndex = 3;
            this.radioButton_Skill_CustomRange.TabStop = true;
            this.radioButton_Skill_CustomRange.Text = "지정범위";
            this.radioButton_Skill_CustomRange.UseVisualStyleBackColor = true;
            this.radioButton_Skill_CustomRange.CheckedChanged += new System.EventHandler(this.radioButton_Skill_CustomRange_CheckedChanged);

            // [구분선 추가]
            this.panel_SkillSeparator.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
            this.panel_SkillSeparator.BackColor = global::System.Drawing.Color.LightGray;
            this.panel_SkillSeparator.Location = new global::System.Drawing.Point(15, 86);
            this.panel_SkillSeparator.Name = "panel_SkillSeparator";
            this.panel_SkillSeparator.Size = new global::System.Drawing.Size(300, 1);
            this.panel_SkillSeparator.TabIndex = 4;
            this.panel_SkillSeparator.TabStop = false;

            // 범위 연마 체크박스 설정 (TabIndex 5)
            this.checkBox_Skill_RangeExtension.AutoSize = true;
            this.checkBox_Skill_RangeExtension.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.checkBox_Skill_RangeExtension.Location = new global::System.Drawing.Point(15, 98);
            this.checkBox_Skill_RangeExtension.Name = "checkBox_Skill_RangeExtension";
            this.checkBox_Skill_RangeExtension.Size = new global::System.Drawing.Size(82, 24);
            this.checkBox_Skill_RangeExtension.TabIndex = 5;
            this.checkBox_Skill_RangeExtension.Text = "범위 연마";
            this.checkBox_Skill_RangeExtension.UseVisualStyleBackColor = true;
            this.checkBox_Skill_RangeExtension.CheckedChanged += new System.EventHandler(this.checkBox_Skill_RangeExtension_CheckedChanged);

            // 불시엔 체크박스 설정 (TabIndex 6)
            this.checkBox_Skill_RangeExtensionItem.AutoSize = true;
            this.checkBox_Skill_RangeExtensionItem.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.checkBox_Skill_RangeExtensionItem.Location = new global::System.Drawing.Point(165, 98);
            this.checkBox_Skill_RangeExtensionItem.Name = "checkBox_Skill_RangeExtensionItem";
            this.checkBox_Skill_RangeExtensionItem.Size = new global::System.Drawing.Size(65, 24);
            this.checkBox_Skill_RangeExtensionItem.TabIndex = 6;
            this.checkBox_Skill_RangeExtensionItem.Text = "불시엔";
            this.checkBox_Skill_RangeExtensionItem.UseVisualStyleBackColor = true;
            this.checkBox_Skill_RangeExtensionItem.CheckedChanged += new System.EventHandler(this.checkBox_Skill_RangeExtensionItem_CheckedChanged);

            // 스킬 범위값 입력 텍스트박스 설정 (TabIndex 7)
            this.textBox_Skill_Range.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.textBox_Skill_Range.ImeMode = global::System.Windows.Forms.ImeMode.Disable;
            this.textBox_Skill_Range.Location = new global::System.Drawing.Point(15, 130);
            this.textBox_Skill_Range.MaxLength = 2;
            this.textBox_Skill_Range.Name = "textBox_Skill_Range";
            this.textBox_Skill_Range.Size = new global::System.Drawing.Size(130, 27);
            this.textBox_Skill_Range.TabIndex = 7;
            this.textBox_Skill_Range.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);

            // 스킬 범위값 안내 라벨 설정
            this.label_Skill_Range.AutoSize = true;
            this.label_Skill_Range.Font = new global::System.Drawing.Font("Segoe UI", 9.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label_Skill_Range.Location = new global::System.Drawing.Point(155, 134);
            this.label_Skill_Range.Name = "label_Skill_Range";
            this.label_Skill_Range.Size = new global::System.Drawing.Size(150, 20);
            this.label_Skill_Range.Text = "스킬 범위값 (예: 30~60)";

            // 스킬 범위 투명도 조절 트랙바 설정
            this.Skill_Range_Opacity.Location = new global::System.Drawing.Point(15, 166);
            this.Skill_Range_Opacity.Name = "Skill_Range_Opacity";
            this.Skill_Range_Opacity.Size = new global::System.Drawing.Size(130, 45);
            this.Skill_Range_Opacity.Maximum = 200;
            this.Skill_Range_Opacity.TickFrequency = 50;
            this.Skill_Range_Opacity.Scroll += new System.EventHandler(this.Skill_RangeOpacity_Scroll);

            // 스킬 범위 투명도 안내 라벨 설정 (TabIndex 8)
            this.label_Skill_RangeOpacity.AutoSize = true;
            this.label_Skill_RangeOpacity.Font = new global::System.Drawing.Font("Segoe UI", 10f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label_Skill_RangeOpacity.Location = new global::System.Drawing.Point(155, 171);
            this.label_Skill_RangeOpacity.Name = "label_Skill_RangeOpacity";
            this.label_Skill_RangeOpacity.Size = new global::System.Drawing.Size(150, 25);
            this.label_Skill_RangeOpacity.TabIndex = 8;
            this.label_Skill_RangeOpacity.Text = "스킬 범위 투명도";
            this.label_Skill_RangeOpacity.UseCompatibleTextRendering = true;

            // [1~5 키 입력 시 Space -> Enter 치환]
            this.groupBox_SpaceToEnter.Controls.Add(this.checkBox_SpaceToEnter_Enable);
            this.groupBox_SpaceToEnter.Controls.Add(this.textBox_SpaceToEnter_Duration);
            this.groupBox_SpaceToEnter.Controls.Add(this.label_SpaceToEnter_Duration);
            this.groupBox_SpaceToEnter.Font = new global::System.Drawing.Font("Segoe UI", 11f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.groupBox_SpaceToEnter.Location = new global::System.Drawing.Point(15, 564);
            this.groupBox_SpaceToEnter.Name = "groupBox_SpaceToEnter";
            this.groupBox_SpaceToEnter.Size = new global::System.Drawing.Size(330, 88);
            this.groupBox_SpaceToEnter.TabIndex = 3;
            this.groupBox_SpaceToEnter.TabStop = false;
            this.groupBox_SpaceToEnter.Text = "Space → Enter 치환 (1~5 키)";
            this.groupBox_SpaceToEnter.UseCompatibleTextRendering = true;

            // ON/OFF 체크박스 설정
            this.checkBox_SpaceToEnter_Enable.AutoSize = true;
            this.checkBox_SpaceToEnter_Enable.Font = new global::System.Drawing.Font("Segoe UI", 9.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.checkBox_SpaceToEnter_Enable.Location = new global::System.Drawing.Point(15, 25);
            this.checkBox_SpaceToEnter_Enable.Name = "checkBox_SpaceToEnter_Enable";
            this.checkBox_SpaceToEnter_Enable.Size = new global::System.Drawing.Size(130, 24);
            this.checkBox_SpaceToEnter_Enable.TabIndex = 0;
            this.checkBox_SpaceToEnter_Enable.Text = "치환 기능 사용";
            this.checkBox_SpaceToEnter_Enable.UseVisualStyleBackColor = true;
            this.checkBox_SpaceToEnter_Enable.CheckedChanged += new System.EventHandler(this.checkBox_SpaceToEnter_Enable_CheckedChanged);

            // 지속 시간 입력 텍스트박스 설정 (ms 단위)
            this.textBox_SpaceToEnter_Duration.Font = new global::System.Drawing.Font("Segoe UI", 10.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.textBox_SpaceToEnter_Duration.ImeMode = global::System.Windows.Forms.ImeMode.Disable;
            this.textBox_SpaceToEnter_Duration.Location = new global::System.Drawing.Point(15, 52);
            this.textBox_SpaceToEnter_Duration.MaxLength = 5;
            this.textBox_SpaceToEnter_Duration.Name = "textBox_SpaceToEnter_Duration";
            this.textBox_SpaceToEnter_Duration.Size = new global::System.Drawing.Size(130, 26);
            this.textBox_SpaceToEnter_Duration.TabIndex = 1;
            this.textBox_SpaceToEnter_Duration.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);

            // 지속 시간 안내 라벨 설정
            this.label_SpaceToEnter_Duration.AutoSize = true;
            this.label_SpaceToEnter_Duration.Font = new global::System.Drawing.Font("Segoe UI", 9.5f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            this.label_SpaceToEnter_Duration.Location = new global::System.Drawing.Point(155, 55);
            this.label_SpaceToEnter_Duration.Name = "label_SpaceToEnter_Duration";
            this.label_SpaceToEnter_Duration.Size = new global::System.Drawing.Size(155, 20);
            this.label_SpaceToEnter_Duration.TabIndex = 2;
            this.label_SpaceToEnter_Duration.Text = "유지 시간 ms (기본 200)";
            this.label_SpaceToEnter_Duration.UseCompatibleTextRendering = true;
            #endregion

            #region [2-6. 하단 메인 제어 버튼 UI 배치 및 속성 설정]
            // 커서 변경 버튼 설정
            this.button_ChangeCursor.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
            this.button_ChangeCursor.Location = new global::System.Drawing.Point(15, 665);
            this.button_ChangeCursor.Name = "button_ChangeCursor";
            this.button_ChangeCursor.Size = new global::System.Drawing.Size(103, 35);
            this.button_ChangeCursor.TabIndex = 4;
            this.button_ChangeCursor.Text = "커서 변경";
            this.button_ChangeCursor.UseCompatibleTextRendering = true;
            this.button_ChangeCursor.UseVisualStyleBackColor = true;
            this.button_ChangeCursor.Click += new System.EventHandler(this.button_ChangeCursor_Click);

            // 설정값 저장 버튼 설정
            this.button_Apply.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
            this.button_Apply.Location = new global::System.Drawing.Point(123, 665);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new global::System.Drawing.Size(103, 35);
            this.button_Apply.TabIndex = 5;
            this.button_Apply.Text = "설정값 저장";
            this.button_Apply.UseCompatibleTextRendering = true;
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);

            // 트레이로 최소화 버튼 설정
            this.button_MinimizeTray.Font = new global::System.Drawing.Font("Segoe UI", 8.5f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
            this.button_MinimizeTray.Location = new global::System.Drawing.Point(231, 665);
            this.button_MinimizeTray.Name = "button_MinimizeTray";
            this.button_MinimizeTray.Size = new global::System.Drawing.Size(114, 35);
            this.button_MinimizeTray.TabIndex = 6;
            this.button_MinimizeTray.Text = "트레이로 최소화";
            this.button_MinimizeTray.UseCompatibleTextRendering = true;
            this.button_MinimizeTray.UseVisualStyleBackColor = true;
            this.button_MinimizeTray.Click += new System.EventHandler(this.button_MinimizeTray_Click);
            base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.button_MinimizeTray_KeyDown);
            #endregion

            #region [2-7. 시스템 트레이 아이콘 및 우클릭 컨텍스트 메뉴 설정]
            // 컨텍스트 메뉴 설정
            this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
            {
                this.toolStripMenuItemOpen,
                this.toolStripMenuItemExit
            });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new global::System.Drawing.Size(100, 48);

            // [열기] 메뉴 아이템 설정
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new global::System.Drawing.Size(99, 22);
            this.toolStripMenuItemOpen.Text = "열기";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.TrayMenu_Open_Click);

            // [종료] 메뉴 아이템 설정
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new global::System.Drawing.Size(99, 22);
            this.toolStripMenuItemExit.Text = "종료";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.TrayMenu_Exit_Click);

            // 시스템 트레이 아이콘 설정
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
            this.notifyIcon1.Text = "TalesWeaver Cursor Assistance";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            #endregion

            #region [2-8. 메인 폼(Form1) 창 전체 속성 및 컨트롤 등록]
            base.AutoScaleDimensions = new global::System.Drawing.SizeF(96f, 96f);
            base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            base.ClientSize = new global::System.Drawing.Size(360, 716);
            base.KeyPreview = true;

            // 폼 컨트롤 컬렉션 등록
            base.Controls.Add(this.button_MinimizeTray);
            base.Controls.Add(this.button_Apply);
            base.Controls.Add(this.button_ChangeCursor);
            base.Controls.Add(this.groupBox_Skill);
            base.Controls.Add(this.groupBox_CustomCursor);
            base.Controls.Add(this.groupBox_MouseHighlight);
            base.Controls.Add(this.groupBox_SpaceToEnter);

            this.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
            base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form1";
            this.Text = "TalesWeaver Cursor Assistance";

            // 폼 주요 이벤트 핸들러 바인딩
            base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            base.Load += new System.EventHandler(this.Form1_Load);
            base.Shown += new System.EventHandler(this.Form1_Shown);
            #endregion

            #region [2-9. 레이아웃 일괄 업데이트 재개 (ResumeLayout)]
            this.groupBox_MouseHighlight.ResumeLayout(false);
            this.groupBox_MouseHighlight.PerformLayout();
            this.groupBox_CustomCursor.ResumeLayout(false);
            this.groupBox_CustomCursor.PerformLayout();
            this.groupBox_Skill.ResumeLayout(false);
            this.groupBox_Skill.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox_SpaceToEnter.ResumeLayout(false);
            this.groupBox_SpaceToEnter.PerformLayout();
            base.ResumeLayout(false);
            #endregion

            #region [2-10. 툴팁(ToolTip) 안내 메시지 설정]
            this.toolTip1.SetToolTip(this.Mouse_Opacity, "마우스 커서의 투명도를 슬라이더로 조절합니다 (0~255)");
            this.toolTip1.SetToolTip(this.Skill_Range_Opacity, "스킬 범위 오버레이의 투명도를 슬라이더로 조절합니다 (0~255)");
            this.toolTip1.SetToolTip(this.comboBox_CursorPreset, "원하는 커서 디자인 프리셋을 선택하거나 직접 입력을 선택하세요");
            this.toolTip1.SetToolTip(this.textBox_CursorBase64, "커서 이미지의 Base64 문자열 코드를 입력합니다");
            this.toolTip1.SetToolTip(this.radioButton_Skill_UserRange, "화면에 자기 중심 범위 오버레이를 표시합니다");
            this.toolTip1.SetToolTip(this.radioButton_Skill_StraightRange, "화면에 직선 범위 오버레이를 표시합니다");
            this.toolTip1.SetToolTip(this.radioButton_Skill_CustomRange, "화면에 지정 범위 오버레이를 표시합니다");
            this.toolTip1.SetToolTip(this.checkBox_Skill_RangeExtension, "스킬 범위 연마 효과를 적용하여 범위를 넓힙니다 (+20)");
            this.toolTip1.SetToolTip(this.checkBox_Skill_RangeExtensionItem, "불시엔 효과를 적용하여 범위를 넓힙니다 (+7)");
            this.toolTip1.SetToolTip(this.textBox_Skill_Range, "자기 중심 범위 : 기본 44 / 예프넨 48\n직선 범위 : 기본 40 ' 30\n지정 범위 : 기본 32 / 예프넨 36");
            this.toolTip1.SetToolTip(this.button_ChangeCursor, "선택/입력한 Base64 코드로 커서를 변경합니다");
            this.toolTip1.SetToolTip(this.button_Apply, "현재 변경된 모든 설정값을 설정 파일에 저장합니다");
            this.toolTip1.SetToolTip(this.button_MinimizeTray, "[ESC] 프로그램을 작업 표시줄에서 숨기고 시스템 트레이로 내립니다");
            this.toolTip1.SetToolTip(this.checkBox_SpaceToEnter_Enable, "1~5 키 입력 후 지정된 시간(ms) 동안 Space 키를 Enter 키로 변환합니다");
            this.toolTip1.SetToolTip(this.textBox_SpaceToEnter_Duration, "Space → Enter 변환이 유지되는 시간(ms, 1000 = 1초)을 입력합니다");
            #endregion
        }
        #endregion

        #region [3. 필드 및 컨트롤 변수 선언부 (Fields Declaration)]

        #region [3-1. 시스템 컴포넌트 및 툴팁 관리 필드]
        /// <summary>컨트롤 및 자원 관리를 위한 컴포넌트 컨테이너</summary>
        private global::System.ComponentModel.IContainer components;
        /// <summary>각 UI 컨트롤에 대한 도움말 툴팁 컴포넌트</summary>
        private global::System.Windows.Forms.ToolTip toolTip1;
        #endregion

        #region [3-2. 커서 크기 및 투명도 설정 그룹 필드]
        /// <summary>커서 설정 그룹박스</summary>
        private global::System.Windows.Forms.GroupBox groupBox_MouseHighlight;
        /// <summary>투명도 입력 트랙바 라벨</summary>
        private global::System.Windows.Forms.Label label2;
        /// <summary>마우스 크기 입력 텍스트박스 라벨</summary>
        private global::System.Windows.Forms.Label label1;
        /// <summary>커서 투명도 조절 트랙바</summary>
        private global::System.Windows.Forms.TrackBar Mouse_Opacity;
        /// <summary>마우스 커서 크기 입력 텍스트박스</summary>
        private global::System.Windows.Forms.TextBox textBox_Mouse_Size;
        #endregion

        #region [3-3. 하단 제어 버튼 및 시스템 트레이/메뉴 관련 필드]
        /// <summary>설정값 저장 버튼</summary>
        private global::System.Windows.Forms.Button button_Apply;
        /// <summary>트레이로 최소화 버튼</summary>
        private global::System.Windows.Forms.Button button_MinimizeTray;
        /// <summary>시스템 트레이 아이콘 컨트롤</summary>
        private global::System.Windows.Forms.NotifyIcon notifyIcon1;
        /// <summary>트레이 아이콘 우클릭 컨텍스트 메뉴 스트립</summary>
        private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        /// <summary>트레이 메뉴 [열기] 항목</summary>
        private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
        /// <summary>트레이 메뉴 [종료] 항목</summary>
        private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        #endregion

        #region [3-4. 커서 프리셋 및 Base64 입력 관련 필드]
        /// <summary>커서 프리셋 및 Base64 설정 그룹박스</summary>
        private global::System.Windows.Forms.GroupBox groupBox_CustomCursor;
        /// <summary>커서 Base64 코드 입력 텍스트박스</summary>
        private global::System.Windows.Forms.TextBox textBox_CursorBase64;
        /// <summary>커서 변경 적용 버튼</summary>
        private global::System.Windows.Forms.Button button_ChangeCursor;
        /// <summary>커서 설명 안내 라벨</summary>
        private global::System.Windows.Forms.Label label_CursorDesc;
        /// <summary>Base64 변환 링크 라벨</summary>
        private global::System.Windows.Forms.LinkLabel linkLabel_CursorUrl;
        /// <summary>커서 프리셋 선택 콤보박스</summary>
        private global::System.Windows.Forms.ComboBox comboBox_CursorPreset;
        #endregion

        #region [3-5. 센터 스킬 영역 오버레이 관련 필드]
        /// <summary>센터 스킬 영역 오버레이 설정 그룹박스</summary>
        private global::System.Windows.Forms.GroupBox groupBox_Skill;
        /// <summary>사용안함 라디오 버튼</summary>
        private global::System.Windows.Forms.RadioButton radioButton_Skill_None;
        /// <summary>자기 중심 범위 라디오 버튼</summary>
        private global::System.Windows.Forms.RadioButton radioButton_Skill_UserRange;
        /// <summary>직선 범위 라디오 버튼</summary>
        private global::System.Windows.Forms.RadioButton radioButton_Skill_StraightRange;
        /// <summary>지정 범위 라디오 버튼</summary>
        private global::System.Windows.Forms.RadioButton radioButton_Skill_CustomRange;
        /// <summary>스킬 범위값 안내 라벨</summary>
        private global::System.Windows.Forms.Label label_Skill_Range;
        /// <summary>스킬 범위값 입력 텍스트박스</summary>
        private global::System.Windows.Forms.TextBox textBox_Skill_Range;
        /// <summary>범위 연마 옵션 체크박스</summary>
        private global::System.Windows.Forms.CheckBox checkBox_Skill_RangeExtension;
        /// <summary>불시엔 옵션 체크박스</summary>
        private global::System.Windows.Forms.CheckBox checkBox_Skill_RangeExtensionItem;
        /// <summary>스킬 범위 투명도 조절 트랙바</summary>
        private System.Windows.Forms.TrackBar Skill_Range_Opacity;
        /// <summary>스킬 범위 투명도 안내 라벨</summary>
        private global::System.Windows.Forms.Label label_Skill_RangeOpacity;
        /// <summary>구분선 패널</summary>
        private global::System.Windows.Forms.Panel panel_SkillSeparator;
        #endregion

        #region [3-6. Space -> Enter 치환 설정 관련 필드]
        private global::System.Windows.Forms.GroupBox groupBox_SpaceToEnter;
        private global::System.Windows.Forms.CheckBox checkBox_SpaceToEnter_Enable;
        private global::System.Windows.Forms.TextBox textBox_SpaceToEnter_Duration;
        private global::System.Windows.Forms.Label label_SpaceToEnter_Duration;
        #endregion

        #endregion
    }
}