using PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MailView : Window
    {
        public MailView(MailViewModel mailViewModel)
        {
            InitializeComponent();
            DataContext = mailViewModel;
        }


    }
}
