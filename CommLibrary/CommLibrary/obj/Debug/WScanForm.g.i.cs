﻿#pragma checksum "..\..\WScanForm.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "527627D3493DB5ACC8BBCB7E6D69BB7F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CommLibrary {
    
    
    /// <summary>
    /// WScanForm
    /// </summary>
    public partial class WScanForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox scanBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar pbar;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ipBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox portBox;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button beginScanBtn;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\WScanForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button beginClose;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CommLibrary;component/wscanform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\WScanForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\WScanForm.xaml"
            ((CommLibrary.WScanForm)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 6 "..\..\WScanForm.xaml"
            ((CommLibrary.WScanForm)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.scanBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 24 "..\..\WScanForm.xaml"
            this.scanBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.scanBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.pbar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 4:
            this.ipBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.portBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.beginScanBtn = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\WScanForm.xaml"
            this.beginScanBtn.Click += new System.Windows.RoutedEventHandler(this.beginScanBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.beginClose = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\WScanForm.xaml"
            this.beginClose.Click += new System.Windows.RoutedEventHandler(this.beginClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

