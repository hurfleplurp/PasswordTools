﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PGen.Utils;

namespace PGen
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private short passwordLength = 12;

        private static HashSet<string> wordlist = new HashSet<string>();
        public MainWindow()
        {
            this.InitializeComponent();
        }

        public enum PasswordScore
        {
            Blank = -1,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static PasswordScore CheckStrength(string password)
        {
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 5)
                return PasswordScore.VeryWeak;
            if (password.Length < 8)
                return PasswordScore.Weak;

            if (wordlist.Count == 0)
                WordlistOperations.CreateUniqueShitlist(ref wordlist);

            /********* HASHSET INTERSECT SEARCH ********/
            //var sw = Stopwatch.StartNew();

            var pwdString = ComplexityCalc.GetConsecutiveSubstrings(password);

            var wordsInShitlist = pwdString.Intersect(wordlist);

            //sw.Stop();
            //TimeSpan t1 = sw.Elapsed;
            var badWords = wordsInShitlist.Count();

            /********* .NET LINQ SEARCH ********/
            //var sw2 = Stopwatch.StartNew();
            //var wordsInShitlist2 = new HashSet<string>();
            
            //foreach(var shit in ComplexityCalc.wordlist)
            //    if (password.Contains(shit))
            //        wordsInShitlist2.Add(shit);

            
            //sw2.Stop();
            //TimeSpan t2 = sw2.Elapsed;

            /********* Z-ALGORITHM SEARCH ********/
            //var sw3 = Stopwatch.StartNew();
            //var wordsInShitlist3 = new HashSet<string>();

            //foreach (var shit in ComplexityCalc.wordlist)
            //    if (ComplexityCalc.zAlgorithm(shit, password))
            //        wordsInShitlist3.Add(shit);

            //sw3.Stop();
            //TimeSpan t3 = sw3.Elapsed;

            var score = 0;

            if (badWords > 0)
                for (var x = 1; x < badWords; x++)
                {
                    score--;
                    if (x > 2)
                        break;
                }

            if (password.Length > 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (password.Length >= 16)
                score++;
            if (password.Length >= 20)
                score+=2;
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            // Limit highest score to 5
            if (score > 5)
                score = 5;
            // Limit lowest score to 1
            if (score < 1)
                score = 1;

            return (PasswordScore)score;
        }

        public static Tuple<string, SolidColorBrush> EnumStyler(Enum enumToStyle)
        {
            Tuple<string, SolidColorBrush> styleInfo = Tuple.Create(Enum.GetName(enumToStyle.GetType(), enumToStyle), Brushes.Black);

            string enumTypeAsString = enumToStyle.GetType().ToString();

            switch (enumTypeAsString)
            {
                case "PGen.MainWindow+PasswordScore":
                    {
                        PasswordScore scoreToStyle = (PasswordScore)enumToStyle;

                        switch(scoreToStyle)
                        {
                            case PasswordScore.Blank:
                                return Tuple.Create("", Brushes.Black);
                            case PasswordScore.VeryWeak:
                                return Tuple.Create("Very Weak", Brushes.Red);
                            case PasswordScore.Weak:
                                return Tuple.Create("Weak", Brushes.OrangeRed);
                            case PasswordScore.Medium:
                                return Tuple.Create("Medium", Brushes.Gold);
                            case PasswordScore.Strong:
                                return Tuple.Create("Strong", Brushes.MediumSpringGreen);
                            case PasswordScore.VeryStrong:
                                return Tuple.Create("Very Strong", Brushes.Lime);
                        }
                        break;
                    }
            }

            return styleInfo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Obfuscator obfuscator = new Obfuscator();

            obfuscator.Salt = UrlTextBox.Text.ToLower() + UserNameTextBox.Text.ToLower();
            obfuscator.RegEx = @RegexTextBox.Text;
            obfuscator.ReplacementText = ReplaceTextBox.Text;
            obfuscator.DesiredLength = passwordLength;
            obfuscator.GeneratePassword();

            RegexInvalidLabel.Visibility = Visibility.Visible;
            RegexInvalidLabel.Content = obfuscator.RegexStatus.Item1;
            RegexInvalidLabel.Foreground = obfuscator.RegexStatus.Item2;

            if (!string.IsNullOrEmpty(obfuscator.Password))
            {
                PasswordTextBox.Text = obfuscator.Password;
                Clipboard.SetText(obfuscator.Password);

                PasswordScore generatedPwdScore = CheckStrength(obfuscator.Password);
                Tuple<string, SolidColorBrush> enumStyle = EnumStyler(generatedPwdScore);
                GeneratedPwdStrLab.Visibility = Visibility.Visible;
                GeneratedPwdStrLab.Content = enumStyle.Item1;
                GeneratedPwdStrLab.Foreground = enumStyle.Item2;
            }
        }

        private void DesiredLengthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem lengthItem = (ComboBoxItem)DesiredLengthCombo.SelectedItem;
            this.passwordLength = Convert.ToInt16(lengthItem.Content);
        }

        private void label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDC3B3B");
        }

        private void label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF61616A");
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA82B2B");
        }
        private void labelMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFB0B0B6");
        }

        private void labelMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF61616A");
        }

        private void labelMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label clickedLabel = sender as Label;
            clickedLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF323236");
        }
        private void labelMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;;
        }

        private void headerLab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            pwdStrLab.Content = Enum.GetName(typeof(PasswordScore), CheckStrength(tb.Text));
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            PasswordScore generatedPwdScore = CheckStrength(tb.Text);
            Tuple<string, SolidColorBrush> enumStyle = EnumStyler(generatedPwdScore);
            GeneratedPwdStrLab.Visibility = Visibility.Visible;
            GeneratedPwdStrLab.Content = enumStyle.Item1;
            GeneratedPwdStrLab.Foreground = enumStyle.Item2;
        }

        private void pGenHeaderLab_MouseEnter(object sender, MouseEventArgs e)
        {
            var tab = (TabItem)tabControl.Items[0];
            tab.Visibility = Visibility.Visible;
            this.labelClose.Visibility = Visibility.Visible;
            this.labelMinimize.Visibility = Visibility.Visible;
        }

        private void PGen_MouseLeave(object sender, MouseEventArgs e)
        {
            var tab = (TabItem) tabControl.Items[0];
            tab.Visibility = Visibility.Hidden;
            this.labelClose.Visibility = Visibility.Hidden;
            this.labelMinimize.Visibility = Visibility.Hidden;
        }
    }
}
