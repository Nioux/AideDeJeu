set INKSCAPE=..\..\Ignore\inkscape\inkscape.com
set ANDROID_PATH=..\..\AideDeJeu\AideDeJeu.Android\Resources\
set UWP_PATH=..\..\AideDeJeu\AideDeJeu.UWP\
set IOS_PATH=..\..\AideDeJeu\AideDeJeu.iOS\Media.xcassets\

goto ios

:android

%INKSCAPE% -z -e %ANDROID_PATH%drawable\%1.png -w 72 -h 72 %2.svg
%INKSCAPE% -z -e %ANDROID_PATH%drawable-hdpi\%1.png -w 72 -h 72 %2.svg
%INKSCAPE% -z -e %ANDROID_PATH%drawable-xhdpi\%1.png -w 96 -h 96 %2.svg
%INKSCAPE% -z -e %ANDROID_PATH%drawable-xxhdpi\%1.png -w 144 -h 144 %2.svg

:uwp

%INKSCAPE% -z -e %UWP_PATH%%1.png -w 144 -h 144 %2.svg

:ios

md %IOS_PATH%%1.imageset\
%INKSCAPE% -z -e %IOS_PATH%%1.imageset\%1.png -w 69 -h 69 %2.svg
