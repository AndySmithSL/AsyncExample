﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonRun_Click(object sender, EventArgs e)
        {
            textBoxResults.Clear();

            // Call and await separately.  
            //Task<int> getLengthTask = AccessTheWebAsync();  
            //// You can do independent work here.  
            //int contentLength = await getLengthTask;  

            int contentLength = await AccessTheWebAsync();

            textBoxResults.Text += String.Format("\r\nLength of the downloaded string: {0}.\r\n", contentLength);
        }



        async Task<int> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.  
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the  
            // task you'll get a string (urlContents).  
            Task<string> getStringTask = client.GetStringAsync("https://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.  
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.  
            //  - AccessTheWebAsync can't continue until getStringTask is complete.  
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.  
            //  - Control resumes here when getStringTask is complete.   
            //  - The await operator then retrieves the string result from getStringTask.  
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.  
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.  
            return urlContents.Length;
        }

        private void DoIndependentWork()
        {
            textBoxResults.Text += "Working . . . . . . .\r\n";
        }

        private async void buttonRun2_Click(object sender, EventArgs e)
        {
            textBoxResults.Clear();

            Task<string> result = DoLoadsOfStuff();

            DoMoreStuff();

            textBoxResults.Text += await result;

        }

        private async Task<string> DoLoadsOfStuff()
        {
            int sum = 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    //textBoxResults.Text += i.ToString() + System.Environment.NewLine;
                    sum += i;
                    Thread.Sleep(1000);
                }
            });

            return sum.ToString();
        }

        private void DoMoreStuff()
        {
            for (int i = 0; i < 100; i++)
            {
                textBoxResults.Text += i.ToString() + System.Environment.NewLine;
                Thread.Sleep(10);
            }
        }
    }
}
