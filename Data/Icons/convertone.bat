set INKSCAPE=..\..\Ignore\inkscape\inkscape.com
set ANDROID_PATH=..\..\AideDeJeu\AideDeJeu.Android\Resources\
set UWP_PATH=..\..\AideDeJeu\AideDeJeu.UWP\

%INKSCAPE% -z -e %ANDROID_PATH%drawable\%1 -w 72 -h 72 %2
%INKSCAPE% -z -e %ANDROID_PATH%drawable-hdpi\%1 -w 72 -h 72 %2
%INKSCAPE% -z -e %ANDROID_PATH%drawable-xhdpi\%1 -w 96 -h 96 %2
%INKSCAPE% -z -e %ANDROID_PATH%drawable-xxhdpi\%1 -w 144 -h 144 %2
%INKSCAPE% -z -e %UWP_PATH%%1 -w 144 -h 144 %2

