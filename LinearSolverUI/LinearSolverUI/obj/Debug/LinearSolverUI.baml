   M S B A M L   `   `   `  ����  LinearSolverUI    LinearSolverUI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  LinearSolver��5   	   $	��600��6   $	��800��6+    � 
ResizeMode$ NoResize=�67    � WindowStyle$ 
ToolWindow=�6M    � WindowStartupLocation$ CenterScreen=�6f   .��5      � -   5         �Name$ mainContainer��5      $��0q�6"   .+�5   
   �� -   5         ��Name$ equList��5      $
��Left=�6#   $	��300��6>   $��	50,70,0,0q�6K   $	��Top=�6^   $	��325��6v   $y�Hidden=�6�   �� 5   
   -   5         ��Name$ equBox��5      $��25��6"   $��100,30,100,0q�6.    �� TextWrapping$
 Wrap=�6D   $	��Top=�6X   $��0,0,0,1q�6p   $��Bottom=�6�   $��Type Your Equation Here��6  
�������$��Center=�6J   �� TextDecorations 6�   Z� 6n  �� 5   
   -   5        	 ��Name$	 newCstBt��5      $��New Constraints��6#   $��
342,70,0,0q�6=   $	��Top=�6Q   $	��110��6i   $��20��6u   $��1q�6�   
�������$
��Left=�6�   �� 5	   
   -   5        $	 slvBt��5	      $��Solve��6    $
��Left=�60   $��272,392,0,0q�6K   $	��Top=�6`   $��75��6x   
���   �
���   �
�������6�   �� 5
   
   -   5        $	 slvStpBt��5
      $��Solve by Step��6#   $
��Left=�6;   $��472,392,0,0q�6V   $	��Top=�6k   $��98��6�   
�������
���   �
���   �6�   �� 5   
   -   5        
 ��Name$
 isObj��5      $��Objetive Function��6"   $
��Left=�6>   $��336,101,0,0q�6Y   $	��Top=�6n   $	��125��6�   $��20��6�   $��1q�6�   $��0q�6�    d� 	IsChecked$
 TrueY�6�   �� 5   
   $��Objective Function: ��6   $
��Left=�6/   $��135,131,0,0q�6J   $	��Top=�6_   $	��139��6w   $��Right=�6�   �� 5   
   $��Constraint(s): ��6   $
��Left=�6*   $��135,159,0,0q�6E   $	��Top=�6Z   $	��139��6r   $��Right=�6~   �� 5   
   -	   5         ��Name$ srcollViewer��5      $
��Left=�6-   $	��250��6H   $��272,131,0,0q�6U   $	��Top=�6j   $	��300��6�    �� PanningMode$
 Both=�6�   �� 5   
   -
   5         ��Name$ noticeLb��5      $
��Left=�6&   $��135,426,0,0q�6A   $
��Wrap=�6V   $	��Top=�6j   $��52��6�   $	��560��6�    �� TextAlignment$ Center=�6�   �� 5   
   -   5        $��Reset��5      $
��Left=�6!   $��698,543,0,0q�6<   $	��Top=�6Q   $��75��6i   
���  ��
���   �6�   �� 5   
   -   5         ��Name$ enterNoticeLb��5      $��Press [ENTER] to submit��6'   $
��Left=�6I   $��
322,10,0,0q�6d   $	��Top=�6x   ���	#FFC6C6C6$y�Hidden=�6�   ��6�   Z� 6�   5      5      