   M S B A M L   `   `   `  ����D  @lecture14, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:lecture14	lecture14  5   	       lecture14.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:lecture14     5   	     � Title$  
MainWindow��5   	   $	��504��6   $	��800��6)   .��5	      � $	��488��6   $	��Top=�6   .+�5
   
   �� -   5        $!��btnMainProgramExceptionTest��5
      $!��main program exception test��64   $
��Left=�6Z   $��	32,29,0,0q�6u   $	��Top=�6�   $��37��6�   $	��223��6�   6�   �� 5   
   -   5         ��Name$  btnDispatcherExceptionTest��5      $��dispatcher exception test��65   $��Center=�6Y   $��0,29,0,0q�6v   $	��Top=�6�   $��37��6�   $	��224��6�   6�   �� 5   
   -   5        $' !btnDispatcherExceptionInsideATask��5      $-��'dispatcher inside a task exception test��6<   $
��Left=�6n   $��
533,29,0,0q�6�   $	��Top=�6�   $��37��6�   $	��224��6�   6�   �� 5   
   -   5        $* $btnTestUnhandledExceptionInsideATask��5      $,��&test unhandled exception inside a task��6?   $
��Left=�6p   $��	32,85,0,0q�6�   $	��Top=�6�   $��37��6�   $	��224��6�   6�   �� 5   
   -   5        $��btnStartCrawling��5   *   $��start crawling��6   $
��Left=�6B   $��
32,249,0,0q�6]   $	��Top=�6q   $��46��6�   $	��224��6�   6�   �� 5   
   -   5        $��
txtRootUrl��5      $��https://www.toros.edu.tr��6$   $��32,144,256,250q�6D   �� 5   
   -   5        $��	lblStatus��5      $��Label��6!   $
��Left=�61   $��288,259,0,0q�6L   $	��Top=�6a   $	��469��6y   �� 5   
   -   5        $ ��btnStartWaitingToBeCrawled��5   �   $#��display waiting to be crawled��6   $
��Left=�69   $��
32,370,0,0q�6T   $	��Top=�6h   $	��179��6�   $��51��6�   6�   �� 5   
   -	   5        $��cmbWhichHashSet��5      $
��Left=�6*   $��
32,325,0,0q�6E   $	��Top=�6Y   $	��120��6q   �� 5   
   -
   5        $��txtNewTaskCount��5      $
��Left=�6)   $��
308,85,0,0q�6D   $-��'set number of concurrent crawling tasks��6X    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��214��6�   �� 5   
   -   5        $ btnSetCrawlingTaskCount��5      $��set new count��62   $
��Left=�6J   $��
550,85,0,0q�6e   $	��Top=�6y   $��33��6�   $	��144��6�   6�   5      5      