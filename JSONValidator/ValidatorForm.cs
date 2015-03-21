﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONValidator
{
    public partial class ValidatorForm : Form
    {
        public ValidatorForm()
        {
            InitializeComponent();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            jsonTextBox.Text = "";
            resultTextBox.Text = "";
        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start");
            List<Token> TokensList = new List<Token>();
            Lexer l = new Lexer();
            Parser p = new Parser();
            bool failed = false;
            try 
	        {	        
		        failed = l.lexer(jsonTextBox.Text, ref TokensList);
                p.parse(TokensList);                
	        }
	        catch (ParseJSONException pje){
                failed = true;
                resultTextBox.Text = pje.Message + "\n"  + " Validation failed."; 
	        }
           
            if (!failed)
                resultTextBox.Text = "Json is valid.";
            failed = false;
            Console.WriteLine("Finished");

        }
    }
}
