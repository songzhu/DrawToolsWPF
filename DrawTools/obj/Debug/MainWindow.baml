   M S B A M L   `   `   `  ����W  SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35eDclr-namespace:System.Windows.Media.Effects;assembly=presentationcoreSystem.Windows.Media.Effects  5      G CDrawToolsLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:DrawToolsLib;assembly=DrawToolsLibDrawToolsLib 5      K GUtilities, Version=1.0.6758.37592, Culture=neutral, PublicKeyToken=null8*clr-namespace:Utilities;assembly=Utilities	Utilities 5      J3clr-namespace:Petzold.Converters;assembly=UtilitiesPetzold.Converters 5      K GDrawTools, Version=1.0.6764.25932, Culture=neutral, PublicKeyToken=null%clr-namespace:DrawTools	DrawTools 5         DrawTools.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      XeffectsDclr-namespace:System.Windows.Media.Effects;assembly=presentationcore      5      @lib0clr-namespace:DrawToolsLib;assembly=DrawToolsLib      5      ;util*clr-namespace:Utilities;assembly=Utilities      5      J
libconvert3clr-namespace:Petzold.Converters;assembly=Utilities      5      )localclr-namespace:DrawTools      5      $	��300��5	      $	��300��6     J� CommandBindings  5      )   #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.New��5      6;     5   
   -   5        $ ApplicationCommands.Print��5      6=     5   
   -   5        $ ApplicationCommands.Close��5      6=     5   
   -   5        $ ApplicationCommands.Save��5      6<     5   
   -   5        $  ApplicationCommands.SaveAs��5      6>     5   
   -   5        $ ApplicationCommands.Open��5      6<     5   
   -   5        $ ApplicationCommands.Undo��5      6<     5   
   -   5        $ ApplicationCommands.Redo��5      6<     5   
   -	   5        $ ApplicationCommands.Help��5      6<    � 	Resources 5      $  DrawToolsLib.ToolTypeConverter   convToolType1  +Petzold.Converters.DoubleToDecimalConverter  convDoubleDecimal!  Utilities.FontSizeConverter  convFont  	bmpEffect �� 
KernelType �� Radius%  Utilities.ColorToBrushConverter  convColorToBrush  toolbarImageStyle �� 
TargetType �� Triggers �� Binding �� Path	 �� RelativeSource
 �� Mode �� AncestorType �� AncestorLevel �� Value �� Property �� Value  fontTextBlockStyle �� ElementName �� Mode��%#  &	        &	      &	 &     &	 9     &	 c     &	 v     7  &	 I      5   
   6      5   
   6.     5   
   6!   �� 5    
   $	 Box=�65   $ 2Q�6F     5"   
   6%   �� 5%   
   " ��6*    5&      �� 5'       5(      �� $ 	IsEnabled�	 �� $
 FindAncestor=�" ��$ 1��	 False5'      .a�5,      ��  �@J�BitmapEffect8   6;   5-      5/      �� 52   
   " ��6+   .�53      ��  � �� 6+   �� $ drawingCanvas��$ TextFontFamilyName�$ OneWay=��� 54       � �� 6*   �� $ drawingCanvas��$ TextFontStyle�$ OneWay=��� 55       � �� 6+   �� $ drawingCanvas��$ TextFontWeight�$ OneWay=��� 56       � �� 6,   �� $ drawingCanvas��$ TextFontStretch�$ OneWay=�57      58      .��5;      ]� ��. .+�5=   
   o� $	��Top=�6   .@�5>      m� $
��File��6   .@�5?      m� $	��New��6    m� Command$ ApplicationCommands.New��6(    m� Icon 5@      �� 5A      $��Images/NewDocumentHS.png��6    5C      m� 5D      $
��Open��6   $ ApplicationCommands.Open��6)    5E      �� 5F      $��Images/openHS.png��6    5H      m� 5I      $
��Save��6   $ ApplicationCommands.Save��6)    5J      �� 5K      $��Images/saveHS.png��6    5M      m� 5N      $��
Save As...��6   $  ApplicationCommands.SaveAs��6/    5O      �� 5Q      $��Images/Transparent.png��6    5S      �� 5T      m� 5U      $��Print��6   $ ApplicationCommands.Print��6*    5V      �� 5W      $��Images/PrintHS.png��6    5Y      �� 5Z      m� 5[      -
   5        $��menuFileRecentFiles��5[      $��Recent Files��67    5\      �� 5]      $��Images/Transparent.png��6    5_      �� 5`      m� 5a      $
��Exit��6   $ ApplicationCommands.Close��6)    5b      �� 5c      $��Images/Transparent.png��6    5e      5f      m� 5h      -   5        $��menuEdit��5h   %   $
��Edit��6   .@�5i      m� -   5        $��menuEditSelectAll��5i   /   $��
Select All��6   6/   m� 5j      -   5        $��menuEditUnselectAll��5j   1   $��Unselect All��6   61   m� 5k      -   5        $��menuEditDelete��5k   +   $��Delete��6   6+   m� 5l      -   5        $��menuEditDeleteAll��5l   /   $��
Delete All��6   6/   �� 5m      m� 5n      -   5        $��menuEditMoveToFront��5n   2   $��Move to Front��6   62   m� 5o      -   5        $��menuEditMoveToBack��5o   1   $��Move to Back��6   61   �� 5p      m� 5q      -   5        $��menuEditUndo��5q   )   $
��Undo��6   $ ApplicationCommands.Undo��6>   m� 5r      -   5        $��menuEditRedo��5r   )   $
��Redo��6   $ ApplicationCommands.Redo��6=   m� 5s      -   5        $��menuEditSetProperties��5s   3   $��Set Properties��6   63   5t      m� 5v      -   5        $��	menuTools��5v   %   $
��Draw��6   .@�5w      m� -   5        $��menuToolsPointer��5w   ,   $��Pointer��6   
 � Tag$ Pointer��6D   m� 5x      -   5        $��menuToolsRectangle��5x   .   $��	Rectangle��6   $ 	Rectangle��6H   m� 5y      -   5        $��menuToolsEllipse��5y   ,   $��Ellipse��6   $ Ellipse��6D   m� 5z      -   5        $��menuToolsLine��5z   )   $
��Line��6   $
 Line��6>   m� 5{      -   5        $��menuToolsPencil��5{   +   $��Pencil��6   $ PolyLine��6B   m� 5|      -   5        $��menuToolsText��5|   )   $
��Text��6   $
 Text��6>   5}      m� 5      $
��Help��6   .@�5�      m� $��About DrawTools��6   $ ApplicationCommands.Help��64    5�      �� 5�      $��Images/About.bmp��6    5�      5�      5�      `� 5�   
   $	��Top=�6   .�5�      c� .@�5�      �� $��ApplicationCommands.New��6    � ToolTip$	 New��6;   .��5�      �� $��Images/NewDocumentHS.png��6   5�      �� 5�      $��ApplicationCommands.Open��6   $
 Open��6<   .��5�      �� $��Images/openHS.png��6   5�      �� 5�      $��ApplicationCommands.Save��6   $
 Save��6<   .��5�      �� $��Images/saveHS.png��6   5�      �� 5�      d� 5�      -   5        $��buttonToolPointer��5�      $ Pointer��69   $ Pointer��6K    d� 	IsChecked 5�      �� $ drawingCanvas��$
 Tool�$ OneWay=� �� 	Converter# [   �� ConverterParameter$ Pointer��.��5�      �� $��Images/PointerHS.png��6   5�      d� 5�      -   5        $��buttonToolRectangle��5�      $ 	Rectangle��6:   $ 	Rectangle��6N    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $ 	Rectangle��.��5�      �� $��Images/rectangle.png��6   5�      d� 5�      -   5        $��buttonToolEllipse��5�      $ Ellipse��68   $ Ellipse��6J    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $ Ellipse��.��5�      �� $��Images/ellipse.png��6   5�      d� 5�      -   5        $��buttonToolLine��5�      $
 Line��65   $
 Line��6D    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $
 Line��.��5�      �� $��Images/line.png��6   5�      d� 5�      -    5        $��buttonToolPencil��5�      $ Pencil��67   $ PolyLine��6H    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $ PolyLine��.��5�      �� $��Images/pencil.png��6   5�      d� 5�      -!   5        $��buttonToolEraser��5�      $ Eraser��67   $ Eraser��6H    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $ Eraser��.��5�      �� $��Images/Eraser.png��6   5�      d� 5�      -"   5        $��buttonToolText��5�      $
 Text��65   $
 Text��6D    5�      �� $ drawingCanvas��$
 Tool�$ OneWay=�# [  $
 Text��.��5�      �� $��Images/Text.png��6   5�      �� 5�      �� 5�      $��ApplicationCommands.Undo��6   $
 Undo��6=   {�5�      �� $ drawingCanvas��$ CanUndo�$ OneWay=�.��5�      �� $��Images/Edit_UndoHS.png��6   #��[ 5�      5�      �� 5�      $��ApplicationCommands.Redo��6   $
 Redo��6=   {�5�      �� $ drawingCanvas��$ CanRedo�$ OneWay=�.��5�      �� $��Images/Edit_RedoHS.png��6   #��[ 5�      5�      �� 5�      �� 5�      -#   5        $��buttonPropertiesFont��5�      $
 Font��65   .��5�      ��  �� Orientation$ 
Horizontal=�6!   .+�5�      �� #��[ 6$   ��5�      �� $ drawingCanvas��$ TextFontFamilyName�$ OneWay=��� 5�      #��[ 6$   $�� (��5�      �� 5�      #��[ 6$   ��5�      �� $ drawingCanvas��$ TextFontSize�$ OneWay=�# [ �� 5�      #��[ 6$   $��)��5�      5�      5�      �� 5�      �� 5�      $��      ��6   $��5,5,3,5q�6*   $ Color��6;   ��5�      �� $ drawingCanvas��$ ObjectColor�$ OneWay=�# [ �� 5�      -$   5        $��buttonPropertiesColor��5�      $ Color��66   .��5�      �� �� 5�      .�6   Color6&   5�      5�      �� 5�      �� 5�      $��Center=�6   $��5,0,2,0q�67   $ 
Line Width��6H   Width5�      �� 5�      -%   5        $��comboPropertiesLineWidth��5�      $ 
Line Width��6;   $��40��6P    �� 
IsEditable .  $��5,0,0,0q�5�      �� 5�      �� 5�      $��ApplicationCommands.Help��6   $ About��6<   .��5�      �� $��Images/About.bmp��6   5�      5�      5�         DrawToolsLib.DrawingCanvas  5�   
   -&   5          Name$ drawingCanvas��5�      
���W�.� J� IsManipulationEnabled . 5�      5G     5H     