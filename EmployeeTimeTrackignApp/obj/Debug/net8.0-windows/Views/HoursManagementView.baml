   M S B A M L   `   `   `  ����R  NEmployeeTimeTrackignApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:EmployeeTimeTrackignApp.ViewsEmployeeTimeTrackignApp.Views  5      W0clr-namespace:EmployeeTimeTrackignApp.ViewModels"EmployeeTimeTrackignApp.ViewModels  5      7    1EmployeeTimeTrackignApp.Views.HoursManagementView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:EmployeeTimeTrackignApp.Views     5      G
viewmodels0clr-namespace:EmployeeTimeTrackignApp.ViewModels     5      -   5        $��HoursManagement��5      $
|�True��5      .��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $	��280 �6   �� 5      $	��160 �6   .+�5   
   �� $��0��5      $��10, 0q�5      .+�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $��* �6'   .+�5      �� $��0��5      $-��'Select the range for the working hours:��5      $��16(�5       $
��Bold"�5!      $��Center=�5"      $��Center=�5#      $��0, 20, 0,60q�5$      �� 5&      $��1��5'      $��30��5(      $��100, 10, 100, 50q�5)      $��Center=�5,      $��Center=�5-      ��5*      �� *WorkHoursRange+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem 5+      �� *SelectedRange+5-      5.      5/      �� 51   
   $��1��52      $��10, 0q�53      .+�55      &   System.Windows.Controls.DataGrid  -   5        $��ProjectsTable��56      $	��280��59      
������ $
��Auto=�5;      $
��Auto=�5<      $
��True��5=         AutoGenerateColumns$ False��5>         CanUserAddRows$ False��5?      ��57      �� *AllWorkHoursAdded+ 58      �� *SelectedWorkHours+$ TwoWay=�   Columns 5@      4  .System.Windows.Controls.DataGridTemplateColumn  5A      ,  &System.Windows.Controls.DataGridColumn   Header$ Check��5B      	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 50 5C      
   CellTemplate
 5D      �� 5E      .R�5F   "   �� $��Center=�5G   +   $��Center=�5H   +    d� 	IsChecked 5F   +   �� *SelectedCheck+$ PropertyChanged=�5H   +   5I      5K        5M      $ 
Project ID��5N      $	 80 5O      
 5P      �� 5Q      .R�5R   "   �� $��Center=�5S   +   $��Center=�5T   +   ��5R   ,   �� *	ProjectID+5T   +   5U      5W        5Y      $ Employee ID��5Z      $	 80 5[      
 5\      �� 5]      .R�5^   "   �� $��Center=�5_   +   $��Center=�5`   +   ��5^   ,   �� *
EmployeeID+5`   +   5a      5c        5e      $ Hours��5f      $	 50 5g      
 5h      �� 5i      .R�5j   "   �� $��Center=�5k   +   $��Center=�5l   +   ��5j   ,   �� *
AddedHours+5l   +   5m      5o        5q      $ 
Created at��5r      $		 140 5s      
 5t      �� 5u      .R�5v   "   �� $��Center=�5w   +   $��Center=�5x   +   ��5v   ,   �� *	CreatedAt+5x   +   5y      5{        5}      $ Status��5~      $	 70 5      
 5�      �� 5�      .R�5�   "   �� $��Center=�5�   +   $��Center=�5�   +   ��5�   ,   �� *Status+5�   +   5�      5�        5�      $ Comment��5�      $		 287 5�      
 5�      �� 5�      .R�5�   "   �� $��Center=�5�   +   $��Center=�5�   +   ��5�   ,   �� *	Comment+5�   +   5�      5�      5�      5�      �� 5�   
   $��2��5�      $��10, 0q�5�      .+�5�      �  5�      �� 5�      $��* �6'   �� 5�      $��* �6'   �� 5�      $��* �6'   .+�5�      �� $��0��5�      $��Accept��5�      $��16(�5�      $
��Bold"�5�      $��40��5�      $	��120��5�      $��Center=�5�      $��Center=�5�      $��2q�5�      $��0, 80, 0, 40q�5�       � ToolTip$ Accept working hours��5�      ��5�      �� *AcceptWorkHoursCommand+�� 5�      $��1��5�      $"��Enter comment for rejection:��5�      $��16(�5�      $
��Bold"�5�      $��Center=�5�      $��
0, 5, 0, 0q�5�      �� 5�      -   5        $��WHCommentTextBox��5�      $��1��5�      $��0, 40, 0, 50q�5�      $��70��5�       �� TextWrapping$
 Wrap=�5�      " �� VerticalScrollBarVisibility$
 Auto=�5�      $ �� HorizontalScrollBarVisibility$ Disabled=�5�      $ Enter your comment��5�      ��5�      �� *WorkHoursRejectionComment+$ TwoWay=�$ PropertyChanged=�.��5�   2   5�      �� 5�      $��2��5�      $��Reject��5�      $��16(�5�      $
��Bold"�5�      $��40��5�      $	��120��5�      $��Center=�5�      $��Center=�5�      $��2q�5�      $��0, 80, 0, 40q�5�      $ Reject working hours��5�      ��5�      �� *RejectWorkHoursCommand+5�      5�      5�      5�      