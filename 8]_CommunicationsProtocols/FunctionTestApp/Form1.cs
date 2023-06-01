using System;
using System.Windows.Forms;
using static XkartsCommonFunctions.Colours;

namespace FunctionTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                testfunc();

                var steve = GetColoursList();
                foreach (var item in steve)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {

            }
            

        }
    }
}
