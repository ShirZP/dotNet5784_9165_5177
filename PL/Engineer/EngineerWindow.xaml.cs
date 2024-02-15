using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty EngineerProperty = DependencyProperty.Register(
                                                                                        "CurrentEngineer",
                                                                                        typeof(BO.Engineer),
                                                                                        typeof(EngineerWindow),
                                                                                        new PropertyMetadata(null));


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public EngineerWindow(int id = 0)
        {
            InitializeComponent();

            if (id == 0) 
            {
                CurrentEngineer = new BO.Engineer(0, "", "", BO.EngineerExperience.Beginner, null, null);
            }
            else
            {
                try
                {
                    CurrentEngineer = s_bl?.Engineer.Read(id)!;
                }
                catch(BO.BlDoesNotExistException ex)
                { 
                    MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                try
                {
                    MessageBoxResult messageBoxResult;

                    if(button.Content.ToString() == "Add")
                    {
                        s_bl.Engineer.Create(CurrentEngineer);
                        messageBoxResult = MessageBox.Show($"Engineer {CurrentEngineer.ID} added Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        s_bl.Engineer.Update(CurrentEngineer);
                        messageBoxResult = MessageBox.Show($"Engineer {CurrentEngineer.ID} updated Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
