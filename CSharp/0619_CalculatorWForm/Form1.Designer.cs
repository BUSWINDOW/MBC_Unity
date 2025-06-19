namespace _0619_CalculatorWForm
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.num1_lb = new System.Windows.Forms.Label();
            this.num2_lb = new System.Windows.Forms.Label();
            this.num1_txtBox = new System.Windows.Forms.TextBox();
            this.num2_txtBox = new System.Windows.Forms.TextBox();
            this.Line = new System.Windows.Forms.Label();
            this.Answer_lb = new System.Windows.Forms.Label();
            this.Answer_txtBox = new System.Windows.Forms.TextBox();
            this.Plus_Btn = new System.Windows.Forms.Button();
            this.Minus_Btn = new System.Windows.Forms.Button();
            this.Multi_Btn = new System.Windows.Forms.Button();
            this.Divide_Btn = new System.Windows.Forms.Button();
            this.Clear_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // num1_lb
            // 
            this.num1_lb.AutoSize = true;
            this.num1_lb.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num1_lb.Location = new System.Drawing.Point(39, 62);
            this.num1_lb.Name = "num1_lb";
            this.num1_lb.Size = new System.Drawing.Size(171, 48);
            this.num1_lb.TabIndex = 0;
            this.num1_lb.Text = "Num 1";
            // 
            // num2_lb
            // 
            this.num2_lb.AutoSize = true;
            this.num2_lb.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num2_lb.Location = new System.Drawing.Point(39, 148);
            this.num2_lb.Name = "num2_lb";
            this.num2_lb.Size = new System.Drawing.Size(171, 48);
            this.num2_lb.TabIndex = 1;
            this.num2_lb.Text = "Num 2";
            // 
            // num1_txtBox
            // 
            this.num1_txtBox.Font = new System.Drawing.Font("굴림", 36F);
            this.num1_txtBox.Location = new System.Drawing.Point(259, 62);
            this.num1_txtBox.Name = "num1_txtBox";
            this.num1_txtBox.Size = new System.Drawing.Size(466, 63);
            this.num1_txtBox.TabIndex = 2;
            //this.num1_txtBox.TextChanged += new System.EventHandler(this.num1_txtBox_TextChanged);
            // 
            // num2_txtBox
            // 
            this.num2_txtBox.Font = new System.Drawing.Font("굴림", 36F);
            this.num2_txtBox.Location = new System.Drawing.Point(259, 148);
            this.num2_txtBox.Name = "num2_txtBox";
            this.num2_txtBox.Size = new System.Drawing.Size(466, 63);
            this.num2_txtBox.TabIndex = 3;
            //this.num2_txtBox.TextChanged += new System.EventHandler(this.num2_txtBox_TextChanged);
            // 
            // Line
            // 
            this.Line.AutoSize = true;
            this.Line.Location = new System.Drawing.Point(45, 264);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(683, 12);
            this.Line.TabIndex = 4;
            this.Line.Text = "---------------------------------------------------------------------------------" +
    "--------------------------------";
            // 
            // Answer_lb
            // 
            this.Answer_lb.AutoSize = true;
            this.Answer_lb.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Answer_lb.Location = new System.Drawing.Point(39, 294);
            this.Answer_lb.Name = "Answer_lb";
            this.Answer_lb.Size = new System.Drawing.Size(189, 48);
            this.Answer_lb.TabIndex = 5;
            this.Answer_lb.Text = "Answer";
            // 
            // Answer_txtBox
            // 
            this.Answer_txtBox.Font = new System.Drawing.Font("굴림", 36F);
            this.Answer_txtBox.Location = new System.Drawing.Point(259, 291);
            this.Answer_txtBox.Name = "Answer_txtBox";
            this.Answer_txtBox.Size = new System.Drawing.Size(466, 63);
            this.Answer_txtBox.TabIndex = 6;

            // 
            // Plus_Btn
            // 
            this.Plus_Btn.Font = new System.Drawing.Font("굴림", 36F);
            this.Plus_Btn.Location = new System.Drawing.Point(84, 366);
            this.Plus_Btn.Name = "Plus_Btn";
            this.Plus_Btn.Size = new System.Drawing.Size(70, 72);
            this.Plus_Btn.TabIndex = 7;
            this.Plus_Btn.Text = "+";
            this.Plus_Btn.UseVisualStyleBackColor = true;
            this.Plus_Btn.Click += new System.EventHandler(this.Plus_Btn_Click);
            // 
            // Minus_Btn
            // 
            this.Minus_Btn.Font = new System.Drawing.Font("굴림", 36F);
            this.Minus_Btn.Location = new System.Drawing.Point(186, 366);
            this.Minus_Btn.Name = "Minus_Btn";
            this.Minus_Btn.Size = new System.Drawing.Size(70, 72);
            this.Minus_Btn.TabIndex = 8;
            this.Minus_Btn.Text = "-";
            this.Minus_Btn.UseVisualStyleBackColor = true;
            this.Minus_Btn.Click += new System.EventHandler(this.Minus_Btn_Click);
            // 
            // Multi_Btn
            // 
            this.Multi_Btn.Font = new System.Drawing.Font("굴림", 36F);
            this.Multi_Btn.Location = new System.Drawing.Point(286, 366);
            this.Multi_Btn.Name = "Multi_Btn";
            this.Multi_Btn.Size = new System.Drawing.Size(70, 72);
            this.Multi_Btn.TabIndex = 9;
            this.Multi_Btn.Text = "×";
            this.Multi_Btn.UseVisualStyleBackColor = true;
            this.Multi_Btn.Click += new System.EventHandler(this.Multi_Btn_Click);
            // 
            // Divide_Btn
            // 
            this.Divide_Btn.Font = new System.Drawing.Font("굴림", 36F);
            this.Divide_Btn.Location = new System.Drawing.Point(379, 366);
            this.Divide_Btn.Name = "Divide_Btn";
            this.Divide_Btn.Size = new System.Drawing.Size(70, 72);
            this.Divide_Btn.TabIndex = 10;
            this.Divide_Btn.Text = "÷";
            this.Divide_Btn.UseVisualStyleBackColor = true;
            this.Divide_Btn.Click += new System.EventHandler(this.Divide_Btn_Click);
            // 
            // Clear_Btn
            // 
            this.Clear_Btn.Font = new System.Drawing.Font("굴림", 36F);
            this.Clear_Btn.Location = new System.Drawing.Point(525, 366);
            this.Clear_Btn.Name = "Clear_Btn";
            this.Clear_Btn.Size = new System.Drawing.Size(155, 72);
            this.Clear_Btn.TabIndex = 11;
            this.Clear_Btn.Text = "Clear";
            this.Clear_Btn.UseVisualStyleBackColor = true;
            this.Clear_Btn.Click += new System.EventHandler(this.Clear_Btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Clear_Btn);
            this.Controls.Add(this.Divide_Btn);
            this.Controls.Add(this.Multi_Btn);
            this.Controls.Add(this.Minus_Btn);
            this.Controls.Add(this.Plus_Btn);
            this.Controls.Add(this.Answer_txtBox);
            this.Controls.Add(this.Answer_lb);
            this.Controls.Add(this.Line);
            this.Controls.Add(this.num2_txtBox);
            this.Controls.Add(this.num1_txtBox);
            this.Controls.Add(this.num2_lb);
            this.Controls.Add(this.num1_lb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label num1_lb;
        private System.Windows.Forms.Label num2_lb;
        private System.Windows.Forms.TextBox num1_txtBox;
        private System.Windows.Forms.TextBox num2_txtBox;
        private System.Windows.Forms.Label Line;
        private System.Windows.Forms.Label Answer_lb;
        private System.Windows.Forms.TextBox Answer_txtBox;
        private System.Windows.Forms.Button Plus_Btn;
        private System.Windows.Forms.Button Minus_Btn;
        private System.Windows.Forms.Button Multi_Btn;
        private System.Windows.Forms.Button Divide_Btn;
        private System.Windows.Forms.Button Clear_Btn;
    }
}

