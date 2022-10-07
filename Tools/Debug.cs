using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CPU_Soft_Rasterization
{
    public static class Debug
    {
        private static TextBlock m_textBlock;

        public static void BindTextBox(TextBlock textBlock)
        {
            m_textBlock = textBlock;
        }
        public static void Log(string text) {


            m_textBlock.Dispatcher.BeginInvoke(() =>
            {
                m_textBlock.Text += text;
            });
        }
        public static void LogError(string text) {


            m_textBlock.Text = text;
        }

               



    }
}
