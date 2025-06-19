using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0619_CalculatorWForm
{
    enum eCalculator
    {
        Plus = 0, Minus = 1, Multi = 2, Divide = 3
    }
    public partial class Form1 : Form
    {

        private double num1;
        private double num2;
        private double answer;
        public Form1()
        {
            InitializeComponent();
            Clear();
        }

        private void Clear()
        {
            this.Answer_txtBox.Text = "";
            this.num2_txtBox.Text = "";
            this.num1_txtBox.Text = "";
        }


        /*private void num1_txtBox_TextChanged(object sender, EventArgs e)
        {
            this.num1 =  int.Parse(this.num1_txtBox.Text);
        }

        private void num2_txtBox_TextChanged(object sender, EventArgs e)
        {
            
        }*/

        // TextChanged = 안에 Text가 변할때 호출
       // int.Parse에 공백 들어가고 하니 에러
       
        private void Plus_Btn_Click(object sender, EventArgs e)
        {
            this.Answer_txtBox.Text = this.GetAnswer(eCalculator.Plus);
        }

        private void Minus_Btn_Click(object sender, EventArgs e)
        {
            this.Answer_txtBox.Text = this.GetAnswer(eCalculator.Minus);

        }

        private void Multi_Btn_Click(object sender, EventArgs e)
        {
            this.Answer_txtBox.Text = this.GetAnswer(eCalculator.Multi);

        }

        private void Divide_Btn_Click(object sender, EventArgs e)
        {
            this.Answer_txtBox.Text = this.GetAnswer(eCalculator.Divide);

        }

        private string GetAnswer(eCalculator cal)
        {
            try
            {
                this.num1 = double.Parse(this.num1_txtBox.Text);
                this.num2 = double.Parse(this.num2_txtBox.Text);
            }
            catch 
            {
                MessageBox.Show("숫자를 입력하세요");
                return "숫자로 입력해주세요.";
            }
            
            switch (cal)
            {
                case eCalculator.Plus:
                    {
                        this.answer = num1 + num2;
                        return answer.ToString();
                    }

                case eCalculator.Minus:
                    {
                        this.answer = num1 - num2;
                        return answer.ToString();
                    }
                case eCalculator.Multi:
                    {
                        this.answer = num1 * num2;
                        return answer.ToString();
                    }
                case eCalculator.Divide:
                    {
                        try
                        {
                            this.answer = num1 / num2;
                            
                        }
                        catch (DivideByZeroException e)
                        {
                            

                            return "∞";
                        }
                        return answer.ToString();
                    }
            }
            return "";
        }

        private void Clear_Btn_Click(object sender, EventArgs e)
        {
            this.Clear();
        }
    }
}
