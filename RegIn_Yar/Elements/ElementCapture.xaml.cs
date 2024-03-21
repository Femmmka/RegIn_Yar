﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace RegIn_Yar.Elements
{
    /// <summary>
    /// Логика взаимодействия для ElementCapture.xaml
    /// </summary>
    public partial class ElementCapture : UserControl
    {
        public CorrectCapture HandlerCorrectCapture;
        public delegate void CorrectCapture();
        string StrCapture = "";
        int ElementWidth = 280;
        int ElementHeigth = 50;

        public ElementCapture()
        {
            InitializeComponent();
            CreateCapture();
        }
        public void CreateCapture()
        {
            InputCapture.Text = "";
            Capture.Children.Clear();
            StrCapture = "";
            CreateBackground();
            Background();
        }
        #region CreateCapture
        void CreateBackground()
        {
            Random ThisRandom = new Random();
            for (int i = 0; i < 100; i++)
            {
                int back = ThisRandom.Next(0, 10);
                Label LBackground = new Label()
                {
                    Content = back,
                    FontSize = ThisRandom.Next(10, 16),
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(100, (byte)ThisRandom.Next(0, 255), (byte)ThisRandom.Next(0, 255), (byte)ThisRandom.Next(0, 255))),
                    Margin = new Thickness(ThisRandom.Next(0, ElementWidth - 20), ThisRandom.Next(0, ElementHeigth - 20), 0, 0)
                };
                Capture.Children.Add(LBackground);
            }
        }
        void Background()
        {
            Random ThisRandom = new Random();
            for (int i = 0; i < 4; i++)
            {
                int back = ThisRandom.Next(0, 10);
                Label lCode = new Label()
                {
                    Content = back,
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)ThisRandom.Next(0, 255), (byte)ThisRandom.Next(0, 255), (byte)ThisRandom.Next(0, 255))),
                    Margin = new Thickness(ElementWidth / 2 - 60 + i * 30, ThisRandom.Next(-10, 10), 0, 0)
                };
                StrCapture += back.ToString();
                Capture.Children.Add(lCode);
                }
            }
        #endregion
        public bool OnCapture()
        {
            return StrCapture == InputCapture.Text;
        }
        private void EnterCapture(object sender,KeyEventArgs e)
        {
            if (InputCapture.Text.Length == 4)
                if (!OnCapture())
                    CreateCapture();
                else if (HandlerCorrectCapture != null)
                    HandlerCorrectCapture.Invoke();
        }
    }
    } 

